using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Ultraviolet.Core
{
    /// <summary>
    /// Represents a JSON contract resolver which implements standard rules for Ultraviolet Core object serialization.
    /// </summary>
    public class CoreJsonContractResolver : CamelCasePropertyNamesContractResolver
    {
        /// <inheritdoc/>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);
            if (prop.Writable)
                return prop;

            prop.Writable = (member as PropertyInfo)?.GetSetMethod(true) != null;

            return prop;
        }
    }
}
