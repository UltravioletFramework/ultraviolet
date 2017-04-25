using System;
using System.Collections;
using System.Collections.Generic;
using Ultraviolet.Core.Collections;
using Ultraviolet.Presentation.Media;

namespace Ultraviolet.Presentation.Controls
{
    partial class UIElementCollection
    {
        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>A <see cref="List{T}.Enumerator"/> that iterates through the collection.</returns>
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
