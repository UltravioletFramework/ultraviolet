using System;
using System.Collections;
using System.Collections.Generic;
using TwistedLogik.Nucleus.Collections;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    partial class UIElementCollection
    {
        /// <inheritdoc/>
        public GenericEnumerator<UIElement> GetEnumerator()
        {
            return new GenericEnumerator<UIElement>(visualChildren,
                (Object state, Int32 index, out UIElement result) =>
                {
                    var vc = (VisualCollection)state;
                    if (index >= 0 && index < vc.Count)
                    {
                        result = (UIElement)vc[index];
                        return true;
                    }
                    result = null;
                    return false;
                });
        }

        /// <inheritdoc/>
        IEnumerator<UIElement> IEnumerable<UIElement>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
