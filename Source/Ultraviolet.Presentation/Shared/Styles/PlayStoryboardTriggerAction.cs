using System;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents a trigger action which plays the specified storyboard.
    /// </summary>
    public sealed class PlayStoryboardTriggerAction : TriggerAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayStoryboardTriggerAction"/> class.
        /// </summary>
        /// <param name="storyboardName">The name of the storyboard to play.</param>
        /// <param name="selector">A selector specifying which elements will be targeted by the storyboard.</param>
        internal PlayStoryboardTriggerAction(String storyboardName, UvssSelector selector)
        {
            this.storyboardName = storyboardName;
            this.selector       = selector;
        }

        /// <inheritdoc/>
        public override void Activate(UltravioletContext uv, DependencyObject dobj)
        {
            var element = dobj as UIElement;
            if (element == null || element.View == null)
                return;

            if (selector == null)
            {
                var storyboard = element.View.FindStoryboard(storyboardName);
                if (storyboard == null)
                    return;

                storyboard.Begin(element);
            }
            else
            {
                var rooted = selector.PartCount == 0 ? false :
                    String.Equals(selector[0].PseudoClass, "trigger-root", StringComparison.InvariantCultureIgnoreCase);
                var target = rooted ? dobj as UIElement : null;

                element.View.Select(target, selector, this, (e, s) =>
                {
                    var action = (PlayStoryboardTriggerAction)s;

                    var storyboard = e.View.FindStoryboard(action.storyboardName);
                    if (storyboard != null)
                    {
                        storyboard.Begin(e);
                    }
                });
            }
            base.Activate(uv, dobj);
        }

        /// <inheritdoc/>
        public override void Deactivate(UltravioletContext uv, DependencyObject dobj)
        {
            var element = dobj as UIElement;
            if (element == null || element.View == null)
                return;

            if (selector == null)
            {
                var storyboard = element.View.FindStoryboard(storyboardName);
                if (storyboard == null)
                    return;

                storyboard.Stop(element);
            }
            else
            {
                var rooted = selector.PartCount == 0 ? false :
                    String.Equals(selector[0].PseudoClass, "trigger-root", StringComparison.InvariantCultureIgnoreCase);
                var target = rooted ? dobj as UIElement : null;

                element.View.Select(target, selector, this, (e, s) =>
                {
                    var action = (PlayStoryboardTriggerAction)s;

                    var storyboard = e.View.FindStoryboard(action.storyboardName);
                    if (storyboard != null)
                    {
                        storyboard.Stop(e);
                    }
                });
            }
            base.Deactivate(uv, dobj);
        }

        // State values.
        private readonly String storyboardName;
        private readonly UvssSelector selector;
    }
}