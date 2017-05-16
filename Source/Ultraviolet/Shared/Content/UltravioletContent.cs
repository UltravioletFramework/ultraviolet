using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ultraviolet.Core;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents the core implementation of the Ultraviolet content subsystem.
    /// </summary>
    public sealed class UltravioletContent : UltravioletResource, IUltravioletContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UltravioletContent"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public UltravioletContent(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <inheritdoc/>
        public void RegisterImportersAndProcessors(Assembly asm)
        {
            Contract.Require(asm, nameof(asm));

            importers.RegisterAssembly(asm);
            processors.RegisterAssembly(asm);
        }

        /// <inheritdoc/>
        public void RegisterImportersAndProcessors(IEnumerable<Assembly> additionalAssemblies = null)
        {
            Contract.EnsureNot(registered, UltravioletStrings.ContentHandlersAlreadyRegistered);

            var asmCore = typeof(UltravioletContext).Assembly;
            var asmImpl = Ultraviolet.GetType().Assembly;
            var asmEntry = Assembly.GetEntryAssembly();
            var asmShim = Ultraviolet.PlatformCompatibilityShimAssembly;
            var asmViews = Ultraviolet.ViewProviderAssembly;

            var assemblies = new[] { asmCore, asmImpl, asmShim, asmViews, asmEntry }
                .Union(additionalAssemblies ?? Enumerable.Empty<Assembly>()).Where(x => x != null).Distinct();

            foreach (var asm in assemblies)
                RegisterImportersAndProcessors(asm);

            registered = true;
        }

        /// <inheritdoc/>
        public void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            OnUpdating(time);
        }

        /// <inheritdoc/>
        public ContentManifestRegistry Manifests
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return manifests;
            }
        }

        /// <inheritdoc/>
        public ContentImporterRegistry Importers
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);
            
                return importers; 
            }
        }

        /// <inheritdoc/>
        public ContentProcessorRegistry Processors
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);
                
                return processors;
            }
        }

        /// <inheritdoc/>
        public event UltravioletSubsystemUpdateEventHandler Updating;

        /// <summary>
        /// Raises the <see cref="Updating"/> event.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        private void OnUpdating(UltravioletTime time) =>
            Updating?.Invoke(this, time);
        
        // Registered content importers and processors.
        private Boolean registered;
        private readonly ContentManifestRegistry manifests = new ContentManifestRegistry();
        private readonly ContentImporterRegistry importers = new ContentImporterRegistry();
        private readonly ContentProcessorRegistry processors = new ContentProcessorRegistry();
    }
}
