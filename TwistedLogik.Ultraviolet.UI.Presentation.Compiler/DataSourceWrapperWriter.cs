using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Compiler
{
    /// <summary>
    /// Contains methods for writing the source code for a data source wrapper.
    /// </summary>
    internal class DataSourceWrapperWriter : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceWrapperWriter"/> class.
        /// </summary>
        public DataSourceWrapperWriter()
        {
            LineCount = 1;

            strWriter = new StringWriter();
            txtWriter = new IndentedTextWriter(strWriter);           
        }
        
        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public override String ToString()
        {
            return strWriter.ToString();
        }

        /// <summary>
        /// Writes a block of text to the buffer.
        /// </summary>
        /// <param name="text">The text to write to the buffer.</param>
        public void Write(String text)
        {
            txtWriter.Write(text);
            LineCount += text.Count(c => c == '\n');
        }

        /// <summary>
        /// Writes a block of text to the buffer.
        /// </summary>
        /// <param name="fmt">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public void Write(String fmt, params Object[] args)
        {
            var text = String.Format(fmt, args);
            Write(text);
        }

        /// <summary>
        /// Writes a newline character to the buffer.
        /// </summary>
        public void WriteLine()
        {
            WriteLine(String.Empty);
        }

        /// <summary>
        /// Writes a block of text to the buffer, followed by a newline character.
        /// </summary>
        /// <param name="text">The text to write to the buffer.</param>
        public void WriteLine(String text)
        {
            if (text == "}")
                txtWriter.Indent--;

            var storedIndent = txtWriter.Indent;

            var pragma = text.StartsWith("#pragma");
            if (pragma)
                txtWriter.Indent = 0;

            txtWriter.WriteLine(text);
            LineCount += 1 + text.Count(c => c == '\n');

            if (pragma)
                txtWriter.Indent = storedIndent;

            if (text == "{")
                txtWriter.Indent++;
        }

        /// <summary>
        /// Writes a block of text to the buffer, followed by a newline character.
        /// </summary>
        /// <param name="fmt">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public void WriteLine(String fmt, params Object[] args)
        {
            var text = String.Format(fmt, args);
            WriteLine(text);
        }

        /// <summary>
        /// Writes a constructor for the specified data source wrapper.
        /// </summary>
        /// <param name="dataSourceWrapperInfo">A <see cref="DataSourceWrapperInfo"/> describing the data source for which to write a constructor.</param>
        public void WriteConstructor(DataSourceWrapperInfo dataSourceWrapperInfo)
        {
            WriteLine("public {0}({1} dataSource)", dataSourceWrapperInfo.DataSourceWrapperName, GetCSharpTypeName(dataSourceWrapperInfo.DataSourceType));
            WriteLine("{");

            WriteLine("this.dataSource = dataSource;");

            WriteLine("}");
        }

        /// <summary>
        /// Writes an implementation of the <see cref="IDataSourceWrapper"/> interface.
        /// </summary>
        /// <param name="dataSourceWrapperInfo">A <see cref="DataSourceWrapperInfo"/> describing the data source wrapper for which to write an <see cref="IDataSourceWrapper"/> implementation.</param>
        public void WriteIDataSourceWrapperImplementation(DataSourceWrapperInfo dataSourceWrapperInfo)
        {
            WriteLine("public override Object WrappedDataSource");
            WriteLine("{");

            WriteLine("get { return dataSource; }");

            WriteLine("}");
            WriteLine("private readonly {0} dataSource;", GetCSharpTypeName(dataSourceWrapperInfo.DataSourceType));
        }

        /// <summary>
        /// Writes a wrapper method for the specified method.
        /// </summary>
        /// <param name="method">The method for which to write a wrapper.</param>
        public void WriteWrapperMethod(MethodInfo method)
        {
            var isStatic = method.IsStatic;
            var parameters = method.GetParameters();

            var methodStaticQualifier = isStatic ? "static " : String.Empty;
            var methodParameterList = String.Join(", ", parameters.Select(x => GetParameterText(x)));
            var methodArgumentList = String.Join(", ", parameters.Select(x => GetArgumentText(x)));
            var methodGenericArgumentsList = method.IsGenericMethod ? "<" + String.Join(", ", method.GetGenericArguments().Select(x => x.Name)) + ">" : String.Empty;
                       
            WriteLine("public {0}{1} {2}{3}({4})", methodStaticQualifier, GetCSharpTypeName(method.ReturnType), method.Name, methodGenericArgumentsList, methodParameterList);
            WriteGenericMethodConstraints(method);
            WriteLine("{");

            var target = isStatic ? GetCSharpTypeName(method.DeclaringType) : "dataSource";
            Write(method.ReturnType == typeof(void) ? String.Empty : "return ");
            WriteLine("{0}.{1}{2}({3});", target, method.Name, methodGenericArgumentsList, methodArgumentList);

            WriteLine("}");
        }

        /// <summary>
        /// Writes the specified method's list of generic constraints.
        /// </summary>
        /// <param name="method">The method for which to write generic constraints.</param>
        public void WriteGenericMethodConstraints(MethodInfo method)
        {
            var methodGenericArguments = method.GetGenericArguments();
            if (methodGenericArguments.Any())
            {
                txtWriter.Indent++;

                foreach (var arg in methodGenericArguments)
                {
                    var constraints = arg.GetGenericParameterConstraints();
                    var attributes = arg.GenericParameterAttributes;
                    if (attributes == GenericParameterAttributes.None && !constraints.Any())
                        continue;
                    
                    var buffer = new StringBuilder(String.Format("where {0} : ", arg.Name));

                    // class
                    if ((attributes & GenericParameterAttributes.ReferenceTypeConstraint) == GenericParameterAttributes.ReferenceTypeConstraint)
                    {
                        if (buffer.Length > 0)
                        {
                            buffer.Append(", ");
                        }
                        buffer.Append("class");
                    }
                    
                    // struct
                    if ((attributes & GenericParameterAttributes.NotNullableValueTypeConstraint) == GenericParameterAttributes.NotNullableValueTypeConstraint)
                    {
                        if (buffer.Length > 0)
                        {
                            buffer.Append(", ");
                        }
                        buffer.Append("struct");
                    }

                    // type constraints
                    foreach (var constraint in constraints)
                    {
                        if (constraint == typeof(ValueType))
                            continue;

                        if (buffer.Length > 0)
                        {
                            buffer.Append(", ");
                        }
                        buffer.Append(constraint.FullName);
                    }

                    // new()
                    if ((attributes & GenericParameterAttributes.DefaultConstructorConstraint) == GenericParameterAttributes.DefaultConstructorConstraint &&
                        (attributes & GenericParameterAttributes.NotNullableValueTypeConstraint) != GenericParameterAttributes.NotNullableValueTypeConstraint)
                    {
                        if (buffer.Length > 0)
                        {
                            buffer.Append(", ");
                        }
                        buffer.Append("new()");
                    }

                    WriteLine(buffer.ToString());
                }

                txtWriter.Indent--;
            }
        }

        /// <summary>
        /// Writes a wrapper property for the specified property.
        /// </summary>
        /// <param name="method">The method for which to write a wrapper.</param>
        public void WriteWrapperProperty(PropertyInfo property)
        {
            var getter = property.GetGetMethod(false);
            var setter = property.GetSetMethod(false);
            if (getter == null && setter == null)
                return;

            var isStatic = (getter ?? setter).IsStatic;
            var isIndexer = false;

            var propertyStaticQualifier = isStatic ? "static " : String.Empty;
            var propertyIndexerParameterList = String.Empty;
            var propertyIndexerArgumentList = String.Empty;

            var parameters = property.GetIndexParameters();
            if (parameters != null && parameters.Length > 0)
            {
                isIndexer = true;

                propertyIndexerParameterList = String.Format("[{0}]",
                    String.Join(", ", parameters.Select(x => GetParameterText(x))));

                propertyIndexerArgumentList = String.Format("[{0}]",
                    String.Join(", ", parameters.Select(x => GetArgumentText(x))));
            }

            WriteLine("public {0}{1} {2}{3}", propertyStaticQualifier, 
                GetCSharpTypeName(property.PropertyType), isIndexer ? "this" : property.Name, propertyIndexerParameterList);
            WriteLine("{");

            var target = isStatic ? GetCSharpTypeName(property.DeclaringType) : "dataSource";

            if (getter != null)
            {
                WriteLine("get {{ return {0}{1}{2}{3}; }}", target,
                    isIndexer ? String.Empty : ".", isIndexer ? String.Empty : property.Name, propertyIndexerArgumentList);
            }
            if (setter != null)
            {
                WriteLine("set {{ {0}{1}{2}{3} = value; }}", target, 
                    isIndexer ? String.Empty : ".", isIndexer ? String.Empty : property.Name, propertyIndexerArgumentList);
            }

            WriteLine("}");
        }

        /// <summary>
        /// Writes a wrapper property for the specified field.
        /// </summary>
        /// <param name="method">The method for which to write a wrapper.</param>
        public void WriteWrapperProperty(FieldInfo field)
        {
            var isStatic = field.IsStatic;

            var propertyStaticQualifier = isStatic ? "static " : String.Empty;

            WriteLine("public {0}{1} {2}", propertyStaticQualifier, GetCSharpTypeName(field.FieldType), field.Name);
            WriteLine("{");

            var target = isStatic ? GetCSharpTypeName(field.DeclaringType) : "dataSource";

            WriteLine("get {{ return {0}.{1}; }}", target, field.Name);
            if (!field.IsInitOnly && !field.IsLiteral)
            {
                WriteLine("set {{ {0}.{1} = value; }}", target, field.Name);
            }

            WriteLine("}");
        }

        /// <summary>
        /// Writes a property which wraps a binding expression.
        /// </summary>
        /// <param name="dataSourceWrapperInfo">A <see cref="DataSourceWrapperInfo"/> describing the data source for which to write an expression property.</param>
        /// <param name="expressionInfo">The binding expression for which to write a property.</param>
        /// <param name="id">The expression's identifier within the view model.</param>
        public void WriteExpressionProperty(DataSourceWrapperInfo dataSourceWrapperInfo, BindingExpressionInfo expressionInfo, Int32 id)
        {
            var isDependencyProperty = false;
            var isSimpleDependencyProperty = false;

            var expMemberPath = BindingExpressions.GetBindingMemberPathPart(expressionInfo.Expression);
            var expFormatString = BindingExpressions.GetBindingFormatStringPart(expressionInfo.Expression);

            var dprop = DependencyProperty.FindByName(expMemberPath, dataSourceWrapperInfo.DataSourceType);
            var dpropField = default(FieldInfo);
            if (dprop != null)
            {
                isDependencyProperty = true;

                dpropField =
                    (from prop in dprop.OwnerType.GetFields(BindingFlags.Public | BindingFlags.Static)
                     where
                       prop.FieldType == typeof(DependencyProperty) &&
                       prop.GetValue(null) == dprop
                     select prop).SingleOrDefault();

                if (dpropField == null)
                    throw new InvalidOperationException(PresentationStrings.CannotFindDependencyPropertyField.Format(dprop.OwnerType.Name, dprop.Name));

                if (String.IsNullOrEmpty(expFormatString))
                {
                    isSimpleDependencyProperty = true;
                }
            }

            WriteLine("[{0}(@\"{1}\", SimpleDependencyPropertyOwner = {2}, SimpleDependencyPropertyName = {3})]", typeof(CompiledBindingExpressionAttribute).FullName, expressionInfo.Expression.Replace("\"", "\"\""),
                isSimpleDependencyProperty ? "typeof(" + GetCSharpTypeName(dprop.OwnerType) + ")" : "null",
                isSimpleDependencyProperty ? "\"" + dprop.Name + "\"" : "null");

            WriteLine("public {0} __UPF_Expression{1}", GetCSharpTypeName(expressionInfo.Type), id);
            WriteLine("{");

            if (expressionInfo.GenerateGetter)
            {
                expressionInfo.GetterLineStart = LineCount;
                
                var getexp = isDependencyProperty ? String.Format("GetValue<{0}>({1}.{2})", 
                    GetCSharpTypeName(dprop.PropertyType), 
                    GetCSharpTypeName(dprop.OwnerType), dpropField.Name) : expMemberPath;

                expFormatString = String.IsNullOrEmpty(expFormatString) ? "null" : String.Format("\"{{0:{0}}}\"", expFormatString);

                if (expressionInfo.Type == typeof(String) || expressionInfo.Type == typeof(VersionedStringSource))
                {
                    WriteLine("get");
                    WriteLine("{");
                    WriteLine("return ({0})__UPF_ConvertToString({1}, {2});", GetCSharpTypeName(expressionInfo.Type), getexp, expFormatString);
                    WriteLine("}");
                }
                else
                {
                    WriteLine("get {{ return ({0})({1}); }}", GetCSharpTypeName(expressionInfo.Type), getexp);
                }

                expressionInfo.GetterLineEnd = LineCount - 1;
            }

            if (dprop != null && dprop.IsReadOnly)
                expressionInfo.GenerateSetter = false;

            if (expressionInfo.GenerateSetter)
            {
                var targetTypeName = expressionInfo.CS0266TargetType;
                var targetTypeSpecified = !String.IsNullOrEmpty(targetTypeName);

                expressionInfo.SetterLineStart = LineCount;
                if (isDependencyProperty)
                {
                    if (expressionInfo.NullableFixup)
                    {
                        WriteLine(targetTypeSpecified ? "set {{ SetValue<{0}>({1}.{2}, ({3})(value ?? default({0}))); }}" : "set {{ SetValue<{0}>({1}.{2}, value ?? default({0})); }}", 
                            GetCSharpTypeName(dprop.PropertyType), 
                            GetCSharpTypeName(dprop.OwnerType),
                            dpropField.Name, targetTypeName);
                    }
                    else
                    {
                        WriteLine(targetTypeSpecified ? "set {{ SetValue<{0}>({1}.{2}, ({3})(value)); }}" : "set {{ SetValue<{0}>({1}.{2}, value); }}", 
                            GetCSharpTypeName(dprop.PropertyType),
                            GetCSharpTypeName(dprop.OwnerType), 
                            dpropField.Name, targetTypeName);
                    }
                }
                else
                {
                    if (expressionInfo.NullableFixup)
                    {
                        WriteLine(targetTypeSpecified ? "set {{ {0} = ({2})(value ?? default({1})); }}" : "set {{ {0} = value ?? default({1}); }}",
                            expMemberPath, GetCSharpTypeName(Nullable.GetUnderlyingType(expressionInfo.Type)), targetTypeName);
                    }
                    else
                    {
                        WriteLine(targetTypeSpecified ? "set {{ {0} = ({1})(value); }}" : "set {{ {0} = value; }}", expMemberPath, targetTypeName);
                    }
                }

                expressionInfo.SetterLineEnd = LineCount - 1;
            }
            
            WriteLine("}");
        }

        /// <summary>
        /// Gets the C# name of the specified type, including by-ref specifications.
        /// </summary>
        public String GetCSharpTypeName(Type type)
        {
            if (type == typeof(void))
                return "void";

            var isByRef = type.IsByRef;
            if (isByRef)
                type = type.GetElementType();

            var name = type.IsGenericParameter ? type.Name : type.FullName;
            name = name.Replace('+', '.');

            if (type.IsGenericType)
            {
                var genericTypeDef = type.GetGenericTypeDefinition();
                var genericTypeName = genericTypeDef.FullName.Substring(0, genericTypeDef.FullName.IndexOf('`'));
                var genericArguments = type.GetGenericArguments();

                name = String.Format("{0}<{1}>", genericTypeName,
                    String.Join(", ", genericArguments.Select(x => GetCSharpTypeName(x))));
            }
            
            return isByRef ? "ref " + name : name;
        }

        /// <summary>
        /// Gets the source text for the specified parameter when it is part of a parameter list.
        /// </summary>
        public String GetParameterText(ParameterInfo parameter)
        {
            return GetCSharpTypeName(parameter.ParameterType) + " " + parameter.Name;
        }

        /// <summary>
        /// Gets the source text for the specified parameter when it is part of an argument list.
        /// </summary>
        public String GetArgumentText(ParameterInfo parameter)
        {
            if (parameter.IsOut)
            {
                return "out " + parameter.Name;
            }
            if (parameter.ParameterType.IsByRef)
            {
                return "ref " + parameter.Name;
            }
            return parameter.Name;
        }
        
        /// <summary>
        /// Gets the number of lines that have been written.
        /// </summary>
        public Int32 LineCount
        {
            get;
            private set;
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing"><c>true</c> if the object is being disposed; <c>false</c> if the object is being finalized.</param>
        protected virtual void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                SafeDispose.Dispose(txtWriter);
                SafeDispose.Dispose(strWriter);
            }
        }

        // State values.
        private readonly StringWriter strWriter;
        private readonly IndentedTextWriter txtWriter;
    }
}
