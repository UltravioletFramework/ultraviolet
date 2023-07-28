using System;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.ImGuiViewProvider.Bindings;

namespace Ultraviolet.ImGuiViewProvider
{
    /// <summary>
    /// Represents a buffer for ImGui geometry.
    /// </summary>
    public sealed unsafe class ImGuiGeometryBuffer : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImGuiGeometryBuffer"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="view">The view which owns the geometry buffer.</param>
        public ImGuiGeometryBuffer(UltravioletContext uv, ImGuiView view)
            : base(uv)
        {
            this.effect = SpriteBatchEffect.Create();
            this.view = view;
        }

        /// <summary>
        /// Draws the specified data.
        /// </summary>
        /// <param name="drawDataPtr">A pointer to the ImGui data to draw.</param>
        public void Draw(ref ImDrawDataPtr drawDataPtr)
        {
            if (drawDataPtr.CmdListsCount == 0)
                return;

            DrawBuffers(ref drawDataPtr);            
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (geometryStream != null)
            {
                geometryStream.Dispose();
                geometryStream = null;
            }
            if (vertexBuffer != null)
            {
                vertexBuffer.Dispose();
                vertexBuffer = null;
            }
            if (indexBuffer != null)
            {
                indexBuffer.Dispose();
                indexBuffer = null;
            }
            if (effect != null)
            {
                effect.Dispose();
                effect = null;
            }
            if (rasterizerState != null)
            {
                rasterizerState.Dispose();
                rasterizerState = null;
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Ensures that the geometry buffers exist and have sufficient size.
        /// </summary>
        private void EnsureBuffers(ref ImDrawDataPtr drawDataPtr)
        {
            var dirty = false;
            var vtxCount = 0;
            var idxCount = 0;
            for (var i = 0; i < drawDataPtr.CmdListsCount; i++)
            {
                var cmd = drawDataPtr.CmdListsRange[i];
                vtxCount = Math.Max(vtxCount, cmd.VtxBuffer.Size);
                idxCount = Math.Max(idxCount, cmd.IdxBuffer.Size);
            }

            if (vertexBuffer == null || vertexBuffer.VertexCount < vtxCount)
            {
                if (vertexBuffer != null)
                    vertexBuffer.Dispose();

                vertexBuffer = DynamicVertexBuffer.Create(ImGuiVertex.VertexDeclaration, vtxCount);
                dirty = true;
            }

            if (indexBuffer == null || indexBuffer.IndexCount < idxCount)
            {
                if (indexBuffer != null)
                    indexBuffer.Dispose();

                indexBuffer = DynamicIndexBuffer.Create(IndexBufferElementType.Int16, idxCount);
                dirty = true;
            }

            if (rasterizerState == null)
            {
                rasterizerState = RasterizerState.Create();
                rasterizerState.ScissorTestEnable = true;
            }

            if (geometryStream == null || dirty)
            {
                this.geometryStream = GeometryStream.Create();
                this.geometryStream.Attach(this.vertexBuffer);
                this.geometryStream.Attach(this.indexBuffer);
            }
        }

        /// <summary>
        /// Draws the contents of the vertex and index buffer.
        /// </summary>
        private void DrawBuffers(ref ImDrawDataPtr drawDataPtr)
        {
            EnsureBuffers(ref drawDataPtr);

            var gfx = Ultraviolet.GetGraphics();
            gfx.SetGeometryStream(geometryStream);
            gfx.SetBlendState(BlendState.NonPremultiplied);
            gfx.SetDepthStencilState(DepthStencilState.None);
            gfx.SetRasterizerState(rasterizerState);
            gfx.SetSamplerState(0, SamplerState.LinearClamp);

            for (var i = 0; i < drawDataPtr.CmdListsCount; i++)
            {
                var cmdList = drawDataPtr.CmdListsRange[i];

                this.vertexBuffer.SetRawData(cmdList.VtxBuffer.Data, 0, 0,
                    cmdList.VtxBuffer.Size * sizeof(ImGuiVertex), SetDataOptions.Discard);

                this.indexBuffer.SetRawData(cmdList.IdxBuffer.Data, 0, 0,
                    cmdList.IdxBuffer.Size * sizeof(UInt16), SetDataOptions.Discard);

                var idxOffset = 0;

                for (int j = 0; j < cmdList.CmdBuffer.Size; j++)
                {
                    var cmd = cmdList.CmdBuffer[j];

                    gfx.SetScissorRectangle(
                        (Int32)cmd.ClipRect.X,
                        (Int32)cmd.ClipRect.Y,
                        (Int32)(cmd.ClipRect.Z - cmd.ClipRect.X),
                        (Int32)(cmd.ClipRect.W - cmd.ClipRect.Y));

                    var texture = view.Textures.Get((Int32)cmd.TextureId);
                    if (texture != null)
                    {
                        effect.SrgbColor = gfx.Capabilities.SrgbEncodingEnabled;
                        effect.Texture = texture;
                        effect.TextureSize = new Size2(1, 1);
                        effect.MatrixTransform = Matrix.CreateOrthographicOffCenter(0,
                            ImGui.GetIO().DisplaySize.X,
                            ImGui.GetIO().DisplaySize.Y, 0, 0, 1);

                        foreach (var pass in effect.CurrentTechnique.Passes)
                        {
                            pass.Apply();
                            gfx.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, idxOffset, (Int32)cmd.ElemCount / 3);
                        }
                    }

                    idxOffset += (Int32)cmd.ElemCount;
                }
            }

            gfx.SetRasterizerState(RasterizerState.CullCounterClockwise);
        }

        // The view which owns the geometry buffer.
        private readonly ImGuiView view;

        // Geometry resources.
        private GeometryStream geometryStream;
        private DynamicVertexBuffer vertexBuffer;
        private DynamicIndexBuffer indexBuffer;
        private SpriteBatchEffect effect;
        private RasterizerState rasterizerState;
    }
}
