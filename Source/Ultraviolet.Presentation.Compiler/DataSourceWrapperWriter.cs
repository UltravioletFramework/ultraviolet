using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Compiler
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
            WriteLine("public {0}({1} dataSource, {2} namescope) : base(namescope)", dataSourceWrapperInfo.DataSourceWrapperName, 
                CSharpLanguage.GetCSharpTypeName(dataSourceWrapperInfo.DataSourceType),
                CSharpLanguage.GetCSharpTypeName(typeof(Namescope)));
            WriteLine("{");

            WriteLine("this.value = dataSource;");

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

            WriteLine("get { return value; }");

            WriteLine("}");

            WriteLine("private readonly {0} value;", CSharpLanguage.GetCSharpTypeName(dataSourceWrapperInfo.DataSourceType));
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
                       
            WriteLine("public {0}{1} {2}{3}({4})", methodStaticQualifier, CSharpLanguage.GetCSharpTypeName(method.ReturnType), method.Name, methodGenericArgumentsList, methodParameterList);
            WriteGenericMethodConstraints(method);
            WriteLine("{");

            var target = isStatic ? CSharpLanguage.GetCSharpTypeName(method.DeclaringType) : "this.value";
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
        /// <param name="property">The property for which to write a wrapper.</param>
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
                CSharpLanguage.GetCSharpTypeName(property.PropertyType), isIndexer ? "this" : property.Name, propertyIndexerParameterList);
            WriteLine("{");

            var target = isStatic ? CSharpLanguage.GetCSharpTypeName(property.DeclaringType) : "this.value";

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
        /// <param name="field">The field for which to write a wrapper.</param>
        public void WriteWrapperProperty(FieldInfo field)
        {
            var isStatic = field.IsStatic;

            var propertyStaticQualifier = isStatic ? "static " : String.Empty;

            WriteLine("public {0}{1} {2}", propertyStaticQualifier, CSharpLanguage.GetCSharpTypeName(field.FieldType), field.Name);
            WriteLine("{");

            var target = isStatic ? CSharpLanguage.GetCSharpTypeName(field.DeclaringType) : "this.value";

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
        /// <param name="state">The expression compiler's current state.</param>
        /// <param name="dataSourceWrapperInfo">A <see cref="DataSourceWrapperInfo"/> describing the data source for which to write an expression property.</param>
        /// <param name="expressionInfo">The binding expression for which to write a property.</param>
        /// <param name="id">The expression's identifier within the view model.</param>
        public void WriteExpressionProperty(IExpressionCompilerState state, DataSourceWrapperInfo dataSourceWrapperInfo, BindingExpressionInfo expressionInfo, Int32 id)
        {
            var isDependencyProperty = false;
            var isSimpleDependencyProperty = false;
            
            var expText = BindingExpressions.GetBindingMemberPathPart(expressionInfo.Expression);
            var expTarget = default(String);
            var expTargetType = dataSourceWrapperInfo.DataSourceType;
            var expFormatString = BindingExpressions.GetBindingFormatStringPart(expressionInfo.Expression);

            var targeted = GetExpressionTargetInfo(state, dataSourceWrapperInfo, 
                expressionInfo.Source, ref expText, out expTarget, out expTargetType);

            var dprop = DependencyProperty.FindByName(expText, expTargetType);
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
                {
                    throw new BindingExpressionCompilationErrorException(expressionInfo.Source, dataSourceWrapperInfo.DataSourceDefinition.DefinitionPath,
                        PresentationStrings.CannotFindDependencyPropertyField.Format(dprop.OwnerType.Name, dprop.Name));
                }

                if (String.IsNullOrEmpty(expFormatString) && !targeted)
                {
                    isSimpleDependencyProperty = true;
                }
            }

            WriteLine("[{0}(@\"{1}\", SimpleDependencyPropertyOwner = {2}, SimpleDependencyPropertyName = {3})]", typeof(CompiledBindingExpressionAttribute).FullName, expressionInfo.Expression.Replace("\"", "\"\""),
                isSimpleDependencyProperty ? "typeof(" + CSharpLanguage.GetCSharpTypeName(dprop.OwnerType) + ")" : "null",
                isSimpleDependencyProperty ? "\"" + dprop.Name + "\"" : "null");
            
            WriteLine("public {0} __UPF_Expression{1}", CSharpLanguage.GetCSharpTypeName(expressionInfo.Type), id);
            WriteLine("{");

            if (expressionInfo.GenerateGetter)
            {
                expressionInfo.GetterLineStart = LineCount;

                var getexp = default(String);
                if (isDependencyProperty)
                {
                    getexp = String.Format("{0}GetValue<{1}>({2}.{3})", expTarget,
                       CSharpLanguage.GetCSharpTypeName(dprop.PropertyType),
                       CSharpLanguage.GetCSharpTypeName(dprop.OwnerType), dpropField.Name);
                }
                else
                {
                    getexp = String.Format("{0}{1}", expTarget, expText);
                }

                var hasFormatString = !String.IsNullOrEmpty(expFormatString);
                expFormatString = hasFormatString ? String.Format("\"{{0:{0}}}\"", expFormatString) : "null";
                
                if (IsStringType(expressionInfo.Type) || (expressionInfo.Type == typeof(Object) && hasFormatString))
                {
                    WriteLine("get");
                    WriteLine("{");
                    WriteLine("var value = {0};", getexp);
                    WriteLine("return ({0})__UPF_ConvertToString(value, {1});", CSharpLanguage.GetCSharpTypeName(expressionInfo.Type), expFormatString);
                    WriteLine("}");
                }
                else
                {
                    WriteLine("get {{ return ({0})({1}); }}", CSharpLanguage.GetCSharpTypeName(expressionInfo.Type), getexp);
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
                    if (IsStringType(expressionInfo.Type))
                    {
                        WriteLine("set");
                        WriteLine("{");
                        WriteLine("var current = {0}GetValue<{1}>({2}.{3});", 
                            expTarget, CSharpLanguage.GetCSharpTypeName(dprop.PropertyType), CSharpLanguage.GetCSharpTypeName(dprop.OwnerType), dpropField.Name);
                        WriteLine("{0}SetValue<{1}>({2}.{3}, __UPF_ConvertFromString(value, current));",
                            expTarget, CSharpLanguage.GetCSharpTypeName(dprop.PropertyType), CSharpLanguage.GetCSharpTypeName(dprop.OwnerType), dpropField.Name);
                        WriteLine("}");
                    }
                    else
                    {
                        if (expressionInfo.NullableFixup)
                        {
                            WriteLine(targetTypeSpecified ? "set {{ {0}SetValue<{1}>({2}.{3}, ({4})(value ?? default({1}))); }}" : "set {{ {0}SetValue<{1}>({2}.{3}, value ?? default({1})); }}",
                                expTarget,
                                CSharpLanguage.GetCSharpTypeName(dprop.PropertyType),
                                CSharpLanguage.GetCSharpTypeName(dprop.OwnerType),
                                dpropField.Name, targetTypeName);
                        }
                        else
                        {
                            WriteLine(targetTypeSpecified ? "set {{ {0}SetValue<{1}>({2}.{3}, ({4})(value)); }}" : "set {{ {0}SetValue<{1}>({2}.{3}, value); }}",
                                expTarget,
                                CSharpLanguage.GetCSharpTypeName(dprop.PropertyType),
                                CSharpLanguage.GetCSharpTypeName(dprop.OwnerType),
                                dpropField.Name, targetTypeName);
                        }
                    }
                }
                else
                {
                    if (IsStringType(expressionInfo.Type))
                    {
                        WriteLine("set");
                        WriteLine("{");
                        WriteLine("var current = {0}{1};", expTarget, expText);
                        WriteLine("{0}{1} = __UPF_ConvertFromString(value, current);", expTarget, expText);
                        WriteLine("}");                        
                    }
                    else
                    {
                        if (expressionInfo.NullableFixup)
                        {
                            WriteLine(targetTypeSpecified ? "set {{ ({0}{1}) = ({3})(value ?? default({2})); }}" : "set {{ ({0}{1}) = value ?? default({2}); }}",
                                expTarget, expText, CSharpLanguage.GetCSharpTypeName(Nullable.GetUnderlyingType(expressionInfo.Type)), targetTypeName);
                        }
                        else
                        {
                            WriteLine(targetTypeSpecified ? "set {{ ({0}{1}) = ({2})(value); }}" : "set {{ ({0}{1}) = value; }}",
                                expTarget, expText, targetTypeName);
                        }
                    }
                }

                expressionInfo.SetterLineEnd = LineCount - 1;
            }

            WriteLine("}");
            WriteLine();
            
            if (expressionInfo.GenerateGetter)
            {
                WriteLine("public static readonly DataBindingGetter<{0}> __Get__UPF_Expression{1} = new DataBindingGetter<{0}>(vm => (({2})vm).__UPF_Expression{1});",
                    CSharpLanguage.GetCSharpTypeName(expressionInfo.Type), id, dataSourceWrapperInfo.DataSourceWrapperName);
                WriteLine();
            }

            if (expressionInfo.GenerateSetter)
            {
                WriteLine("public static readonly DataBindingSetter<{0}> __Set__UPF_Expression{1} = new DataBindingSetter<{0}>((vm, value) => (({2})vm).__UPF_Expression{1} = value);",
                    CSharpLanguage.GetCSharpTypeName(expressionInfo.Type), id, dataSourceWrapperInfo.DataSourceWrapperName);
                WriteLine();
            }
        }

        /// <summary>
        /// Gets the source text for the specified parameter when it is part of a parameter list.
        /// </summary>
        public String GetParameterText(ParameterInfo parameter)
        {
            return CSharpLanguage.GetCSharpTypeName(parameter.ParameterType) + " " + parameter.Name;
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
        /// <param name="disposing"><see langword="true"/> if the object is being disposed; <see langword="false"/> if the object is being finalized.</param>
        protected virtual void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                SafeDispose.Dispose(txtWriter);
                SafeDispose.Dispose(strWriter);
            }
        }

        /// <summary>
        /// Given a targeted binding expression (in the form "foo->bar"), this method extracts the target name, target type, and expression text.
        /// </summary>
        private Boolean GetExpressionTargetInfo(IExpressionCompilerState state, DataSourceWrapperInfo dataSourceWrapperInfo,
            XObject source, ref String expText, out String expTarget, out Type expTargetType)
        {
            const string TargetExpressionDelimiter = "->";

            var expOriginal = expText;

            var delimiterIndex = expText.IndexOf(TargetExpressionDelimiter);
            if (delimiterIndex >= 0)
            {
                var expPartTarget = expText.Substring(0, delimiterIndex);
                var expPartText = expText.Substring(delimiterIndex + TargetExpressionDelimiter.Length);

                var matchCandidates = (from element in dataSourceWrapperInfo.DataSourceDefinition.Definition.Descendants()
                                       where (String)element.Attribute("Name") == expPartTarget
                                       select element).ToList();
                
                if (matchCandidates.Count == 0)
                {
                    throw new BindingExpressionCompilationErrorException(source, dataSourceWrapperInfo.DataSourceDefinition.DefinitionPath,
                        CompilerStrings.ExpressionTargetIsNotFound.Format(expPartTarget));
                }

                if (matchCandidates.Count > 1)
                {
                    throw new BindingExpressionCompilationErrorException(source, dataSourceWrapperInfo.DataSourceDefinition.DefinitionPath,
                        CompilerStrings.ExpressionTargetIsAmbiguous.Format(expPartTarget));
                }

                var match = matchCandidates.Single();
                var matchName = match.Name.LocalName;

                expText = expPartText;
                expTargetType = UvmlTypeAnalysis.GetPlaceholderType(dataSourceWrapperInfo.DataSourceType, matchName);
                
                if (expTargetType == null && !state.GetKnownType(matchName, out expTargetType))
                {
                    throw new BindingExpressionCompilationErrorException(source, dataSourceWrapperInfo.DataSourceDefinition.DefinitionPath,
                        CompilerStrings.ExpressionTargetIsUnrecognizedType.Format(expOriginal, matchName));
                }

                expTarget = String.Format("__UPF_FindName<{0}>(\"{1}\").", CSharpLanguage.GetCSharpTypeName(expTargetType), expPartTarget);

                return true;
            }

            expTarget = default(String);
            expTargetType = dataSourceWrapperInfo.DataSourceType;

            return false;
        }

        private Boolean IsStringType(Type type)
        {
            return type == typeof(String) || type == typeof(VersionedStringSource);
        }

        // State values.
        private readonly StringWriter strWriter;
        private readonly IndentedTextWriter txtWriter;
    }
}
