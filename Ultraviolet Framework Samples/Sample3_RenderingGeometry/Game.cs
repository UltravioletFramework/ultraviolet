using System;
using System.IO;
using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.OpenGL;
using UltravioletSample.Sample3_RenderingGeometry.Input;

namespace UltravioletSample.Sample3_RenderingGeometry
{
#if ANDROID
    [Android.App.Activity(Label = "Sample 3 - Rendering Geometry", MainLauncher = true, ConfigurationChanges = 
        Android.Content.PM.ConfigChanges.Orientation | 
        Android.Content.PM.ConfigChanges.ScreenSize | 
        Android.Content.PM.ConfigChanges.KeyboardHidden)]
#endif
    public class Game : SampleApplicationBase1
    {
        public Game()
            : base("TwistedLogik", "Sample 3 - Rendering Geometry", uv => uv.GetInput().GetActions())
        {

        }

        public static void Main(String[] args)
        {
            using (var game = new Game())
            {
                game.Run();
            }
        }

        protected override UltravioletContext OnCreatingUltravioletContext()
        {
            return new OpenGLUltravioletContext(this);
        }
        
        protected override void OnLoadingContent()
        {
            this.effect = BasicEffect.Create();

            this.vbuffer = VertexBuffer.Create<VertexPositionColor>(3);
            this.vbuffer.SetData<VertexPositionColor>(new[]
            {
                new VertexPositionColor(new Vector3(0, 1, 0), Color.Red),
                new VertexPositionColor(new Vector3(1, -1, 0), Color.Lime),
                new VertexPositionColor(new Vector3(-1, -1, 0), Color.Blue),
            });

            this.geometryStream = GeometryStream.Create();
            this.geometryStream.Attach(this.vbuffer);

            base.OnLoadingContent();
        }

        protected override void OnUpdating(UltravioletTime time)
        {
            if (Ultraviolet.GetInput().GetActions().ExitApplication.IsPressed())
            {
                Exit();
            }
            base.OnUpdating(time);
        }

        protected override void OnDrawing(UltravioletTime time)
        {
            var gfx         = Ultraviolet.GetGraphics();
            var window      = Ultraviolet.GetPlatform().Windows.GetPrimary();
            var aspectRatio = window.ClientSize.Width / (float)window.ClientSize.Height;

            effect.World              = Matrix.CreateRotationY((float)(2.0 * Math.PI * time.TotalTime.TotalSeconds));
            effect.View               = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
            effect.Projection         = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, aspectRatio, 1f, 1000f);
            effect.VertexColorEnabled = true;

            foreach (var pass in this.effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                gfx.SetRasterizerState(RasterizerState.CullNone);
                gfx.SetGeometryStream(geometryStream);
                gfx.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
            }

            base.OnDrawing(time);
        }

        private BasicEffect effect;
        private VertexBuffer vbuffer;
        private GeometryStream geometryStream;
    }
}
