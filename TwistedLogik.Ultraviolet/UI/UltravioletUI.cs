using System;
using System.Linq;
using System.Reflection;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.UI
{
    /// <summary>
    /// Represents the core implementation of the Ultraviolet UI subsystem.
    /// </summary>
    public sealed class UltravioletUI : UltravioletResource, IUltravioletUI
    {
        /// <summary>
        /// Initializes a new instance of the UltravioletUI class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="configuration">The Ultraviolet Framework configuration settings for the current context.</param>
        public UltravioletUI(UltravioletContext uv, UltravioletConfiguration configuration)
            : base(uv)
        {
            if (!String.IsNullOrEmpty(configuration.LayoutProviderAssembly))
            {
                SetLayoutProvider(configuration.LayoutProviderAssembly);
            }
            screenStacks = new UIScreenStackCollection(uv);
        }

        /// <summary>
        /// Updates the subsystem's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to Update.</param>
        public void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (layoutProvider != null)
            {
                layoutProvider.Update(time);
            }

            foreach (var stack in screenStacks)
            {
                stack.Update(time);
            }

            OnUpdating(time);
        }

        /// <summary>
        /// Gets the screen stack associated with the primary window.
        /// </summary>
        /// <returns>The screen stack associated with the primary window.</returns>
        public UIScreenStack GetScreens()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var primary = Ultraviolet.GetPlatform().Windows.GetPrimary();
            if (primary == null)
                throw new InvalidOperationException(UltravioletStrings.NoPrimaryWindow);

            return screenStacks[primary];
        }

        /// <summary>
        /// Gets the screen stack associated with the specified window.
        /// </summary>
        /// <param name="window">The window for which to retrieve a screen stack, 
        /// or null to retrieve the screen stack for the primary window.</param>
        /// <returns>The screen stack associated with the specified window.</returns>
        public UIScreenStack GetScreens(IUltravioletWindow window)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return (window == null) ? GetScreens() : screenStacks[window];
        }

        /// <summary>
        /// Sets the assembly which contains the layout provider.
        /// </summary>
        /// <param name="assembly">The name of the assembly that contains the layout provider.</param>
        public void SetLayoutProvider(String assembly)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var asm = String.IsNullOrEmpty(assembly) ? null : Assembly.LoadFrom(assembly);
            SetLayoutProvider(asm);
        }

        /// <summary>
        /// Sets the assembly which contains the layout provider.
        /// </summary>
        /// <param name="assembly">The assembly that contains the layout provider.</param>
        public void SetLayoutProvider(Assembly assembly)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (assembly == null)
            {
                this.layoutProviderAssembly = null;
                this.layoutProvider = null;
            }
            else
            {
                if (assembly.FullName != layoutProviderAssembly)
                {
                    var types = from t in assembly.GetTypes()
                                where t.GetInterfaces().Contains(typeof(IUILayoutProvider))
                                select t;

                    if (!types.Any() || types.Count() > 1)
                        throw new InvalidOperationException(UltravioletStrings.LayoutProviderNotFound);

                    var type = types.Single();
                    var ctor = type.GetConstructor(new Type[] { typeof(UltravioletContext) });
                    if (ctor == null)
                        throw new InvalidOperationException(UltravioletStrings.LayoutProviderInvalidCtor);

                    var instance = (IUILayoutProvider)ctor.Invoke(new Object[] { Ultraviolet });

                    this.layoutProviderAssembly = assembly.FullName;
                    this.layoutProvider = instance;
                }
            }
        }

        /// <summary>
        /// Gets the current layout provider service.
        /// </summary>
        /// <returns>The current layout provider service.</returns>
        public IUILayoutProvider GetLayoutProvider()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return layoutProvider;
        }

        /// <summary>
        /// Occurs when the subsystem is updating its state.
        /// </summary>
        public event UltravioletSubsystemUpdateEventHandler Updating;

        /// <summary>
        /// Releases resources associated with this object.
        /// </summary>
        /// <param name="disposing">true if the object is being disposed; false if the object is being finalized.</param>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing && !Disposed)
            {
                SafeDispose.Dispose(screenStacks);
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Raises the Updating event.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to Update.</param>
        private void OnUpdating(UltravioletTime time)
        {
            var temp = Updating;
            if (temp != null)
            {
                temp(this, time);
            }
        }

        // The current layout provider service.
        private String layoutProviderAssembly;
        private IUILayoutProvider layoutProvider;

        // The collection of screens associated with each window.
        private readonly UIScreenStackCollection screenStacks;
    }
}
