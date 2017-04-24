using System;
using System.IO;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;
using Ultraviolet;
using Ultraviolet.Content;
using Ultraviolet.Input;
using Ultraviolet.Platform;

namespace UltravioletSample
{
#if ANDROID
    public abstract class SampleApplicationBase1 : UltravioletActivity
#else
    public abstract class SampleApplicationBase1 : UltravioletApplication
#endif
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SampleApplicationBase1"/> class.
        /// </summary>
        /// <param name="company">The name of the company that produced the application.</param>
        /// <param name="application">The name of the application </param>
        /// <param name="getInputActions">A function which returns the application's input action collection.</param>
        public SampleApplicationBase1(String company, String application, Func<UltravioletContext, InputActionCollection> getInputActions) 
            : base(company, application)
        {
            this.getInputActions = getInputActions;
        }

        /// <inheritdoc/>
        protected override void OnInitialized()
        {
            SetFileSourceFromManifestIfExists($"{GetType().Namespace}.Content.uvarc");

            base.OnInitialized();
        }

        /// <inheritdoc/>
        protected override void OnLoadingContent()
        {
            LoadInputBindings();

            base.OnLoadingContent();
        }

        /// <inheritdoc/>
        protected override void OnShutdown()
        {
            SaveInputBindings();

            base.OnShutdown();
        }

        /// <summary>
        /// Loads the application's input bindings.
        /// </summary>
        protected virtual void LoadInputBindings()
        {
            var actions = getInputActions?.Invoke(Ultraviolet);
            if (actions != null)
            {
                actions.Load(GetInputBindingsPath(), throwIfNotFound: false);
            }
        }

        /// <summary>
        /// Saves the application's input bindings.
        /// </summary>
        protected virtual void SaveInputBindings()
        {
            var actions = getInputActions?.Invoke(Ultraviolet);
            if (actions != null)
            {
                actions.Save(GetInputBindingsPath());
            }
        }
        
        /// <summary>
        /// Gets the path to the file where the application's input bindings are saved.
        /// </summary>
        /// <returns>The path to the file where the application's input bindings are saved.</returns>
        protected String GetInputBindingsPath()
        {
            return Path.Combine(GetRoamingApplicationSettingsDirectory(), "InputBindings.xml");
        }

        // State values.
        private readonly Func<UltravioletContext, InputActionCollection> getInputActions;
    }
}
