using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace Ultraviolet.Design
{
    /// <summary>
    /// Represents an type editor for the <see cref="Radians"/> structure.
    /// </summary>
    public class RadiansEditor : UITypeEditor
    {
        /// <inheritdoc/>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        /// <inheritdoc/>
        public override Object EditValue(ITypeDescriptorContext context, IServiceProvider provider, Object value)
        {
            if (provider != null)
            {
                editorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            }

            if (editorService != null)
            {
                var control = new RadiansEditorControl((Radians)value, editorService);

                editorService.DropDownControl(control);

                value = control.Radians;
            }

            return value;
        }

        // State values.
        private IWindowsFormsEditorService editorService;
    }
}
