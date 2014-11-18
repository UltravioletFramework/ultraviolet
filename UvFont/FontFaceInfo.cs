using System;
using System.Drawing;

namespace UvFont
{
    public class FontFaceInfo
    {
        public FontFaceInfo(String name, Font font)
        {
            Name = name;
            Font = font;
        }
        
        public String Name { get; private set; }
        
        public Font Font { get; private set; }

        public Bitmap Texture { get; set; }
    }
}
