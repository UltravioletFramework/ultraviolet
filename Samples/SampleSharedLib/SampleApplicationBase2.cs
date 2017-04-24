using System;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;
using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.Platform;

namespace UltravioletSample
{
    public abstract class SampleApplicationBase2 : SampleApplicationBase1
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SampleApplicationBase2"/> class.
        /// </summary>
        /// <param name="company">The name of the company that produced the application.</param>
        /// <param name="application">The name of the application </param>
        /// <param name="getInputActions">A function which returns the application's input action collection.</param>
        public SampleApplicationBase2(String company, String application, Func<UltravioletContext, InputActionCollection> getInputActions) 
            : base(company, application, getInputActions)
        {

        }

        /// <inheritdoc/>
        protected override void OnInitialized()
        {
            SetFileSourceFromManifestIfExists($"{GetType().Namespace}.Content.uvarc");

            base.OnInitialized();
        }
        
        /// <summary>
        /// Loads the application's content manifests.
        /// </summary>
        /// <param name="content"></param>
        protected virtual void LoadContentManifests(ContentManager content)
        {
            Contract.Require(content, nameof(content));

            var uvContent = Ultraviolet.GetContent();

            var contentManifestFiles = content.GetAssetFilePathsInDirectory("Manifests");
            uvContent.Manifests.Load(contentManifestFiles);
        }

        /// <summary>
        /// Loads the application's localization databases.
        /// </summary>
        protected virtual void LoadLocalizationDatabases(ContentManager content)
        {
            var fss = FileSystemService.Create();
            var databases = content.GetAssetFilePathsInDirectory("Localization", "*.xml");
            foreach (var database in databases)
            {
                using (var stream = fss.OpenRead(database))
                {
                    Localization.Strings.LoadFromStream(stream);
                }
            }
        }
    }
}
