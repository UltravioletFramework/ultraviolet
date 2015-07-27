using System.Reflection;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Compiler
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
            using (var stream = asm.GetManifestResourceStream("TwistedLogik.Ultraviolet.UI.Presentation.Compiler.Resources.Strings.xml"))
            {
                StringDatabase.LoadFromStream(stream);
            }
        }

        private static readonly LocalizationDatabase StringDatabase = new LocalizationDatabase();

#pragma warning disable 1591
        public static readonly StringResource FailedExpressionValidationPass     = new StringResource(StringDatabase, "FAILED_EXPRESSION_VALIDATION_PASS");
        public static readonly StringResource FailedFinalPass                    = new StringResource(StringDatabase, "FAILED_FINAL_PASS");
        public static readonly StringResource ViewModelTypeIsNotFullyQualified   = new StringResource(StringDatabase, "VIEW_MODEL_TYPE_IS_NOT_FULLY_QUALIFIED");
        public static readonly StringResource OnlyDependencyPropertiesCanBeBound = new StringResource(StringDatabase, "ONLY_DEPENDENCY_PROPERTIES_CAN_BE_BOUND");
        public static readonly StringResource ElementDoesNotHaveDefaultProperty  = new StringResource(StringDatabase, "ELEMENT_DOES_NOT_HAVE_DEFAULT_PROPERTY");
#pragma warning restore 1591
    }
}
