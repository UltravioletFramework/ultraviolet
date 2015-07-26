using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Reflection;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Compiler
{
    /// <summary>
    /// Contains methods for writing the source code for a view model wrapper.
    /// </summary>
    internal class ViewModelWrapperWriter : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelWrapperWriter"/> class.
        /// </summary>
        public ViewModelWrapperWriter()
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

            txtWriter.WriteLine(text);
            LineCount += 1 + text.Count(c => c == '\n');

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
        /// Writes a constructor for the specified view model.
        /// </summary>
        /// <param name="viewModelInfo">A <see cref="ViewModelWrapperInfo"/> describing the view model for which to write a constructor.</param>
        public void WriteConstructor(ViewModelWrapperInfo viewModelInfo)
        {
            WriteLine("public {0}({1} viewModel)", viewModelInfo.ViewModelWrapperName, GetCSharpTypeName(viewModelInfo.ViewModelType));
            WriteLine("{");

            WriteLine("this.viewModel = viewModel;");

            WriteLine("}");
        }

        /// <summary>
        /// Writes an implementation of the <see cref="IViewModelWrapper"/> interface.
        /// </summary>
        /// <param name="viewModelInfo">A <see cref="ViewModelWrapperInfo"/> describing the view model for which to write an <see cref="IViewModelWrapper"/> implementation.</param>
        public void WriteIViewModelWrapperImplementation(ViewModelWrapperInfo viewModelInfo)
        {
            WriteLine("Object IViewModelWrapper.ViewModel");
            WriteLine("{");

            WriteLine("get { return viewModel; }");

            WriteLine("}");
            WriteLine("private readonly {0} viewModel;", GetCSharpTypeName(viewModelInfo.ViewModelType));
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

            WriteLine("public {0}{1} {2}({3})", methodStaticQualifier, GetCSharpTypeName(method.ReturnType), method.Name, methodParameterList);
            WriteLine("{");

            var target = isStatic ? GetCSharpTypeName(method.DeclaringType) : "viewModel";
            WriteLine("{0}.{1}({2});", target, method.Name, methodArgumentList);

            WriteLine("}");
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

            var target = isStatic ? GetCSharpTypeName(property.DeclaringType) : "viewModel";

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

            var target = isStatic ? GetCSharpTypeName(field.DeclaringType) : "viewModel";

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
        /// <param name="info">The binding expression for which to write a property.</param>
        /// <param name="id">The expression's identifier within the view model.</param>
        public void WriteExpressionProperty(BindingExpressionInfo info, Int32 id)
        {
            WriteLine("// {0}", info.Expression);
            WriteLine("public {0} __UPF_Expression{1}", GetCSharpTypeName(info.Type), id);
            WriteLine("{");

            var expMemberPath = BindingExpressions.GetBindingMemberPathPart(info.Expression);
            var expFormatString = BindingExpressions.GetBindingFormatStringPart(info.Expression);

            if (info.GenerateGetter)
            {
                info.GetterLineStart = LineCount;

                if (info.Type == typeof(String))
                {
                    if (String.IsNullOrEmpty(expFormatString))
                    {
                        expFormatString = "{0}";
                    }
                    WriteLine("get {{ return String.Format(\"{0}\", {1}); }}", expFormatString, expMemberPath);
                }
                else
                {
                    WriteLine("get {{ return {0}; }}", expMemberPath);
                }

                info.GetterLineEnd = LineCount - 1;
            }

            if (info.GenerateSetter)
            {
                info.SetterLineStart = LineCount;

                WriteLine("set {{ {0} = value; }}", expMemberPath);

                info.SetterLineEnd = LineCount - 1;
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

            if (type.IsByRef)
            {
                return "ref " + type.GetElementType().FullName;
            }

            return type.FullName;
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
