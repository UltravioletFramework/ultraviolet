using System.Reflection;
using Ultraviolet.Core.Text;

namespace Ultraviolet.Presentation.Compiler
{
    /// <summary>
    /// Contains the Ultraviolet Presentation Foundation expression compiler's string resources.
    /// </summary>
    public static class CompilerStrings
    {
        /// <summary>
        /// Initializes the <see cref="CompilerStrings"/> type.
        /// </summary>
        static CompilerStrings()
        {
            var asm = Assembly.GetExecutingAssembly();
            using (var stream = asm.GetManifestResourceStream("Ultraviolet.Presentation.Compiler.Resources.Strings.xml"))
            {
                StringDatabase.LoadFromStream(stream);
            }
        }

        private static readonly LocalizationDatabase StringDatabase = new LocalizationDatabase();

#pragma warning disable 1591
        public static readonly StringResource FailedExpressionValidationPass         = new StringResource(StringDatabase, "FAILED_EXPRESSION_VALIDATION_PASS");
        public static readonly StringResource FailedFinalPass                        = new StringResource(StringDatabase, "FAILED_FINAL_PASS");
        public static readonly StringResource FailedEmit                             = new StringResource(StringDatabase, "FAILED_EMIT");
        public static readonly StringResource ViewModelTypeIsNotFullyQualified       = new StringResource(StringDatabase, "VIEW_MODEL_TYPE_IS_NOT_FULLY_QUALIFIED");
        public static readonly StringResource OnlyDependencyPropertiesCanBeBound     = new StringResource(StringDatabase, "ONLY_DEPENDENCY_PROPERTIES_CAN_BE_BOUND");
        public static readonly StringResource ElementDoesNotHaveDefaultProperty      = new StringResource(StringDatabase, "ELEMENT_DOES_NOT_HAVE_DEFAULT_PROPERTY");
        public static readonly StringResource ViewDirectiveMustHaveType              = new StringResource(StringDatabase, "VIEW_DIRECTIVE_MUST_HAVE_TYPE");
        public static readonly StringResource ViewDirectiveNotRecognized             = new StringResource(StringDatabase, "VIEW_DIRECTIVE_NOT_RECOGNIZED");
        public static readonly StringResource ViewDirectiveHasInvalidValue           = new StringResource(StringDatabase, "VIEW_DIRECTIVE_HAS_INVALID_VALUE");
        public static readonly StringResource ExpressionTargetIsNotFound             = new StringResource(StringDatabase, "EXPRESSION_TARGET_IS_NOT_FOUND");
        public static readonly StringResource ExpressionTargetIsAmbiguous            = new StringResource(StringDatabase, "EXPRESSION_TARGET_IS_AMBIGUOUS");
        public static readonly StringResource ExpressionTargetIsUnrecognizedType     = new StringResource(StringDatabase, "EXPRESSION_TARGET_IS_UNRECOGNIZED_TYPE");
        public static readonly StringResource CouldNotLocateReferenceAssemblies      = new StringResource(StringDatabase, "COULD_NOT_LOCATE_REFERENCE_ASSEMBLIES");
        public static readonly StringResource CouldNotLocateReferenceAssembliesCore3 = new StringResource(StringDatabase, "COULD_NOT_LOCATE_REFERENCE_ASSEMBLIES_CORE3");
#pragma warning restore 1591
    }
}
