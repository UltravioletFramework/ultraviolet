using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Data;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.Content
{
    /// <summary>
    /// Represents an instance of <see cref="DataObjectRegistry{T}"/> which is designed for use with the Ultraviolet Framework.
    /// </summary>
    /// <inheritdoc/>
    [CLSCompliant(false)]
    public abstract class UltravioletDataObjectRegistry<T> : DataObjectRegistry<T> where T : UltravioletDataObject
    {
        /// <summary>
        /// Gets the Ultraviolet context.
        /// </summary>
        public UltravioletContext Ultraviolet =>
            UltravioletContext.DemandCurrent();

        /// <inheritdoc/>
        protected override void OnRegistered()
        {
            UltravioletContext.ContextInvalidated += UltravioletContext_ContextInvalidated;

            base.OnRegistered();
        }

        /// <inheritdoc/>
        protected override void OnUnregistered()
        {
            UltravioletContext.ContextInvalidated -= UltravioletContext_ContextInvalidated;

            base.OnUnregistered();
        }
        
        /// <inheritdoc/>
        [SecuritySafeCritical]
        protected override IEnumerable<T> LoadDefinitionsFromXml(String file)
        {
            Contract.RequireNotEmpty(file, nameof(file));

            var fss = FileSystemService.Create();

            using (var stream = fss.OpenRead(file))
            {
                return ObjectLoader.LoadDefinitions<T>(XDocument.Load(stream), DataElementName, DefaultObjectClass);
            }
        }

        /// <inheritdoc/>
        [SecuritySafeCritical]
        protected override IEnumerable<T> LoadDefinitionsFromJson(String file)
        {
            Contract.RequireNotEmpty(file, nameof(file));

            var fss = FileSystemService.Create();

            using (var stream = fss.OpenRead(file))
            {
                using (var sreader = new StreamReader(stream))
                using (var jreader = new JsonTextReader(sreader))
                {
                    return ObjectLoader.LoadDefinitions<T>(JObject.Load(jreader), DataElementName, DefaultObjectClass);
                }
            }
        }

        /// <summary>
        /// Handles Ultraviolet context invalidation.
        /// </summary>
        private void UltravioletContext_ContextInvalidated(Object sender, EventArgs e)
        {
            Clear();
        }
    }
}