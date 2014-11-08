using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwistedLogik.UvFont
{
    public class FontGenerationParameters
    {
        public FontGenerationParameters(String[] args)
        {
            if (args == null || !args.Any())
                throw new InvalidCommandLineException();

            FontName = args[0];

            var fontSizeParam = args.Where(x => x.StartsWith("-fontsize:")).FirstOrDefault();
            if (fontSizeParam != null)
            {
                Single fontSize;
                if (!Single.TryParse(fontSizeParam.Substring("-fontsize:".Length), out fontSize))
                {
                    throw new InvalidCommandLineException();
                }
                FontSize = fontSize;
            }
            else
            {
                FontSize = 16f;
            }
        }

        public String FontName { get; set; }
        public Single FontSize { get; set; }
    }
}
