using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents an service which can render instances of the <see cref="ModelScene"/> class.
    /// </summary>
    public abstract class ModelSceneRendererBase<TScene, TNode>
        where TScene : class, IModelSceneProvider<TNode>
        where TNode : class, IModelNodeProvider<TNode>
    {
        /// <summary>
        /// Draws the specified scene.
        /// </summary>
        /// <param name="scene">The scene to draw.</param>
        /// <param name="camera">The camera with which to draw the scene.</param>
        /// <param name="worldMatrix">The scene's world matrix.</param>
        public void Draw(TScene scene, Camera camera, ref Matrix worldMatrix)
        {
            Contract.Require(scene, nameof(scene));
            Contract.Require(camera, nameof(camera));

            OnDrawingModelScene(scene, camera, ref worldMatrix);

            var effect = default(Effect);
            for (var i = 0; i < scene.ChildNodeCount; i++)
            {
                var node = scene.GetChildNode(i);
                DrawNode(node, camera, ref effect, worldMatrix);
            }

            OnDrawnModelScene();
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
        /// Called when the renderer is drawing a <typeparamref name="TScene"/> instance.
        /// </summary>
        /// <param name="scene">The <typeparamref name="TScene"/> instance that is being rendered.</param>
        /// <param name="camera">The camera with which the scene is being drawn.</param>
        /// <param name="transform">The transformation which is being applied to the node.</param>
        protected virtual void OnDrawingModelScene(TScene scene, Camera camera, ref Matrix transform)
        { }

        /// <summary>
        /// Called when the renderer is done drawing a <typeparamref name="TScene"/> instance.
        /// </summary>
        protected virtual void OnDrawnModelScene()
        { }

        /// <summary>
        /// Called when the renderer is drawing a <typeparamref name="TNode"/> instance.
        /// </summary>
        /// <param name="node">The <typeparamref name="TNode"/> instance that is being rendered.</param>
        /// <param name="camera">The camera with which the scene is being drawn.</param>
        /// <param name="effect">The current <see cref="Effect"/> instance.</param>
        /// <param name="transform">The transformation which is being applied to the node.</param>
        protected virtual void OnDrawingModelNode(TNode node, Camera camera, Effect effect, ref Matrix transform) 
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
        /// Draws a <typeparamref name="TNode"/> instance.
        /// </summary>
        private void DrawNode(TNode node, Camera camera, ref Effect effect, Matrix transform)
        {
            var modelNode = node.ModelNode;
            if (modelNode == null || !modelNode.HasGeometry)
                return;

            modelNode.Transform.AsMatrix(out var nodeTransformMatrix);
            transform = Matrix.Multiply(nodeTransformMatrix, transform);
            OnDrawingModelNode(node, camera, effect, ref transform);

            var modelMesh = modelNode.Mesh;
            if (modelMesh != null)
            {
                foreach (var geometry in modelMesh.Geometries)
                {
                    var material = geometry.Material;
                    OnDrawingModelMesh(modelMesh, camera, ref transform, ref material);

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

            for (var i = 0; i < node.ChildNodeCount; i++)
            {
                var child = node.GetChildNode(i);
                DrawNode(child, camera, ref effect, transform);
            }
        }
    }
}
