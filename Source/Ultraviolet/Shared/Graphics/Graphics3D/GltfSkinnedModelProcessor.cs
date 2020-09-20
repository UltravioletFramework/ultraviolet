using System;
using System.Collections.Generic;
using System.Linq;
using SharpGLTF.Schema2;
using Ultraviolet.Content;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents a content processor which converts <see cref="ModelRoot"/> instances to <see cref="SkinnedModel"/> instances.
    /// </summary>
    [ContentProcessor, CLSCompliant(false)]
    public class GltfSkinnedModelProcessor : GltfModelProcessor<SkinnedModel>
    {
        /// <inheritdoc/>
        protected override SkinnedModel CreateModelResource(ContentManager contentManager, ModelRoot input, IList<ModelScene> scenes, IList<Texture2D> textures)
        {
            var totalNodeCount = 0;
            TraverseModelNodes(scenes, n => totalNodeCount++);

            var uvAnimations = new List<SkinnedAnimation>(input.LogicalAnimations.Count);
            foreach (var animation in input.LogicalAnimations)
            {
                var uvNodeAnimations = new List<SkinnedModelNodeAnimation>(totalNodeCount);
                TraverseModelNodes(scenes, n =>
                {
                    var gltfNode = input.LogicalNodes[n.LogicalIndex];

                    var animScale = animation.FindScaleSampler(gltfNode);
                    var animTranslation = animation.FindTranslationSampler(gltfNode);
                    var animRotation = animation.FindRotationSampler(gltfNode);
                    var animMorphWeights = animation.FindMorphSampler(gltfNode);

                    if (animScale != null || animRotation != null || animTranslation != null || animMorphWeights != null)
                    {
                        var curveScale = CreateCurve(animScale, x => x,
                            Vector3CurveStepSampler.Instance, Vector3CurveLinearSampler.Instance, Vector3CurveSmoothSampler.Instance);
                        var curveTranslation = CreateCurve(animTranslation, x => x,
                            Vector3CurveStepSampler.Instance, Vector3CurveLinearSampler.Instance, Vector3CurveSmoothSampler.Instance);
                        var curveRotation = CreateCurve(animRotation, x => x,
                            QuaternionCurveStepSampler.Instance, QuaternionCurveLinearSampler.Instance, QuaternionCurveSmoothSampler.Instance);
                        var curveMorphWeights = CreateCurve(animMorphWeights, x => new ArraySegment<Single>(x),
                            SingleArrayCurveStepSampler.Instance, SingleArrayCurveLinearSampler.Instance, SingleArrayCurveSmoothSampler.Instance);

                        var uvNodeAnimation = new SkinnedModelNodeAnimation(n, curveScale, curveTranslation, curveRotation, curveMorphWeights);
                        uvNodeAnimations.Add(uvNodeAnimation);
                    }
                    else
                    {
                        uvNodeAnimations.Add(null);
                    }
                });

                var uvAnimation = new SkinnedAnimation(animation.Name, uvNodeAnimations);
                uvAnimations.Add(uvAnimation);
            }

            return new SkinnedModel(contentManager.Ultraviolet, scenes, textures, uvAnimations);
        }

        /// <summary>
        /// Traverses all of the nodes in the specified collection of scenes and performs the specified action.
        /// </summary>
        private static void TraverseModelNodes(IEnumerable<ModelScene> scenes, Action<ModelNode> action)
        {
            foreach (var scene in scenes)
            {
                foreach (var node in scene.Nodes)
                    TraverseModelNodes(node, action);
            }
        }

        /// <summary>
        /// Traverses all of the nodes in the specified hierarchy of nodes and performs the specified action.
        /// </summary>
        private static void TraverseModelNodes(ModelNode node, Action<ModelNode> action)
        {
            action(node);

            foreach (var child in node.Children)
                TraverseModelNodes(child, action);
        }

        /// <summary>
        /// Creates a collection of <see cref="CurveKey{TValue}"/> values from the specified glTF keys.
        /// </summary>
        private static IEnumerable<CurveKey<TDstValue>> CreateLinearKeys<TSrcValue, TDstValue>(Func<TSrcValue, TDstValue> converter,
            IEnumerable<(Single Key, TSrcValue Value)> keys)
        {
            return keys.Select(x => new CurveKey<TDstValue>(x.Key, converter(x.Value)));
        }

        /// <summary>
        /// Creates a collection of <see cref="SmoothCurveKey{TValue}"/> values from the specified glTF keys.
        /// </summary>
        private static IEnumerable<SmoothCurveKey<TDstValue>> CreateSmoothKeys<TSrcValue, TDstValue>(Func<TSrcValue, TDstValue> converter,
            IEnumerable<(Single Key, (TSrcValue TangentIn, TSrcValue Value, TSrcValue TangentOut))> keys)
        {
            return keys.Select(x => new SmoothCurveKey<TDstValue>(x.Key, converter(x.Item2.Value), converter(x.Item2.TangentIn), converter(x.Item2.TangentOut)));
        }

        /// <summary>
        /// Creates a <see cref="Curve{TValue}"/> from the specified sampler.
        /// </summary>
        private static Curve<TDstValue> CreateCurve<TSrcValue, TDstValue>(IAnimationSampler<TSrcValue> sampler, Func<TSrcValue, TDstValue> converter,
            ICurveSampler<TDstValue, CurveKey<TDstValue>> stepCurveSampler,
            ICurveSampler<TDstValue, CurveKey<TDstValue>> linearCurveSampler,
            ICurveSampler<TDstValue, SmoothCurveKey<TDstValue>> smoothCurveSampler)
        {
            if (sampler == null)
                return null;

            switch (sampler.InterpolationMode)
            {
                case AnimationInterpolationMode.STEP:
                    return new Curve<TDstValue, CurveKey<TDstValue>>(CurveLoopType.Cycle, CurveLoopType.Cycle, stepCurveSampler, 
                        CreateLinearKeys(converter, sampler.GetLinearKeys()));

                case AnimationInterpolationMode.LINEAR:
                    return new Curve<TDstValue, CurveKey<TDstValue>>(CurveLoopType.Cycle, CurveLoopType.Cycle, linearCurveSampler,
                        CreateLinearKeys(converter, sampler.GetLinearKeys()));

                case AnimationInterpolationMode.CUBICSPLINE:
                    return new Curve<TDstValue, SmoothCurveKey<TDstValue>>(CurveLoopType.Cycle, CurveLoopType.Cycle, smoothCurveSampler, 
                        CreateSmoothKeys(converter, sampler.GetCubicKeys()));

                default:
                    throw new NotSupportedException();
            }
        }
    }
}
