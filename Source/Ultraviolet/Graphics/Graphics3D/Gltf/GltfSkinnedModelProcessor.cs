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
            var nodesCount = 0;
            var nodesByLogicalIndex = new Dictionary<Int32, ModelNode>();
            var nodesBySkin = new Dictionary<Int32, List<ModelNode>>();
            TraverseModelNodes(scenes, (n, state) =>
            {
                var gltfNode = input.LogicalNodes[n.LogicalIndex];
                if (gltfNode.Skin != null)
                {
                    if (!nodesBySkin.TryGetValue(gltfNode.Skin.LogicalIndex, out var skinNodeList))
                        nodesBySkin[gltfNode.Skin.LogicalIndex] = skinNodeList = new List<ModelNode>();

                    skinNodeList.Add(n);
                }
                nodesByLogicalIndex[n.LogicalIndex] = n;
                nodesCount++;
            });

            var uvSkins = ProcessSkins(input, nodesByLogicalIndex, nodesBySkin);
            var uvAnimations = ProcessAnimations(input, scenes, nodesCount);
            return new SkinnedModel(contentManager.Ultraviolet, scenes, textures, uvSkins, uvAnimations);
        }

        /// <inheritdoc/>
        protected override String DefaultMaterialLoader => typeof(GltfSkinnedMaterialLoader).AssemblyQualifiedName;

        /// <summary>
        /// Traverses all of the nodes in the specified collection of scenes and performs the specified action.
        /// </summary>
        private static void TraverseModelNodes(IEnumerable<ModelScene> scenes, Action<ModelNode, Object> action, Object state = null)
        {
            foreach (var scene in scenes)
                scene.TraverseNodes(action, state);
        }

        /// <summary>
        /// Processes the model's skins.
        /// </summary>
        private static IEnumerable<Skin> ProcessSkins(ModelRoot input, Dictionary<Int32, ModelNode> nodesByLogicalIndex, Dictionary<Int32, List<ModelNode>> nodesBySkin)
        {
            var uvSkins = new List<Skin>(input.LogicalSkins.Count);
            foreach (var skin in input.LogicalSkins)
            {
                var uvSkinJoints = new List<SkinJoint>();

                for (var i = 0; i < skin.JointsCount; i++)
                {
                    var (jointNode, jointIbm) = skin.GetJoint(i);

                    var uvSkinJointNode = nodesByLogicalIndex[jointNode.LogicalIndex];
                    var uvSkinJoint = new SkinJoint(uvSkinJointNode, jointIbm);
                    uvSkinJoints.Add(uvSkinJoint);
                }

                nodesBySkin.TryGetValue(skin.LogicalIndex, out var uvSkinNodes);
                var uvSkin = new Skin(skin.LogicalIndex, skin.Name, uvSkinJoints, uvSkinNodes);
                uvSkins.Add(uvSkin);
            }
            return uvSkins;
        }

        /// <summary>
        /// Processes the node's animations.
        /// </summary>
        private static IEnumerable<SkinnedAnimation> ProcessAnimations(ModelRoot input, IEnumerable<ModelScene> scenes, Int32 nodesCount)
        {
            var uvAnimations = new List<SkinnedAnimation>(input.LogicalAnimations.Count);
            foreach (var animation in input.LogicalAnimations)
            {
                var uvNodeAnimations = new SkinnedModelNodeAnimation[nodesCount];
                TraverseModelNodes(scenes, (n, state) =>
                {
                    var gltfNode = input.LogicalNodes[n.LogicalIndex];

                    var animScale = animation.FindScaleChannel(gltfNode)?.GetScaleSampler();
                    var animTranslation = animation.FindTranslationChannel(gltfNode)?.GetTranslationSampler();
                    var animRotation = animation.FindRotationChannel(gltfNode)?.GetRotationSampler();
                    var animMorphWeights = animation.FindMorphChannel(gltfNode)?.GetMorphSampler();

                    if (animScale != null || animRotation != null || animTranslation != null || animMorphWeights != null)
                    {
                        var curveScale = CreateCurve(animScale, x => x,
                            Vector3CurveStepSampler.Instance, Vector3CurveLinearSampler.Instance, Vector3CurveCubicSplineSampler.Instance);
                        var curveTranslation = CreateCurve(animTranslation, x => x,
                            Vector3CurveStepSampler.Instance, Vector3CurveLinearSampler.Instance, Vector3CurveCubicSplineSampler.Instance);
                        var curveRotation = CreateCurve(animRotation, x => x,
                            QuaternionCurveStepSampler.Instance, QuaternionCurveLinearSampler.Instance, QuaternionCurveCubicSplineSampler.Instance);
                        var curveMorphWeights = CreateCurve(animMorphWeights, x => new ArraySegment<Single>(x),
                            SingleArrayCurveStepSampler.Instance, SingleArrayCurveLinearSampler.Instance, SingleArrayCurveCubicSplineSampler.Instance);

                        var uvNodeAnimation = new SkinnedModelNodeAnimation(n, curveScale, curveTranslation, curveRotation, curveMorphWeights);
                        uvNodeAnimations[gltfNode.LogicalIndex] = uvNodeAnimation;
                    }
                });

                var uvAnimation = new SkinnedAnimation(animation.Name, uvNodeAnimations);
                uvAnimations.Add(uvAnimation);
            }
            return uvAnimations;
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
        /// Creates a collection of <see cref="CubicSplineCurveKey{TValue}"/> values from the specified glTF keys.
        /// </summary>
        private static IEnumerable<CubicSplineCurveKey<TDstValue>> CreateCubicSplineKeys<TSrcValue, TDstValue>(Func<TSrcValue, TDstValue> converter,
            IEnumerable<(Single Key, (TSrcValue TangentIn, TSrcValue Value, TSrcValue TangentOut))> keys)
        {
            return keys.Select(x => new CubicSplineCurveKey<TDstValue>(x.Key, converter(x.Item2.Value), converter(x.Item2.TangentIn), converter(x.Item2.TangentOut)));
        }

        /// <summary>
        /// Creates a <see cref="Curve{TValue}"/> from the specified sampler.
        /// </summary>
        private static Curve<TDstValue> CreateCurve<TSrcValue, TDstValue>(IAnimationSampler<TSrcValue> sampler, Func<TSrcValue, TDstValue> converter,
            ICurveSampler<TDstValue, CurveKey<TDstValue>> stepCurveSampler,
            ICurveSampler<TDstValue, CurveKey<TDstValue>> linearCurveSampler,
            ICurveSampler<TDstValue, CubicSplineCurveKey<TDstValue>> cubicSplineCurveSampler)
        {
            if (sampler == null)
                return null;

            switch (sampler.InterpolationMode)
            {
                case AnimationInterpolationMode.STEP:
                    return new Curve<TDstValue, CurveKey<TDstValue>>(CurveLoopType.Constant, CurveLoopType.Constant, stepCurveSampler, 
                        CreateLinearKeys(converter, sampler.GetLinearKeys()));

                case AnimationInterpolationMode.LINEAR:
                    return new Curve<TDstValue, CurveKey<TDstValue>>(CurveLoopType.Constant, CurveLoopType.Constant, linearCurveSampler,
                        CreateLinearKeys(converter, sampler.GetLinearKeys()));

                case AnimationInterpolationMode.CUBICSPLINE:
                    return new Curve<TDstValue, CubicSplineCurveKey<TDstValue>>(CurveLoopType.Constant, CurveLoopType.Constant, cubicSplineCurveSampler, 
                        CreateCubicSplineKeys(converter, sampler.GetCubicKeys()));

                default:
                    throw new NotSupportedException();
            }
        }
    }
}
