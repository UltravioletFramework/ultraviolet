using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace TwistedLogik.Ultraviolet.Design
{
    /// <summary>
    /// Represents a control which allows the user to edit the values of a <see cref="Radians"/> structure.
    /// </summary>
    public partial class RadiansEditorControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RadiansEditorControl"/> class.
        /// </summary>
        /// <param name="radians">The <see cref="Radians"/> value to edit.</param>
        /// <param name="editorService">The editor service which is controlling the editor.</param>
        public RadiansEditorControl(Radians radians, IWindowsFormsEditorService editorService)
        {
            Radians = radians;

            DoubleBuffered = true;

            this.editorService = editorService;

            InitializeComponent();

            RecalculateSelectionCircle();
        }

        /// <summary>
        /// Gets or sets the <see cref="Radians"/> value being edited.
        /// </summary>
        public Radians Radians
        {
            get;
            set;
        }

        /// <inheritdoc/>
        protected override void OnClientSizeChanged(EventArgs e)
        {
            RecalculateSelectionCircle();

            base.OnClientSizeChanged(e);
        }

        /// <inheritdoc/>
        protected override void OnClick(EventArgs e)
        {
            Radians = (Radians)radians;

            editorService.CloseDropDown();

            base.OnClick(e);
        }

        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawLine(Pens.Silver, new Point(offset, radius), new Point(offset + (2 * radius), radius));
            e.Graphics.DrawLine(Pens.Silver, new Point(offset + radius, 0), new Point(offset + radius, 2 * radius));
            e.Graphics.DrawLine(Pens.Black, new Point(offset + radius, 0), new Point(offset + radius, radius));

            e.Graphics.DrawLine(Pens.Red, center, target);
            e.Graphics.DrawArc(Pens.DarkRed, 
                new System.Drawing.Rectangle(offset + (radius / 2), radius / 2, radius, radius), -90, (Int32)degrees);

            e.Graphics.DrawEllipse(Pens.Black, new System.Drawing.Rectangle(offset, 0, radius * 2, radius * 2));

            var stringRepresentation = String.Format("{0:0.00} radians\n{1:0.00} degrees", radians, degrees);
            var stringFormat         = new StringFormat()
            {
                Alignment     = StringAlignment.Center,
                LineAlignment = StringAlignment.Far
            };

            var stringOffset = 16;
            var stringArea   = new System.Drawing.RectangleF(0, stringOffset, ClientRectangle.Width, ClientRectangle.Height - (stringOffset * 2));

            e.Graphics.DrawString(stringRepresentation, SystemFonts.DefaultFont, Brushes.Black, stringArea, stringFormat);

            base.OnPaint(e);
        }

        /// <inheritdoc/>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            UpdateAngle();

            Invalidate();

            base.OnMouseMove(e);
        }

        /// <summary>
        /// Recalculates the position and radius of the selection circle.
        /// </summary>
        private void RecalculateSelectionCircle()
        {
            this.radius = Math.Min(ClientSize.Width / 2, ClientSize.Height / 2);
            this.offset = (ClientSize.Width - (2 * radius)) / 2;
            this.center = new Point(offset + radius, radius);

            UpdateAngle();
        }

        /// <summary>
        /// Updates the currently selected angle.
        /// </summary>
        private void UpdateAngle()
        {
            var cursor = PointToClient(System.Windows.Forms.Cursor.Position);

            var cursorVector = new Vector2(cursor.X, cursor.Y);
            var centerVector = new Vector2(center.X, center.Y);
            var dirVector    = Vector2.Normalize(cursorVector - centerVector);
            var upVector     = -Vector2.UnitY;
            var targetVector = dirVector * radius;
            if (Single.IsNaN(targetVector.X) || Single.IsNaN(targetVector.Y))
            {
                this.radians = 0;
                this.degrees = 0;
                return;
            }

            target = new Point(center.X + (Int32)targetVector.X, center.Y + (Int32)targetVector.Y);

            var angle = Math.Atan2(dirVector.Y, dirVector.X) - Math.Atan2(upVector.Y, upVector.X);
            if (angle < 0)
            {
                angle += Math.PI * 2.0;
            }

            this.radians = (Single)angle;
            this.degrees = (Single)angle * 57.2957795f;
        }

        // State values.
        private readonly IWindowsFormsEditorService editorService;
        private Single radians;
        private Single degrees;
        private Point center;
        private Point target;
        private Int32 radius;
        private Int32 offset;
    }
}
