using System;
using System.Reflection;
using System.Text;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Presentation.Animations;

namespace Ultraviolet.Presentation
{
    partial class PresentationFoundation
    {
        /// <summary>
        /// Represents a diagnostics panel which displays various performance metrics relating to the Presentation Foundation.
        /// </summary>
        private class DiagnosticsPanel : UltravioletResource
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DiagnosticsPanel"/> class.
            /// </summary>
            /// <param name="uv">The Ultraviolet context.</param>
            public DiagnosticsPanel(UltravioletContext uv)
                : base(uv)
            { }
            
            /// <summary>
            /// Draws the diagnostics panel to the current window.
            /// </summary>
            public void Draw()
            {
                LoadContentIfNecessary();

                var window = Ultraviolet.GetPlatform().Windows.GetCurrent();
                if (window == null)
                    throw new InvalidOperationException();

                var upf = Ultraviolet.GetUI().GetPresentationFoundation();

                var panelWidth = 400;
                var panelHeight = 16f + (7.5f * font.Regular.LineSpacing);
                var panelArea = new RectangleF((window.Compositor.Width - panelWidth) / 2, 0, panelWidth, panelHeight);

                var colorHeader = Color.Yellow;
                var colorSubheader = new Color(200, 200, 0);

                var colWidth = (panelWidth - 24) / 3;
                var xCol1 = panelArea.X + 8f;
                var xCol2 = xCol1 + colWidth + 8f;
                var xCol3 = xCol2 + colWidth + 8f;

                var yLine = 8f;

                spriteBatch.Begin();
                spriteBatch.Draw(blankTexture, panelArea, Color.Black * 0.5f);

                spriteBatch.DrawString(font, "MEMORY", new Vector2(xCol1, yLine), colorHeader);
                spriteBatch.DrawString(font, "UPDATE", new Vector2(xCol2, yLine), colorHeader);
                spriteBatch.DrawString(font, "DRAW", new Vector2(xCol3, yLine), colorHeader);

                yLine += font.Regular.LineSpacing;
                
                formatter.Reset();
                formatter.AddArgument(GC.GetTotalMemory(false) / 1024.0);
                formatter.Format("{0:decimals:2} Kb", buffer);
                
                spriteBatch.DrawString(font, buffer, new Vector2(xCol1, yLine), Color.White);

                formatter.Reset();
                formatter.AddArgument(upf.PerformanceStats.TimeInUpdateLastFrame.TotalMilliseconds);
                formatter.Format("{0:decimals:2} ms", buffer);

                spriteBatch.DrawString(font, buffer, new Vector2(xCol2, yLine), Color.White);

                formatter.Reset();
                formatter.AddArgument(upf.PerformanceStats.TimeInDrawLastFrame.TotalMilliseconds);
                formatter.Format("{0:decimals:2} ms", buffer);

                spriteBatch.DrawString(font, buffer, new Vector2(xCol3, yLine), Color.White);

                yLine += font.Regular.LineSpacing * 1.5f;

                spriteBatch.DrawString(font, "POOLS", new Vector2(xCol1, yLine), colorHeader);

                yLine += font.Regular.LineSpacing;

                spriteBatch.DrawString(font, "Simple Clock", new Vector2(xCol1, yLine), colorSubheader);
                spriteBatch.DrawString(font, "Storyboard Instance", new Vector2(xCol2, yLine), colorSubheader);
                spriteBatch.DrawString(font, "Storyboard Clock", new Vector2(xCol3, yLine), colorSubheader);

                yLine += font.Regular.LineSpacing;

                formatter.Reset();
                formatter.AddArgument(SimpleClockPool.Instance.Active);
                formatter.AddArgument(SimpleClockPool.Instance.Available);
                formatter.AddArgument(SimpleClockPool.Instance.Active + SimpleClockPool.Instance.Available);
                formatter.Format("{0} / {1} [{2}]", buffer);

                spriteBatch.DrawString(font, buffer, new Vector2(xCol1, yLine), Color.White);

                formatter.Reset();
                formatter.AddArgument(StoryboardInstancePool.Instance.Active);
                formatter.AddArgument(StoryboardInstancePool.Instance.Available);
                formatter.AddArgument(StoryboardInstancePool.Instance.Active + StoryboardInstancePool.Instance.Available);
                formatter.Format("{0} / {1} [{2}]", buffer);

                spriteBatch.DrawString(font, buffer, new Vector2(xCol2, yLine), Color.White);

                formatter.Reset();
                formatter.AddArgument(StoryboardClockPool.Instance.Active);
                formatter.AddArgument(StoryboardClockPool.Instance.Available);
                formatter.AddArgument(StoryboardClockPool.Instance.Active + StoryboardClockPool.Instance.Available);
                formatter.Format("{0} / {1} [{2}]", buffer);

                spriteBatch.DrawString(font, buffer, new Vector2(xCol3, yLine), Color.White);

                yLine += font.Regular.LineSpacing;

                spriteBatch.DrawString(font, "OOB Render Target", new Vector2(xCol1, yLine), colorSubheader);
                spriteBatch.DrawString(font, "Weak Refs", new Vector2(xCol2, yLine), colorSubheader);

                yLine += font.Regular.LineSpacing;
                
                formatter.Reset();
                formatter.AddArgument(upf.OutOfBandRenderer.ActiveRenderTargets);
                formatter.AddArgument(upf.OutOfBandRenderer.AvailableRenderTargets);
                formatter.AddArgument(upf.OutOfBandRenderer.ActiveRenderTargets + upf.OutOfBandRenderer.AvailableRenderTargets);
                formatter.Format("{0} / {1} [{2}]", buffer);

                spriteBatch.DrawString(font, buffer, new Vector2(xCol1, yLine), Color.White);

                formatter.Reset();
                formatter.AddArgument(WeakReferencePool.Instance.Active);
                formatter.AddArgument(WeakReferencePool.Instance.Available);
                formatter.AddArgument(WeakReferencePool.Instance.Active + WeakReferencePool.Instance.Available);
                formatter.Format("{0} / {1} [{2}]", buffer);

                spriteBatch.DrawString(font, buffer, new Vector2(xCol2, yLine), Color.White);

                spriteBatch.End();
            }
            
            /// <inheritdoc/>
            protected override void Dispose(Boolean disposing)
            {
                if (disposing)
                {
                    SafeDispose.Dispose(spriteBatch);
                    SafeDispose.Dispose(content);
                    SafeDispose.Dispose(blankTexture);
                }
                base.Dispose(disposing);
            }

            /// <summary>
            /// Loads the panel's content assets if they haven't already been loaded.
            /// </summary>
            private void LoadContentIfNecessary()
            {
                if (content != null)
                    return;

                content = ContentManager.Create();
                spriteBatch = SpriteBatch.Create();

                var asm = Assembly.GetExecutingAssembly();
                using (var stream = asm.GetManifestResourceStream("Ultraviolet.Presentation.Resources.Content.Fonts.SegoeUITexture.png"))
                    font = content.LoadFromStream<UltravioletFont>(stream, "png");

                blankTexture = Texture2D.CreateTexture(1, 1);
                blankTexture.SetData(new[] { Color.White });
            }

            // Panel resources.
            private readonly StringFormatter formatter = new StringFormatter();
            private readonly StringBuilder buffer = new StringBuilder();
            private ContentManager content;
            private SpriteBatch spriteBatch;
            private UltravioletFont font;
            private Texture2D blankTexture;
        }
    }
}
