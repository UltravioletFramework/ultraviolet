using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace TwistedLogik.Ultraviolet.Design
{
    /// <summary>
    /// Represents a control which allows the user to edit the values of a <see cref="Matrix"/> structure.
    /// </summary>
    public partial class MatrixEditorControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixEditorControl"/> class.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix"/> value to edit.</param>
        /// <param name="editorService">The editor service which is controlling the editor.</param>
        public MatrixEditorControl(Matrix matrix, IWindowsFormsEditorService editorService)
        {
            InitializeComponent();

            m11.Value = (Decimal)matrix.M11;
            m12.Value = (Decimal)matrix.M12;
            m13.Value = (Decimal)matrix.M13;
            m14.Value = (Decimal)matrix.M14;

            m21.Value = (Decimal)matrix.M21;
            m22.Value = (Decimal)matrix.M22;
            m23.Value = (Decimal)matrix.M23;
            m24.Value = (Decimal)matrix.M24;

            m31.Value = (Decimal)matrix.M31;
            m32.Value = (Decimal)matrix.M32;
            m33.Value = (Decimal)matrix.M33;
            m34.Value = (Decimal)matrix.M34;

            m41.Value = (Decimal)matrix.M41;
            m42.Value = (Decimal)matrix.M42;
            m43.Value = (Decimal)matrix.M43;
            m44.Value = (Decimal)matrix.M44;
        }

        /// <summary>
        /// Gets or sets the <see cref="Matrix"/> value being edited.
        /// </summary>
        public Matrix Matrix
        {
            get
            {
                return new Matrix(
                    (Single)m11.Value, (Single)m12.Value, (Single)m13.Value, (Single)m14.Value,
                    (Single)m21.Value, (Single)m22.Value, (Single)m23.Value, (Single)m24.Value,
                    (Single)m31.Value, (Single)m32.Value, (Single)m33.Value, (Single)m34.Value,
                    (Single)m41.Value, (Single)m42.Value, (Single)m43.Value, (Single)m44.Value
                );
            }
            set
            {
                m11.Value = (Decimal)value.M11;
                m12.Value = (Decimal)value.M12;
                m13.Value = (Decimal)value.M13;
                m14.Value = (Decimal)value.M14;

                m21.Value = (Decimal)value.M21;
                m22.Value = (Decimal)value.M22;
                m23.Value = (Decimal)value.M23;
                m24.Value = (Decimal)value.M24;

                m31.Value = (Decimal)value.M31;
                m32.Value = (Decimal)value.M32;
                m33.Value = (Decimal)value.M33;
                m34.Value = (Decimal)value.M34;

                m41.Value = (Decimal)value.M41;
                m42.Value = (Decimal)value.M42;
                m43.Value = (Decimal)value.M43;
                m44.Value = (Decimal)value.M44;
            }
        }

        /// <summary>
        /// Handles the <see cref="Control.Click"/> event for the "Identity" button.
        /// </summary>
        private void btnIdentity_Click(object sender, EventArgs e)
        {
            Matrix = Matrix.Identity;
        }
    }
}
