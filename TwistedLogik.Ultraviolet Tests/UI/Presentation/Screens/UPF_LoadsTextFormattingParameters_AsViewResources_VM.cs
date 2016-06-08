using System;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;

namespace TwistedLogik.Ultraviolet.Tests.UI.Presentation.Screens
{
    public class RainbowGlyphShader : GlyphShader
    {
        public override void Execute(ref GlyphShaderContext context, ref GlyphData data, Int32 index)
        {
            data.Color = Colors[index % Colors.Length];
        }

        private static readonly Color[] Colors = new[]
        { Color.Red, Color.Blue, Color.Lime };
    }

    public class UPF_LoadsTextFormattingParameters_AsViewResources_VM
    { }
}
