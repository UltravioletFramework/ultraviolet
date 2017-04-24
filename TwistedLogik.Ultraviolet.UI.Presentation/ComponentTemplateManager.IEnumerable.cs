using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Ultraviolet.Presentation
{
    public sealed partial class ComponentTemplateManager : IEnumerable<KeyValuePair<Type, XDocument>>
    {
        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<Type, XDocument>> GetEnumerator()
        {
            var types = Enumerable.Union(defaults.Keys, templates.Keys).Distinct();
            foreach (var type in types)
            {
                yield return new KeyValuePair<Type, XDocument>(type, Get(type));
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
