using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents an service which can render instances of the <see cref="ModelScene"/> class.
    /// </summary>
    public class ModelSceneRenderer
    {
        /// <summary>
        /// Draws the specified scene.
        /// </summary>
        /// <param name="scene">The scene to draw.</param>
        /// <param name="camera">The camera with which to draw the scene.</param>
        /// <param name="worldMatrix">The scene's world matrix.</param>
        public void Draw(ModelScene scene, Camera camera, ref Matrix worldMatrix)
        {
            Contract.Require(scene, nameof(scene));
            Contract.Require(camera, nameof(camera));

            var effect = default(Effect);
            foreach (var node in scene.Nodes)
                DrawNode(node, camera, ref effect, worldMatrix);
        }

        /// <summary>
        /// Called when the renderer changes to a new effect.
        /// </summary>
        /// <param name="effect">The effect which will be used to draw subsequent geometry.</param>
        /// <param name="camera">The camera with which the scene is being drawn.</param>
        protected virtual void OnEffectChanged(Effect effect, Camera camera)
        {
            var parameters = effect.Parameters;

            if (effect is IEffectMatrices effectMatrices)
            {
                camera.GetViewMatrix(out var view);
                effectMatrices.View = view;

                camera.GetProjectionMatrix(out var proj);
                effectMatrices.Projection = proj;
            }
            else
            {
                var epView = parameters.GetCameraParameter(CameraParameter.View);
                if (epView != null)
                {
                    camera.GetViewMatrix(out var view);
                    epView.SetValueRef(ref view);
                }

                var epProjection = parameters.GetCameraParameter(CameraParameter.Projection);
                if (epProjection != null)
                {
                    camera.GetProjectionMatrix(out var projection);
                    epProjection.SetValueRef(ref projection);
                }
            }

            if (effect is IEffectViewProj effectViewProj)
            {
                camera.GetViewProjectionMatrix(out var viewProj);
                effectViewProj.ViewProj = viewProj;
            }
            else
            {
                var epViewProj = parameters.GetCameraParameter(CameraParameter.ViewProj);
                if (epViewProj != null)
                {
                    camera.GetViewProjectionMatrix(out var viewProj);
                    epViewProj.SetValueRef(ref viewProj);
                }
            }
        }

        /// <summary>
        /// Called when the renderer is drawing a <see cref="ModelNode"/> instance.
        /// </summary>
        /// <param name="node">The <see cref="ModelNode"/> instance that is being rendered.</param>
        /// <param name="camera">The camera with which the scene is being drawn.</param>
        /// <param name="transform">The transformation which is being applied to the node.</param>
        protected virtual void OnDrawingModelNode(ModelNode node, Camera camera, ref Matrix transform) 
        { }

        /// <summary>
        /// Called when the renderer is drawing a <see cref="ModelMesh"/> instance.
        /// </summary>
        /// <param name="mesh">The <see cref="ModelMesh"/> instance that is being rendered.</param>
        /// <param name="camera">The camera with which the scene is being drawn.</param>
        /// <param name="transform">The transformation which is being applied to the node.</param>
        /// <param name="material">The <see cref="Material"/> with which the mesh is being rendered.</param>
        protected virtual void OnDrawingModelMesh(ModelMesh mesh, Camera camera, ref Matrix transform, ref Material material) 
        {
            var effect = material.Effect;
            var parameters = effect.Parameters;

            if (effect is IEffectMatrices effectMatrices)
            {
                effectMatrices.World = transform;
            }
            else
            {
                var epWorld = parameters.GetCameraParameter(CameraParameter.World);
                if (epWorld != null)
                {
                    epWorld.SetValueRef(ref transform);
                }
            }

            if (effect is IEffectWorldViewProj effectWorldViewProj)
            {
                camera.GetViewProjectionMatrix(out var viewProj);
                Matrix.Multiply(ref transform, ref viewProj, out var worldViewProj);
                effectWorldViewProj.WorldViewProj = worldViewProj;
            }
            else
            { 
                var epWorldViewProj = parameters.GetCameraParameter(CameraParameter.WorldViewProj);
                if (epWorldViewProj != null)
                {
                    camera.GetViewProjectionMatrix(out var viewProj);
                    Matrix.Multiply(ref transform, ref viewProj, out var worldViewProj);
                    epWorldViewProj.SetValueRef(ref worldViewProj);
                }
            }
        }

        /// <summary>
        /// Called when the renderer is drawing the geometry of a <see cref="ModelMesh"/> instance.
        /// </summary>
        /// <param name="geometry">The <see cref="ModelMeshGeometry"/> which is being drawn.</param>
        /// <param name="material">The <see cref="Material"/> with which to draw the geometry.</param>
        protected virtual void OnDrawingModelMeshGeometry(ModelMeshGeometry geometry, Material material)
        {
            material.Effect.ConfigureForGeometry(geometry.GeometryStream);
            material.Apply();
            foreach (var pass in material.Effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                var gfx = geometry.GeometryStream.Ultraviolet.GetGraphics();
                gfx.SetGeometryStream(geometry.GeometryStream);

                if (geometry.GeometryStream.HasIndices)
                {
                    gfx.DrawIndexedPrimitives(geometry.PrimitiveType, 0, geometry.PrimitiveCount);
                }
                else
                {
                    gfx.DrawPrimitives(geometry.PrimitiveType, 0, geometry.PrimitiveCount);
                }
            }
        }

        /// <summary>
        /// Draws a <see cref="ModelNode"/> instance.
        /// </summary>
        private void DrawNode(ModelNode node, Camera camera, ref Effect effect, Matrix transform)
        {
            transform = Matrix.Multiply(node.Transform, transform);
            OnDrawingModelNode(node, camera, ref transform);

            if (node.Mesh != null)
            {
                foreach (var geometry in node.Mesh.Geometries)
                {
                    var material = geometry.Material;
                    OnDrawingModelMesh(node.Mesh, camera, ref transform, ref material);

                    if (material == null)
                        continue;

                    if (effect != material.Effect)
                    {
                        effect = material.Effect;
                        OnEffectChanged(effect, camera);
                    }

                    OnDrawingModelMeshGeometry(geometry, material);
                }
            }

            foreach (var child in node.Children)
                DrawNode(child, camera, ref effect, transform);
        }
    }
}
