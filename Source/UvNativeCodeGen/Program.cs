﻿using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            var directory = Path.GetDirectoryName(inputFile);
            var definition = XDocument.Load(inputFile);
            if (definition.Root.Name.LocalName != "NativeLibrary")
                throw new InvalidDataException("Not a valid definition file.");

            var nativeNamespace = GetAttributeString(definition.Root, "Namespace");
            var nativeClassName = GetAttributeString(definition.Root, "ClassName");

            var namesElement = GetChildElement(definition.Root, "Names");
            var nameAndroid =
                (String)namesElement.Attribute("Android") ??
                (String)namesElement.Attribute("Unix") ??
                (String)namesElement.Attribute("Default");

            // FooNativeImpl - the abstract base class for platform-specific implementations.
            WriteImplClass_Base(definition, nativeNamespace, nativeClassName, 
                Path.Combine(directory, $"{nativeClassName}Impl.cs"));

            // FooNativeImpl_Platform - the implementation of FooImpl for each platform.
            WriteImplClass_UltravioletLoader(definition, namesElement, nativeNamespace, nativeClassName, "Default",
                Path.Combine(directory, $"{nativeClassName}Impl_Default.cs"));
            WriteImplClass_PInvoke(definition, nativeNamespace, nativeClassName, "Android", nameAndroid,
                Path.Combine(directory, $"{nativeClassName}Impl_Android.cs"));
            WriteImplClass_PInvoke(definition, nativeNamespace, nativeClassName, "iOS", "__Internal",
                Path.Combine(directory, $"{nativeClassName}Impl_iOS.cs"));

            // FooNative - static class which exposes the underlying platform-specific implementation.
            WriteWrapperClass(definition, nativeNamespace, nativeClassName,
                Path.Combine(directory, $"{nativeClassName}.cs"));
        }

        private static XElement GetChildElement(XElement parent, XName name)
        {
            var element = parent.Element(name);
            if (element == null)
                throw new InvalidDataException($"Missing required {name.LocalName} element.");

            return element;
        }

        private static String GetAttributeString(XElement element, String name)
        {
            var attrval = (String)element.Attribute(name);
            if (String.IsNullOrEmpty(attrval))
                throw new InvalidDataException($"Missing required {name} attribute on {element.Name.LocalName} element.");

            return attrval;
        }

        private static String GetOptionalAttributeString(XElement element, String name)
        {
            var attrval = (String)element.Attribute(name);
            return attrval;
        }

        private static String GetArgumentOrParameterList(XElement root, Func<XElement, String> fn)
        {
            var result = String.Empty;

            var parameterElements = root.Element("Parameters")?.Elements("Parameter");
            if (parameterElements != null)
            {
                var parameterStrings = new List<String>();
                foreach (var parameterElement in parameterElements)
                {
                    var parameterString = fn(parameterElement);
                    parameterStrings.Add(parameterString);
                }

                result = String.Join(", ", parameterStrings);
            }

            return result;
        }

        private static String GetParameterList(XElement root, Boolean includeMarshalAs = false)
        {
            return GetArgumentOrParameterList(root, element =>
            {
                var sb = new StringBuilder();

                var marshalAs = includeMarshalAs ? GetOptionalAttributeString(element, "MarshalAs") : null;
                if (!String.IsNullOrEmpty(marshalAs))
                    sb.Append($"[MarshalAs(UnmanagedType.{marshalAs})] ");

                var typeModifier = GetOptionalAttributeString(element, "TypeModifier");
                if (!String.IsNullOrEmpty(typeModifier))
                    sb.Append($"{typeModifier} ");

                var type = GetAttributeString(element, "Type");
                sb.Append(type);
                sb.Append(" ");

                var name = GetAttributeString(element, "Name");
                sb.Append(name);

                return sb.ToString();
            });
        }

        private static String GetArgumentList(XElement root)
        {
            return GetArgumentOrParameterList(root, element =>
            {
                var sb = new StringBuilder();

                var typeModifier = GetOptionalAttributeString(element, "TypeModifier");
                if (!String.IsNullOrEmpty(typeModifier))
                    sb.Append($"{typeModifier} ");

                var name = GetAttributeString(element, "Name");
                sb.Append(name);

                return sb.ToString();
            });
        }

        private static void WriteImports(IndentedTextWriter twriter, XDocument definition)
        {
            var importRoot = definition.Root.Element("Imports");
            if (importRoot != null)
            {
                var imports = importRoot.Elements("Import");
                foreach (var import in imports)
                {
                    twriter.WriteLine($"using {(String)import.Attribute("Name")};");
                }
            }
            twriter.WriteLine();
        }

        private static void WriteFunctionPointers(IndentedTextWriter twriter, XElement functionPointersElement)
        {
            if (functionPointersElement == null)
                return;

            var globalCallingConvention = GetOptionalAttributeString(functionPointersElement, "CallingConvention") ?? "Cdecl";
            foreach (var functionPointerElement in functionPointersElement.Elements("FunctionPointer"))
            {
                var fnReturnType = GetAttributeString(functionPointerElement, "ReturnType");
                var fnName = GetAttributeString(functionPointerElement, "Name");
                var fnCallingConvention = GetOptionalAttributeString(functionPointerElement, "CallingConvention") ?? globalCallingConvention;
                var fnParameters = GetParameterList(functionPointerElement);

                twriter.WriteLine($"[UnmanagedFunctionPointer(CallingConvention.{fnCallingConvention})]");
                twriter.WriteLine($"public unsafe delegate {fnReturnType} {fnName}({fnParameters});");
                twriter.WriteLine();
            }
        }

        private static void WriteEachElement(XElement parentElement, String elementName, Action<XElement, Int32, Int32> fn)
        {
            if (parentElement == null)
                return;

            var functions = parentElement.Elements(elementName)?.ToList();
            if (functions != null)
            {
                for (int i = 0; i < functions.Count; i++)
                    fn(functions[i], i, functions.Count);
            }
        }

        private static void WriteImplClass_Base(XDocument definition, String nativeNamespace, String nativeClassName, String path)
        {
            using (var stream = File.Create(path))
            using (var swriter = new StreamWriter(stream))
            using (var twriter = new IndentedTextWriter(swriter))
            {
                twriter.WriteLine($"using System;");
                twriter.WriteLine($"using System.Security;");
                twriter.WriteLine($"using System.Runtime.InteropServices;");
                WriteImports(twriter, definition);

                twriter.WriteLine($"namespace {nativeNamespace}");
                twriter.WriteLine($"{{");
                twriter.WriteLine($"#pragma warning disable 1591");
                twriter.Indent++;

                var functionPointers = definition.Root.Element("FunctionPointers");
                WriteFunctionPointers(twriter, functionPointers);

                twriter.WriteLine($"[SuppressUnmanagedCodeSecurity]");
                twriter.WriteLine($"public abstract unsafe class {nativeClassName}Impl");
                twriter.WriteLine($"{{");
                twriter.Indent++;

                var functionsElement = definition.Root.Element("Functions");
                WriteEachElement(functionsElement, "Function", (fn, i, total) =>
                {
                    var fnName = GetAttributeString(fn, "Name");
                    var fnAlias = GetOptionalAttributeString(fn, "Alias") ?? fnName;
                    var fnReturnType = GetAttributeString(fn, "ReturnType");
                    var fnReturnAsString = GetOptionalAttributeString(fn, "MarshalReturnAsString");
                    if (!String.IsNullOrEmpty(fnReturnAsString))
                        fnReturnType = "String";

                    var fnParameters = GetParameterList(fn);
                    twriter.WriteLine($"public abstract {fnReturnType} {fnAlias}({fnParameters});");
                });

                twriter.Indent--;
                twriter.WriteLine($"}}");

                twriter.Indent--;
                twriter.WriteLine($"#pragma warning restore 1591");
                twriter.WriteLine($"}}");
            }
        }

        private static void WriteImplClass_PInvoke(XDocument definition, String nativeNamespace, String nativeClassName, String suffix, String library, String path)
        {
            using (var stream = File.Create(path))
            using (var swriter = new StreamWriter(stream))
            using (var twriter = new IndentedTextWriter(swriter))
            {
                twriter.WriteLine("using System;");
                twriter.WriteLine("using System.Security;");
                twriter.WriteLine("using System.Runtime.CompilerServices;");
                twriter.WriteLine("using System.Runtime.InteropServices;");
                WriteImports(twriter, definition);

                twriter.WriteLine($"namespace {nativeNamespace}");
                twriter.WriteLine($"{{");
                twriter.WriteLine($"#pragma warning disable 1591");
                twriter.Indent++;

                twriter.WriteLine($"[SuppressUnmanagedCodeSecurity]");
                twriter.WriteLine($"internal sealed unsafe class {nativeClassName}Impl_{suffix} : {nativeClassName}Impl");
                twriter.WriteLine($"{{");
                twriter.Indent++;

                var functionsElement = definition.Root.Element("Functions");
                if (functionsElement != null)
                {
                    var globalCallingConvention = GetAttributeString(functionsElement, "CallingConvention") ?? "Cdecl";
                    WriteEachElement(functionsElement, "Function", (fn, i, total) =>
                    {
                        var fnCallingConvention = (String)fn.Attribute("CallingConvention") ?? globalCallingConvention;
                        var fnName = GetAttributeString(fn, "Name");
                        var fnAlias = GetOptionalAttributeString(fn, "Alias") ?? fnName;
                        var fnReturnType = GetAttributeString(fn, "ReturnType");
                        var fnReturnAsString = GetOptionalAttributeString(fn, "MarshalReturnAsString");

                        var fnParametersMarshalled = GetParameterList(fn, true);
                        twriter.WriteLine($"[DllImport(\"{library}\", EntryPoint = \"{fnName}\", CallingConvention = CallingConvention.{fnCallingConvention})]");
                        twriter.WriteLine($"private static extern {fnReturnType} INTERNAL_{fnAlias}({fnParametersMarshalled});");

                        var fnVisibility = String.IsNullOrEmpty(fnReturnAsString) ? "public " : "private ";
                        var fnOverride = String.IsNullOrEmpty(fnReturnAsString) ? "override sealed " : String.Empty;
                        var fnSuffix = String.IsNullOrEmpty(fnReturnAsString) ? String.Empty : "_Raw";

                        var fnArguments = GetArgumentList(fn);
                        var fnParameters = GetParameterList(fn);
                        twriter.WriteLine($"[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                        twriter.WriteLine($"{fnVisibility}{fnOverride}{fnReturnType} {fnAlias}{fnSuffix}({fnParameters}) => INTERNAL_{fnAlias}({fnArguments});");

                        if (!String.IsNullOrEmpty(fnReturnAsString))
                        {
                            twriter.WriteLine($"[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                            twriter.WriteLine($"public override sealed String {fnAlias}({fnParameters}) => Marshal.PtrToString{fnReturnAsString}({fnAlias}{fnSuffix}({fnArguments}));");
                        }

                        if (i + 1 < total)
                            twriter.WriteLine();
                    });
                }

                twriter.Indent--;
                twriter.WriteLine($"}}");

                twriter.Indent--;
                twriter.WriteLine($"#pragma warning restore 1591");
                twriter.WriteLine($"}}");
            }
        }

        private static void WriteImplClass_UltravioletLoader(XDocument definition, XElement namesElement, String nativeNamespace, String nativeClassName, String suffix, String path)
        {
            using (var stream = File.Create(path))
            using (var swriter = new StreamWriter(stream))
            using (var twriter = new IndentedTextWriter(swriter))
            {
                twriter.WriteLine("using System;");
                twriter.WriteLine("using System.Security;");
                twriter.WriteLine("using System.Runtime.CompilerServices;");
                twriter.WriteLine("using System.Runtime.InteropServices;");
                twriter.WriteLine("using Ultraviolet.Core;");
                twriter.WriteLine("using Ultraviolet.Core.Native;");
                WriteImports(twriter, definition);

                twriter.WriteLine($"namespace {nativeNamespace}");
                twriter.WriteLine($"{{");
                twriter.WriteLine($"#pragma warning disable 1591");
                twriter.Indent++;

                twriter.WriteLine($"[SuppressUnmanagedCodeSecurity]");
                twriter.WriteLine($"internal sealed unsafe class {nativeClassName}Impl_{suffix} : {nativeClassName}Impl");
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

                void WritePlatformSelectionCase(String platform, String name)
                {
                    if (String.IsNullOrEmpty(name))
                        return;

                    if (!String.IsNullOrEmpty(name))
                    {
                        twriter.WriteLine($"case UltravioletPlatform.{platform}:");
                        twriter.Indent++;
                        twriter.WriteLine($"lib = new NativeLibrary(\"{name}\");");
                        twriter.WriteLine($"break;");
                        twriter.Indent--;
                    }
                }

                var nameWindows = (String)namesElement.Attribute("Windows");
                WritePlatformSelectionCase("Windows", nameWindows);
                
                var nameUnix = (String)namesElement.Attribute("Unix");
                var nameLinux = (String)namesElement.Attribute("Linux") ?? nameUnix;
                var nameMacOS = (String)namesElement.Attribute("macOS") ?? nameUnix;
                WritePlatformSelectionCase("Linux", nameLinux ?? nameUnix);
                WritePlatformSelectionCase("macOS", nameMacOS ?? nameUnix);

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

                var functionsElement = definition.Root.Element("Functions");
                if (functionsElement != null)
                {
                    var globalCallingConvention = GetAttributeString(functionsElement, "CallingConvention") ?? "Cdecl";
                    WriteEachElement(functionsElement, "Function", (fn, i, total) =>
                    {
                        var fnName = GetAttributeString(fn, "Name");
                        var fnAlias = GetOptionalAttributeString(fn, "Alias") ?? fnName;
                        var fnReturnType = GetAttributeString(fn, "ReturnType");
                        var fnReturnAsString = GetOptionalAttributeString(fn, "MarshalReturnAsString");
                        var fnCallingConvention = GetOptionalAttributeString(fn, "CallingConvention") ?? globalCallingConvention;

                        var fnArguments = GetArgumentList(fn);
                        var fnParameters = GetParameterList(fn);

                        var fnVisibility = String.IsNullOrEmpty(fnReturnAsString) ? "public " : "private ";
                        var fnOverride = String.IsNullOrEmpty(fnReturnAsString) ? "override sealed " : String.Empty;
                        var fnSuffix = String.IsNullOrEmpty(fnReturnAsString) ? String.Empty : "_Raw";

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

                        if (i + 1 < total)
                            twriter.WriteLine();
                    });
                }

                twriter.Indent--;
                twriter.WriteLine($"}}");

                twriter.Indent--;
                twriter.WriteLine($"#pragma warning restore 1591");
                twriter.WriteLine($"}}");
            }
        }

        private static void WriteWrapperClass(XDocument definition, String nativeNamespace, String nativeClassName, String path)
        {
            using (var stream = File.Create(path))
            using (var swriter = new StreamWriter(stream))
            using (var twriter = new IndentedTextWriter(swriter))
            {
                twriter.WriteLine("using System;");
                twriter.WriteLine("using System.Runtime.CompilerServices;");
                twriter.WriteLine("using Ultraviolet.Core;");
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

                var constants = definition.Root.Element("Constants");
                if (constants != null && constants.Elements().Any())
                {
                    WriteEachElement(constants, "Constant", (constant, i, total) =>
                    {
                        var constantType = (String)constant.Attribute("Type");
                        var constantName = (String)constant.Attribute("Name");
                        var constantValue = (String)constant.Attribute("Value");
                        twriter.WriteLine($"public const {constantType} {constantName} = {constantValue};");
                    });
                    twriter.WriteLine();
                }

                var functions = definition.Root.Element("Functions");
                WriteEachElement(functions, "Function", (fn, i, total) =>
                {
                    var fnName = GetAttributeString(fn, "Name");
                    var fnAlias = GetOptionalAttributeString(fn, "Alias") ?? fnName;
                    var fnReturnType = GetAttributeString(fn, "ReturnType");
                    var fnReturnAsString = GetOptionalAttributeString(fn, "MarshalReturnAsString");
                    if (!String.IsNullOrEmpty(fnReturnAsString))
                        fnReturnType = "String";

                    var fnParameters = GetParameterList(fn);
                    var fnArguments = GetArgumentList(fn);

                    twriter.WriteLine($"[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                    twriter.WriteLine($"public static {fnReturnType} {fnAlias}({fnParameters}) => impl.{fnAlias}({fnArguments});");

                    if (i + 1 < total)
                        twriter.WriteLine();
                });

                twriter.Indent--;
                twriter.WriteLine($"}}");

                twriter.Indent--;
                twriter.WriteLine($"#pragma warning restore 1591");
                twriter.WriteLine($"}}");
            }
        }
    }
}
