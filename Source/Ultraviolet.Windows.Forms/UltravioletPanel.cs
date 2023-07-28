using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Ultraviolet.Core;
using Ultraviolet.Platform;

namespace Ultraviolet.Windows.Forms
{
    /// <summary>
    /// Represents a panel designed to be enlisted into an Ultraviolet context.
    /// </summary>
    public partial class UltravioletPanel : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the UltravioletPanel class.
        /// </summary>
        public UltravioletPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when the panel is being drawn.
        /// </summary>
        public event EventHandler Drawing;

        /// <summary>
        /// Occurs when the panel's Ultraviolet window is about to be created.
        /// </summary>
        public event EventHandler CreatingUltravioletWindow;

        /// <summary>
        /// Occurs after the panel's Ultraviolet window has been crated.
        /// </summary>
        public event EventHandler CreatedUltravioletWindow;

        /// <summary>
        /// Occurs when the panel's Ultraviolet window is about to be destroyed.
        /// </summary>
        public event EventHandler DestroyingUltravioletWindow;

        /// <summary>
        /// Occurs after the panel's Ultraviolet window has been destroyed.
        /// </summary>
        public event EventHandler DestroyedUltravioletWindow;

        /// <summary>
        /// Gets the panel's Ultraviolet context.
        /// </summary>
        public UltravioletContext Ultraviolet
        {
            get
            {
                Contract.EnsureNotDisposed(this, IsDisposed);

                return uv;
            }
        }

        /// <summary>
        /// Gets the panel's Ultraviolet window.
        /// </summary>
        public IUltravioletWindow UltravioletWindow
        {
            get 
            {
                Contract.EnsureNotDisposed(this, IsDisposed);

                return uvWindow; 
            }
        }

        /// <summary>
        /// Gets or sets the label displayed on this panel at design time.
        /// </summary>
        [DefaultValue("uv")]
        public String DesignLabel
        {
            get 
            {
                Contract.EnsureNotDisposed(this, IsDisposed);

                return designLabel;
            }
            set
            {
                Contract.EnsureNotDisposed(this, IsDisposed);

                designLabel = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Raises the Drawing event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnDrawing(EventArgs e) =>
            Drawing?.Invoke(this, e);

        /// <summary>
        /// Raises the CreatingUltravioletWindow event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnCreatingUltravioletWindow(EventArgs e) =>
            CreatingUltravioletWindow?.Invoke(this, e);

        /// <summary>
        /// Raises the CreatedUltravioletWindow event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnCreatedUltravioletWindow(EventArgs e) =>
            CreatedUltravioletWindow?.Invoke(this, e);

        /// <summary>
        /// Raises the DestroyingUltravioletWindow event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnDestroyingUltravioletWindow(EventArgs e) =>
            DestroyingUltravioletWindow?.Invoke(this, e);

        /// <summary>
        /// Raises the DestroyedUltravioletWindow event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnDestroyedUltravioletWindow(EventArgs e) =>
            DestroyedUltravioletWindow?.Invoke(this, e);

        /// <summary>
        /// Raises the Paint event.
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (DesignMode)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.Clear(System.Drawing.Color.Magenta);
                if (!String.IsNullOrEmpty(designLabel))
                {
                    using (var font = new FontFamily("Arial"))
                    {
                        var format = new StringFormat();
                        format.Alignment = StringAlignment.Center;
                        format.LineAlignment = StringAlignment.Center;

                        var path = new GraphicsPath();
                        path.AddString(designLabel, font, (int)FontStyle.Bold, 32f, 
                            new System.Drawing.RectangleF(0, 0, ClientSize.Width, ClientSize.Height), format);

                        e.Graphics.FillPath(Brushes.White, path);
                        e.Graphics.DrawPath(Pens.Black, path);
                    }
                }
            }
            base.OnPaint(e);
        }

        /// <summary>
        /// Raises the PaintBackground event.
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the event data.</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {

        }

        /// <summary>
        /// Raises the Load event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            if (uv == null && !DesignMode)
            {
                var uvform = TopLevelControl as UltravioletForm;
                if (uvform == null)
                    throw new InvalidOperationException(WindowsFormsStrings.UltravioletFormRequired);
                
                CreateUltravioletWindow(uvform.Ultraviolet);
            }
            base.OnLoad(e);
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (uv != null && !uv.Disposed)
                {
                    DestroyUltravioletWindow();
                }
                SafeDispose.Dispose(components);
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Handles the Ultraviolet window's Drawing event.
        /// </summary>
        /// <param name="window">The window being rendered.</param>
        /// <param name="time">Time elapsed since the last call to Draw.</param>
        private void uvWindow_Drawing(IUltravioletWindow window, UltravioletTime time)
        {
            OnDrawing(EventArgs.Empty);
        }

        /// <summary>
        /// Enlists the panel in the specified Ultraviolet context.
        /// </summary>
        /// <param name="uv">The Ultraviolet context in which to enlist the panel.</param>
        private void CreateUltravioletWindow(UltravioletContext uv)
        {
            Contract.Require(uv, nameof(uv));

            if (this.uv != null)
                throw new InvalidOperationException(WindowsFormsStrings.PanelAlreadyEnlisted);
            
            OnCreatingUltravioletWindow(EventArgs.Empty);

            this.uv = uv;

            this.uvWindow = uv.GetPlatform().Windows.CreateFromNativePointer(this.Handle);
            this.uvWindow.Drawing += uvWindow_Drawing;
            
            OnCreatedUltravioletWindow(EventArgs.Empty);
        }

        /// <summary>
        /// Releases the panel from its Ultraviolet context.
        /// </summary>
        private void DestroyUltravioletWindow()
        {
            if (this.uv == null)
                throw new InvalidOperationException(WindowsFormsStrings.PanelNotEnlisted);
            
            OnDestroyingUltravioletWindow(EventArgs.Empty);

            this.uvWindow.Drawing -= uvWindow_Drawing;
            this.uv.GetPlatform().Windows.Destroy(uvWindow);
            this.uvWindow = null;

            this.uv = null;

            OnDestroyedUltravioletWindow(EventArgs.Empty);
        }

        // The panel's ID within the Ultraviolet context.
        private UltravioletContext uv;
        private IUltravioletWindow uvWindow;

        // Property values.
        private String designLabel = "uv";
    }
}
