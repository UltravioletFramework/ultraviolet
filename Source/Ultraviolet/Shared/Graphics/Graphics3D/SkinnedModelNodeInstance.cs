using System;
using System.Linq;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents an animated instance of a <see cref="ModelNode"/>. Each instance represents a particular
    /// skinned animation at a particular point in time.
    /// </summary>
    public class SkinnedModelNodeInstance
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinnedModelNodeInstance"/> class.
        /// </summary>
        /// <param name="template">The <see cref="ModelNode"/> which serves as this instance's template.</param>
        public SkinnedModelNodeInstance(ModelNode template)
        {
            Contract.Require(template, nameof(template));

            this.Template = template;
            this.Children = new SkinnedModelNodeInstanceCollection(template.Children.Select(x => new SkinnedModelNodeInstance(x)));

            if (this.Template.ParentModel is SkinnedModel skinnedModel)
            {
                this.Skin = skinnedModel.Skins.TryGetSkinByNode(template.LogicalIndex);
            }
        }

        /// <summary>
        /// Performs an action on all nodes within this node (including this node).
        /// </summary>
        /// <param name="action">The action to perform on each node.</param>
        /// <param name="state">An arbitrary state object to pass to <paramref name="action"/>.</param>
        public void TraverseNodes(Action<SkinnedModelNodeInstance, Object> action, Object state)
        {
            Contract.Require(action, nameof(action));

            action(this, state);

            foreach (var child in Children)
                child.TraverseNodes(action, state);
        }

        /// <summary>
        /// Resets the animation state of this node instance.
        /// </summary>
        public void ResetAnimationState()
        {
            this.LocalTransform.UpdateFromIdentity();
        }

        /// <summary>
        /// Updates the animation state of this node instance.
        /// </summary>
        /// <param name="animation">The animation from which to sample animated values.</param>
        /// <param name="time">The time within the animation timeline at which to sample values.</param>
        public void UpdateAnimationState(SkinnedModelNodeAnimation animation, Double time)
        {
            Contract.Require(animation, nameof(animation));

            var templatedTransform = Template.Transform;

            var t = (Single)time;
            var animatedTranslation = animation.Translation?.Evaluate(t, default) ?? templatedTransform.Translation;
            var animatedRotation = animation.Rotation?.Evaluate(t, default) ?? templatedTransform.Rotation;
            var animatedScale = animation.Scale?.Evaluate(t, default) ?? templatedTransform.Scale;

            this.LocalTransform.UpdateFromTranslationRotationScale(animatedTranslation, animatedRotation, animatedScale);
        }

        /// <summary>
        /// Gets the node instance's template.
        /// </summary>
        public ModelNode Template { get; }

        /// <summary>
        /// Gets the node's skin, if it has one.
        /// </summary>
        public Skin Skin { get; }

        /// <summary>
        /// Gets the instance's collection of child nodes.
        /// </summary>
        public SkinnedModelNodeInstanceCollection Children { get; }

        /// <summary>
        /// Gets the node instance's local transform.
        /// </summary>
        public AffineTransform LocalTransform { get; } = new AffineTransform();
    }
}
