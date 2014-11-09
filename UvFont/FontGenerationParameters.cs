using System;
using System.Linq;
using TwistedLogik.Nucleus.Data;

namespace TwistedLogik.UvFont
{
    public class FontGenerationParameters
    {
        public FontGenerationParameters(String[] args)
        {
            if (args == null || !args.Any())
                throw new InvalidCommandLineException();

            NoBold   = ArgumentExists(args, "nobold");
            NoItalic = ArgumentExists(args, "noitalic");

            FontName = args[0];
            FontSize = ReadArgument<Single?>(args, "fontsize") ?? 16f;

            SourceText = ReadArgument<String>(args, "sourcetext");
            SourceFile = ReadArgument<String>(args, "sourcefile");

            if (SourceText != null && SourceFile != null)
                throw new InvalidCommandLineException("Both a source text and a source file were specified. Pick one!");

            SubstitutionCharacter = ReadArgument<Char?>(args, "sub") ?? '?';
        }

        public Boolean NoBold { get; set; }
        public Boolean NoItalic { get; set; }
        public String FontName { get; set; }
        public Single FontSize { get; set; }
        public String SourceText { get; set; }
        public String SourceFile { get; set; }
        public Char SubstitutionCharacter { get; set; }

        private Boolean ArgumentExists(String[] args, String name)
        {
            return args.Where(x => x.StartsWith("-" + name)).Any();
        }

        private T ReadArgument<T>(String[] args, String name)
        {
            var fullname = String.Format("-{0}:", name);
            var param = args.Where(x => x.StartsWith(fullname)).FirstOrDefault();
            if (param != null)
            {
                var value = param.Substring(fullname.Length);
                return (T)ObjectResolver.FromString(value, typeof(T));
            }
            return default(T);
        }
    }
}
