using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using CommandLine;

namespace UvNativeCodeGen
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    GenerateNativeLibraryFromDefinition(o.InputFile);
                });
        }

        private static void GenerateNativeLibraryFromDefinition(String inputFile)
        {
            var definition = XDocument.Load(inputFile);
            if (definition.Root.Name.LocalName != "NativeLibrary")
                throw new InvalidDataException("Not a valid definition file.");

            var nativeNamespace = (String) definition.Root.Attribute("Namespace");
            if (String.IsNullOrEmpty(nativeNamespace))
                throw new InvalidDataException("Invalid namespace.");

            var nativeClassName = (String)definition.Root.Attribute("ClassName");
            if (String.IsNullOrEmpty(nativeClassName))
                throw new InvalidDataException("Invalid class name.");

            var namesElement = definition.Root.Element("Names");
            if (namesElement == null)
                throw new InvalidDataException("Missing Names element.");

            var directory = Path.GetDirectoryName(inputFile);
            WriteNativeImplClass(definition, nativeNamespace, nativeClassName, 
                Path.Combine(directory, $"{nativeClassName}Impl.cs"));
            WriteUltravioletLoaderImpl(definition, namesElement, nativeNamespace, nativeClassName, "Default",
                Path.Combine(directory, $"{nativeClassName}Impl_Default.cs"));
            WritePInvokeImpl(definition, nativeNamespace, nativeClassName, "Android", (String)namesElement.Attribute("Android") ?? (String)namesElement.Attribute("Unix") ?? (String)namesElement.Attribute("Default"),
                Path.Combine(directory, $"{nativeClassName}Impl_Android.cs"));
            WritePInvokeImpl(definition, nativeNamespace, nativeClassName, "iOS", "__Internal",
                Path.Combine(directory, $"{nativeClassName}Impl_iOS.cs"));
            WriteNativeWrapperClass(definition, nativeNamespace, nativeClassName,
                Path.Combine(directory, $"{nativeClassName}.cs"));
        }

        private static String GetParameterList(XElement root, Boolean includeMarshalAs = false)
        {
            var parameterElements = root.Element("Parameters")?.Elements("Parameter");
            var parameterElementsList = String.Empty;
            if (parameterElements != null)
            {
                var parameterStrings = new List<String>();
                foreach (var parameterElement in parameterElements)
                {
                    var marshalAs = includeMarshalAs ? (String)parameterElement.Attribute("MarshalAs") : null;
                    var marshalAsAttr = String.IsNullOrEmpty(marshalAs) ? String.Empty : $"[MarshalAs(UnmanagedType.{marshalAs})] ";

                    var type = (String)parameterElement.Attribute("Type");
                    var typeModifier = (String)parameterElement.Attribute("TypeModifier");
                    if (!String.IsNullOrEmpty(typeModifier))
                        type = $"{typeModifier} {type}";

                    var parameterName = (String)parameterElement.Attribute("Name");
                    var parameterString = $"{marshalAsAttr}{type} {parameterName}";
                    parameterStrings.Add(parameterString);
                }

                parameterElementsList = String.Join(", ", parameterStrings);
            }
            return parameterElementsList;
        }

        private static String GetArgumentList(XElement root)
        {
            var parameterElements = root.Element("Parameters")?.Elements("Parameter");
            var parameterElementsList = String.Empty;
            if (parameterElements != null)
            {
                var parameterStrings = new List<String>();
                foreach (var parameterElement in parameterElements)
                {
                    var typeModifier = (String)parameterElement.Attribute("TypeModifier");
                    if (!String.IsNullOrEmpty(typeModifier))
                        typeModifier = $"{typeModifier} ";

                    var parameterName = (String)parameterElement.Attribute("Name");
                    var parameterString = $"{typeModifier}{parameterName}";
                    parameterStrings.Add(parameterString);
                }

                parameterElementsList = String.Join(", ", parameterStrings);
            }
            return parameterElementsList;
        }

        private static void WriteImports(IndentedTextWriter twriter, XDocument definition)
        {
            twriter.WriteLine("using System;");
            twriter.WriteLine("using System.Security;");
            twriter.WriteLine("using System.Runtime.CompilerServices;");
            twriter.WriteLine("using System.Runtime.InteropServices;");
            twriter.WriteLine("using Ultraviolet.Core;");
            twriter.WriteLine("using Ultraviolet.Core.Native;");

            var importRoot = definition.Root.Element("Imports");
            if (importRoot != null)
            {
                var imports = importRoot.Elements("Import");
                foreach (var import in imports)
                {
                    twriter.WriteLine($"using {(String)import.Attribute("Name")};");
                }
                twriter.WriteLine();
            }
        }

        private static void WriteNativeImplClass(XDocument definition, String nativeNamespace, String nativeClassName, String path)
        {
            using (var stream = File.Create(path))
            using (var swriter = new StreamWriter(stream))
            using (var twriter = new IndentedTextWriter(swriter))
            {
                WriteImports(twriter, definition);

                twriter.WriteLine($"namespace {nativeNamespace}");
                twriter.WriteLine($"{{");
                twriter.WriteLine($"#pragma warning disable 1591");
                twriter.Indent++;

                var functionPointers = definition.Root.Element("FunctionPointers");
                if (functionPointers != null)
                    WriteFunctionPointers(twriter, functionPointers);

                twriter.WriteLine($"[SuppressUnmanagedCodeSecurity]");
                twriter.WriteLine($"public abstract unsafe class {nativeClassName}Impl");
                twriter.WriteLine($"{{");
                twriter.Indent++;

                var functions = definition.Root.Element("Functions")?.Elements("Function")?.ToList();
                if (functions != null)
                {
                    for (int i = 0; i < functions.Count; i++)
                    {
                        var fn = functions[i];
                        var fnReturnAsString = (String)fn.Attribute("MarshalReturnAsString");
                        var fnReturnType = String.IsNullOrEmpty(fnReturnAsString) ? (String)fn.Attribute("ReturnType") : "String";
                        var fnAlias = (String)fn.Attribute("Alias") ?? (String)fn.Attribute("Name");
                        var fnParameters = GetParameterList(fn);
                        twriter.WriteLine($"public abstract {fnReturnType} {fnAlias}({fnParameters});");
                    }
                }

                twriter.Indent--;
                twriter.WriteLine($"}}");

                twriter.Indent--;
                twriter.WriteLine($"#pragma warning restore 1591");
                twriter.WriteLine($"}}");
            }
        }

        private static void WriteNativeWrapperClass(XDocument definition, String nativeNamespace, String nativeClassName, String path)
        {
            using (var stream = File.Create(path))
            using (var swriter = new StreamWriter(stream))
            using (var twriter = new IndentedTextWriter(swriter))
            {
                WriteImports(twriter, definition);

                twriter.WriteLine($"namespace {nativeNamespace}");
                twriter.WriteLine($"{{");
                twriter.WriteLine($"#pragma warning disable 1591");
                twriter.Indent++;

                twriter.WriteLine($"public static unsafe partial class {nativeClassName}");
                twriter.WriteLine($"{{");
                twriter.Indent++;

                twriter.WriteLine($"private static readonly {nativeClassName}Impl impl;");
                twriter.WriteLine();

                twriter.WriteLine($"static {nativeClassName}()");
                twriter.WriteLine($"{{");
                twriter.Indent++;

                twriter.WriteLine($"switch (UltravioletPlatformInfo.CurrentPlatform)");
                twriter.WriteLine($"{{");
                twriter.Indent++;

                twriter.WriteLine($"case UltravioletPlatform.Android:");
                twriter.Indent++;
                twriter.WriteLine($"impl = new {nativeClassName}Impl_Android();");
                twriter.WriteLine($"break;");
                twriter.WriteLine();
                twriter.Indent--;

                twriter.WriteLine($"case UltravioletPlatform.iOS:");
                twriter.Indent++;
                twriter.WriteLine($"impl = new {nativeClassName}Impl_iOS();");
                twriter.WriteLine($"break;");
                twriter.WriteLine();
                twriter.Indent--;

                twriter.WriteLine($"default:");
                twriter.Indent++;
                twriter.WriteLine($"impl = new {nativeClassName}Impl_Default();");
                twriter.WriteLine($"break;");
                twriter.Indent--;

                twriter.Indent--;
                twriter.WriteLine($"}}");

                twriter.Indent--;
                twriter.WriteLine($"}}");
                twriter.WriteLine();

                var constants = definition.Root.Element("Constants")?.Elements("Constant").ToList();
                if (constants != null)
                {
                    for (int i = 0; i < constants.Count; i++)
                    {
                        var constant = constants[i];
                        var constantType = (String)constant.Attribute("Type");
                        var constantName = (String)constant.Attribute("Name");
                        var constantValue = (String)constant.Attribute("Value");
                        twriter.WriteLine($"public const {constantType} {constantName} = {constantValue};");
                    }
                    twriter.WriteLine();
                }

                var functions = definition.Root.Element("Functions")?.Elements("Function")?.ToList();
                if (functions != null)
                {
                    for (int i = 0; i < functions.Count; i++)
                    {
                        var fn = functions[i];
                        var fnReturnAsString = (String)fn.Attribute("MarshalReturnAsString");
                        var fnReturnType = String.IsNullOrEmpty(fnReturnAsString) ? (String)fn.Attribute("ReturnType") : "String";
                        var fnName = (String)fn.Attribute("Alias") ?? (String)fn.Attribute("Name");
                        var fnParameters = GetParameterList(fn);
                        var fnArguments = GetArgumentList(fn);

                        twriter.WriteLine($"[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                        twriter.WriteLine($"public static {fnReturnType} {fnName}({fnParameters}) => impl.{fnName}({fnArguments});");

                        if (i + 1 < functions.Count)
                            twriter.WriteLine();
                    }
                }

                twriter.Indent--;
                twriter.WriteLine($"}}");

                twriter.Indent--;
                twriter.WriteLine($"#pragma warning restore 1591");
                twriter.WriteLine($"}}");
            }
        }

        private static void WritePInvokeImpl(XDocument definition, String nativeNamespace, String nativeClassName, String suffix, String library, String path)
        {
            using (var stream = File.Create(path))
            using (var swriter = new StreamWriter(stream))
            using (var twriter = new IndentedTextWriter(swriter))
            {
                WriteImports(twriter, definition);

                twriter.WriteLine($"namespace {nativeNamespace}");
                twriter.WriteLine($"{{");
                twriter.WriteLine($"#pragma warning disable 1591");
                twriter.Indent++;

                twriter.WriteLine($"[SuppressUnmanagedCodeSecurity]");
                twriter.WriteLine($"public sealed unsafe class {nativeClassName}Impl_{suffix} : {nativeClassName}Impl");
                twriter.WriteLine($"{{");
                twriter.Indent++;

                var functions = definition.Root.Element("Functions")?.Elements("Function")?.ToList();
                if (functions != null)
                {
                    var globalCallingConvention = (String)definition.Root.Element("Functions").Attribute("CallingConvention");
                    for (int i = 0; i < functions.Count; i++)
                    {
                        var fn = functions[i];
                        var fnName = (String)fn.Attribute("Name");
                        var fnAlias = (String)fn.Attribute("Alias") ?? fnName;
                        var fnReturnType = (String)fn.Attribute("ReturnType");
                        var fnReturnAsString = (String)fn.Attribute("MarshalReturnAsString");
                        var fnVisibility = String.IsNullOrEmpty(fnReturnAsString) ? "public " : "private ";
                        var fnOverride = String.IsNullOrEmpty(fnReturnAsString) ? "override sealed " : String.Empty;
                        var fnSuffix = String.IsNullOrEmpty(fnReturnAsString) ? String.Empty : "_Raw";
                        var fnCallingConvention = (String)fn.Attribute("CallingConvention") ?? globalCallingConvention;
                        var fnArguments = GetArgumentList(fn);
                        var fnParameters = GetParameterList(fn);
                        var fnParametersMarshalled = GetParameterList(fn, true);

                        twriter.WriteLine($"[DllImport(\"{library}\", EntryPoint = \"{fnName}\", CallingConvention = CallingConvention.{fnCallingConvention})]");
                        twriter.WriteLine($"private static extern {fnReturnType} INTERNAL_{fnAlias}({fnParametersMarshalled});");
                        twriter.WriteLine($"[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                        twriter.WriteLine($"{fnVisibility}{fnOverride}{fnReturnType} {fnAlias}{fnSuffix}({fnParameters}) => INTERNAL_{fnAlias}({fnArguments});");

                        if (!String.IsNullOrEmpty(fnReturnAsString))
                        {
                            twriter.WriteLine($"[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                            twriter.WriteLine($"public override sealed String {fnAlias}({fnParameters}) => Marshal.PtrToString{fnReturnAsString}({fnAlias}{fnSuffix}({fnArguments}));");
                        }

                        if (i + 1 < functions.Count)
                            twriter.WriteLine();
                    }
                }

                twriter.Indent--;
                twriter.WriteLine($"}}");

                twriter.Indent--;
                twriter.WriteLine($"#pragma warning restore 1591");
                twriter.WriteLine($"}}");
            }
        }

        private static void WriteUltravioletLoaderImpl(XDocument definition, XElement namesElement, String nativeNamespace, String nativeClassName, String suffix, String path)
        {
            using (var stream = File.Create(path))
            using (var swriter = new StreamWriter(stream))
            using (var twriter = new IndentedTextWriter(swriter))
            {
                WriteImports(twriter, definition);

                twriter.WriteLine($"namespace {nativeNamespace}");
                twriter.WriteLine($"{{");
                twriter.WriteLine($"#pragma warning disable 1591");
                twriter.Indent++;

                twriter.WriteLine($"[SuppressUnmanagedCodeSecurity]");
                twriter.WriteLine($"public sealed unsafe class {nativeClassName}Impl_{suffix} : {nativeClassName}Impl");
                twriter.WriteLine($"{{");
                twriter.Indent++;

                twriter.WriteLine($"private static readonly NativeLibrary lib;");
                twriter.WriteLine();

                twriter.WriteLine($"static {nativeClassName}Impl_{suffix}()");
                twriter.WriteLine($"{{");
                twriter.Indent++;

                twriter.WriteLine($"switch (UltravioletPlatformInfo.CurrentPlatform)");
                twriter.WriteLine($"{{");
                twriter.Indent++;

                var nameWindows = (String)namesElement.Attribute("Windows");
                if (!String.IsNullOrEmpty(nameWindows))
                {
                    twriter.WriteLine($"case UltravioletPlatform.Windows:");
                    twriter.Indent++;
                    twriter.WriteLine($"lib = new NativeLibrary(\"{nameWindows}\");");
                    twriter.WriteLine($"break;");
                    twriter.Indent--;
                }

                var nameUnix = (String)namesElement.Attribute("Unix");
                var nameLinux = (String)namesElement.Attribute("Linux") ?? nameUnix;
                if (!String.IsNullOrEmpty(nameLinux))
                {
                    twriter.WriteLine($"case UltravioletPlatform.Linux:");
                    twriter.Indent++;
                    twriter.WriteLine($"lib = new NativeLibrary(\"{nameLinux}\");");
                    twriter.WriteLine($"break;");
                    twriter.Indent--;
                }

                var namemacOS = (String)namesElement.Attribute("macOS") ?? nameUnix;
                if (!String.IsNullOrEmpty(nameLinux))
                {
                    twriter.WriteLine($"case UltravioletPlatform.macOS:");
                    twriter.Indent++;
                    twriter.WriteLine($"lib = new NativeLibrary(\"{namemacOS}\");");
                    twriter.WriteLine($"break;");
                    twriter.Indent--;
                }

                twriter.WriteLine($"default:");
                twriter.Indent++;
                twriter.WriteLine($"lib = new NativeLibrary(\"{(String)namesElement.Attribute("Default")}\");");
                twriter.WriteLine($"break;");
                twriter.Indent--;

                twriter.Indent--;
                twriter.WriteLine($"}}");

                twriter.Indent--;
                twriter.WriteLine($"}}");
                twriter.WriteLine();

                var functions = definition.Root.Element("Functions")?.Elements("Function")?.ToList();
                if (functions != null)
                {
                    var globalCallingConvention = (String)definition.Root.Element("Functions").Attribute("CallingConvention");
                    for (int i = 0; i < functions.Count; i++)
                    {
                        var fn = functions[i];
                        var fnName = (String)fn.Attribute("Name");
                        var fnAlias = (String)fn.Attribute("Alias") ?? fnName;
                        var fnReturnType = (String)fn.Attribute("ReturnType");
                        var fnReturnAsString = (String)fn.Attribute("MarshalReturnAsString");
                        var fnVisibility = String.IsNullOrEmpty(fnReturnAsString) ? "public " : "private ";
                        var fnOverride = String.IsNullOrEmpty(fnReturnAsString) ? "override sealed " : String.Empty;
                        var fnSuffix = String.IsNullOrEmpty(fnReturnAsString) ? String.Empty : "_Raw";
                        var fnCallingConvention = (String)fn.Attribute("CallingConvention") ?? globalCallingConvention;
                        var fnArguments = GetArgumentList(fn);
                        var fnParameters = GetParameterList(fn);
                        var fnParametersMarshalled = GetParameterList(fn, true);

                        twriter.WriteLine($"[MonoNativeFunctionWrapper]");
                        twriter.WriteLine($"[UnmanagedFunctionPointer(CallingConvention.{fnCallingConvention})]");
                        twriter.WriteLine($"private delegate {fnReturnType} {fnName}{fnSuffix}Delegate({fnParameters});");
                        twriter.WriteLine($"private readonly {fnName}{fnSuffix}Delegate p{fnName}{fnSuffix} = lib.LoadFunction<{fnName}{fnSuffix}Delegate>(\"{fnName}\");");

                        if (String.IsNullOrEmpty(fnReturnAsString))
                        {
                            twriter.WriteLine($"[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                            twriter.WriteLine($"{fnVisibility}{fnOverride}{fnReturnType} {fnAlias}{fnSuffix}({fnParameters}) => p{fnName}{fnSuffix}({fnArguments});");
                        }
                        else
                        {
                            twriter.WriteLine($"[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                            twriter.WriteLine($"public override sealed String {fnAlias}({fnParameters}) => Marshal.PtrToString{fnReturnAsString}(p{fnName}{fnSuffix}({fnArguments}));");
                        }

                        if (i + 1 < functions.Count)
                            twriter.WriteLine();
                    }
                }


                twriter.Indent--;
                twriter.WriteLine($"}}");

                twriter.Indent--;
                twriter.WriteLine($"#pragma warning restore 1591");
                twriter.WriteLine($"}}");
            }
        }

        private static void WriteFunctionPointers(IndentedTextWriter twriter, XElement functionPointers)
        {
            var globalCallingConvention = (String)functionPointers.Attribute("CallingConvention");
            foreach (var fnElement in functionPointers.Elements("FunctionPointer"))
            {
                var fnReturnType = (String)fnElement.Attribute("ReturnType");
                var fnName = (String)fnElement.Attribute("Name");
                var fnCallingConvention = (String)fnElement.Attribute("CallingConvention") ?? globalCallingConvention;
                var fnParameters = GetParameterList(fnElement);

                twriter.WriteLine($"[UnmanagedFunctionPointer(CallingConvention.{fnCallingConvention})]");
                twriter.WriteLine($"public unsafe delegate {fnReturnType} {fnName}({fnParameters});");
                twriter.WriteLine();
            }
        }
    }
}
