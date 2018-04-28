using System;
using Ultraviolet;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.Input;
using Ultraviolet.Presentation;
using Ultraviolet.Presentation.Input;

namespace UvDebug.Content.UI.Controls
{
    /// <summary>
    /// Represents a UI element that displays a 3D triangle.
    /// </summary>
    [UvmlKnownType]
    public sealed class TriangleDisplay : FrameworkElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TriangleDisplay"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The identifying name of this element within its layout.</param>
        public TriangleDisplay(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Gets or sets the rotation with which the triangle is displayed.
        /// </summary>
        public Single TriangleRotation
        {
            get { return GetValue<Single>(TriangleRotationProperty); }
            set { SetValue(TriangleRotationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the zoom with which the triangle is displayed.
        /// </summary>
        public Single TriangleZoom
        {
            get { return GetValue<Single>(TriangleZoomProperty); }
            set { SetValue(TriangleZoomProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the triangle's first vertex.
        /// </summary>
        public Color V1Color
        {
            get { return GetValue<Color>(V1ColorProperty); }
            set { SetValue(V1ColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the triangle's first vertex.
        /// </summary>
        public Color V2Color
        {
            get { return GetValue<Color>(V2ColorProperty); }
            set { SetValue(V2ColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the triangle's first vertex.
        /// </summary>
        public Color V3Color
        {
            get { return GetValue<Color>(V3ColorProperty); }
            set { SetValue(V3ColorProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="TriangleRotation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TriangleRotationProperty = DependencyProperty.Register("TriangleRotation", typeof(Single), typeof(TriangleDisplay),
            new PropertyMetadata<Single>(0f, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="TriangleZoom"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TriangleZoomProperty = DependencyProperty.Register("TriangleZoom", typeof(Single), typeof(TriangleDisplay),
            new PropertyMetadata<Single>(0f, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="V1Color"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty V1ColorProperty = DependencyProperty.Register("V1Color", typeof(Color), typeof(TriangleDisplay),
            new PropertyMetadata<Color>(Color.White, PropertyMetadataOptions.None, HandleVertexColorChanged));

        /// <summary>
        /// Identifies the <see cref="V1Color"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty V2ColorProperty = DependencyProperty.Register("V2Color", typeof(Color), typeof(TriangleDisplay),
            new PropertyMetadata<Color>(Color.White, PropertyMetadataOptions.None, HandleVertexColorChanged));

        /// <summary>
        /// Identifies the <see cref="V1Color"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty V3ColorProperty = DependencyProperty.Register("V3Color", typeof(Color), typeof(TriangleDisplay),
            new PropertyMetadata<Color>(Color.White, PropertyMetadataOptions.None, HandleVertexColorChanged));

        /// <inheritdoc/>
        protected override void OnMouseDown(MouseDevice device, MouseButton button, RoutedEventData data)
        {
            if (button == MouseButton.Left)
            {
                View.CaptureMouse(this, CaptureMode.Element);
            }
            base.OnMouseDown(device, button, data);
        }

        /// <inheritdoc/>
        protected override void OnMouseUp(MouseDevice device, MouseButton button, RoutedEventData data)
        {
            if (button == MouseButton.Left && IsMouseCaptured)
            {
                View.ReleaseMouse();
            }
            base.OnMouseUp(device, button, data);
        }

        /// <inheritdoc/>
        protected override void OnMouseMove(MouseDevice device, Double x, Double y, Double dx, Double dy, RoutedEventData data)
        {
            if (device.IsButtonDown(MouseButton.Left))
            {
                var twopi = 2.0f * (Single)Math.PI;
                var delta = twopi * (Single)(dx / ActualWidth);
                TriangleRotation = Math.Max(0, Math.Min(twopi, TriangleRotation + delta));
            }
            base.OnMouseMove(device, x, y, dx, dy, data);
        }
        
        /// <inheritdoc/>
        protected override void OnMouseWheel(MouseDevice device, Double x, Double y, RoutedEventData data)
        {
            TriangleZoom = (Single)Math.Max(0, Math.Min(1, TriangleZoom + (y * 0.1f)));

            base.OnMouseWheel(device, x, y, data);
        }

        /// <inheritdoc/>
        protected override void DrawOverride(UltravioletTime time, DrawingContext dc)
        {
            DrawBlank(dc, Color.Black * 0.5f);

            var viewport = AdjustViewportFor3D(dc);

            var triangleRotation = TriangleRotation;
            var triangleDistance = 5f - (TriangleZoom * 2.5f);
            var triangleAspectRatio = (Single)(ActualWidth / ActualHeight);

            var gfx = Ultraviolet.GetGraphics();
            var effect = EnsureEffect();
            effect.World = Matrix.CreateRotationY(TriangleRotation);
            effect.View = Matrix.CreateLookAt(new Vector3(0, 0, triangleDistance), Vector3.Zero, Vector3.Up);
            effect.Projection = Matrix.CreatePerspectiveFieldOfView((Single)(Math.PI / 4.0), triangleAspectRatio, 1f, 1000f);
            effect.VertexColorEnabled = true;
            effect.SrgbColor = gfx.CurrentRenderTargetIsSrgbEncoded;
            
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                gfx.SetRasterizerState(RasterizerState.CullNone);
                gfx.SetGeometryStream(EnsureGeometryStream());
                gfx.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
            }

            gfx.SetViewport(viewport);

            base.DrawOverride(time, dc);
        }

        /// <inheritdoc/>
        protected override void CleanupOverride()
        {
            SafeDispose.DisposeRef(ref geometryStream);
            SafeDispose.DisposeRef(ref vbuffer);
            SafeDispose.DisposeRef(ref effect);

            base.CleanupOverride();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="V1Color"/>, <see cref="V2Color"/>, or <see cref="V3Color"/> dependency properties change.
        /// </summary>
        private static void HandleVertexColorChanged(DependencyObject dobj, Color oldValue, Color newValue)
        {
            var display = (TriangleDisplay)dobj;
            display.UpdateVertexBuffer(display.V1Color, display.V2Color, display.V3Color);
        }

        /// <summary>
        /// Ensures that the control's geometry stream exists.
        /// </summary>
        /// <returns>The control's geometry stream.</returns>
        private GeometryStream EnsureGeometryStream()
        {
            if (geometryStream == null)
            {
                geometryStream = GeometryStream.Create();
                geometryStream.Attach(EnsureVertexBuffer());
            }
            return geometryStream;
        }

        /// <summary>
        /// Updates the colors of the displayed triangle's vertices.
        /// </summary>
        /// <param name="v1">The color of the first vertex.</param>
        /// <param name="v2">The color of the second vertex.</param>
        /// <param name="v3">The color of the third vertex.</param>
        /// <returns>The control's vertex buffer.</returns>
        private VertexBuffer UpdateVertexBuffer(Color v1, Color v2, Color v3)
        {
            var vbuffer = EnsureVertexBuffer();
            vdata[0].Color = v1;
            vdata[1].Color = v2;
            vdata[2].Color = v3;
            vbuffer.SetData(vdata);
            return vbuffer;
        }

        /// <summary>
        /// Ensures that the control's vertex buffer exists.
        /// </summary>
        /// <returns>The control's vertex buffer.</returns>
        private VertexBuffer EnsureVertexBuffer()
        {
            if (vbuffer == null)
            {
                vbuffer = VertexBuffer.Create<VertexPositionColor>(3);
                if (vdata == null)
                {
                    vdata = new[] 
                    {
                        new VertexPositionColor(new Vector3(0, 1, 0), Color.White),
                        new VertexPositionColor(new Vector3(1, -1, 0), Color.White),
                        new VertexPositionColor(new Vector3(-1, -1, 0), Color.White)
                    };
                }
                vbuffer.SetData(vdata);
            }
            return vbuffer;
        }

        /// <summary>
        /// Ensures that the control's rendering effect exists.
        /// </summary>
        /// <returns>The control's rendering effect.</returns>
        private BasicEffect EnsureEffect()
        {
            if (effect == null)
            {
                effect = BasicEffect.Create();
            }
            return effect;
        }

        /// <summary>
        /// Adjusts the viewport in preparation for 3D rendering.
        /// </summary>
        /// <param name="dc">The current drawing context.</param>
        /// <returns>The previous viewport.</returns>
        private Viewport AdjustViewportFor3D(DrawingContext dc)
        {
            dc.Flush();

            var posDips = TransformToAncestor(View.LayoutRoot, Point2D.Zero);
            var posPixs = (Vector2)Display.DipsToPixels(posDips);

            var transform = dc.GlobalTransform;
            Vector2.Transform(ref posPixs, ref transform, out posPixs);
            
            var oldViewport = Ultraviolet.GetGraphics().GetViewport();
            var newViewport = new Viewport((Int32)posPixs.X, (Int32)posPixs.Y, 
                (Int32)Display.DipsToPixels(ActualWidth), (Int32)Display.DipsToPixels(ActualHeight));

            Ultraviolet.GetGraphics().SetViewport(newViewport);

            return oldViewport;
        }

        // Rendering resources.
        private GeometryStream geometryStream;
        private BasicEffect effect;
        private VertexBuffer vbuffer;
        private VertexPositionColor[] vdata;
    }
}
