using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Security;
using GdiColor = System.Drawing.Color;
using GdiColorEditor = System.Drawing.Design.ColorEditor;
using UvColor = Ultraviolet.Color;

namespace Ultraviolet.Design
{
    /// <summary>
    /// Represents a custom property editor for the <see cref="Color"/> structure.
    /// </summary>
    [SecurityCritical]
    public class ColorEditor : GdiColorEditor
    {
        /// <inheritdoc/>
        [SecuritySafeCritical]
        public override Object EditValue(ITypeDescriptorContext context, IServiceProvider provider, Object value)
        {
            var xnaColorValue = (value == null) ? UvColor.White : (UvColor)value;

            var selectedGdi = (GdiColor)base.EditValue(context, provider, GdiColorFromUvColor(xnaColorValue));
            var selectedXna = UvColorFromGdiColor(selectedGdi);

            return selectedXna;
        }

        /// <inheritdoc/>
        [SecuritySafeCritical]
        public override void PaintValue(PaintValueEventArgs e)
        {
            if (e.Value is UvColor)
            {
                var overrideValue = GdiColorFromUvColor((UvColor)e.Value);
                var overrideEventData = new PaintValueEventArgs(e.Context, overrideValue, e.Graphics, e.Bounds);
                base.PaintValue(overrideEventData);
            }
            else
            {
                base.PaintValue(e);
            }
        }

        /// <inheritdoc/>
        private static GdiColor GdiColorFromUvColor(UvColor color)
        {
            return GdiColor.FromArgb(color.A, color.R, color.G, color.B);
        }

        /// <inheritdoc/>
        private static UvColor UvColorFromGdiColor(GdiColor color)
        {
            return new UvColor(color.R, color.G, color.B) * (color.A / 255f);
        }
    }
}
