using System;
using Ultraviolet.TestFramework;

namespace Ultraviolet.TestApplication
{
    /// <summary>
    /// An Ultraviolet application used for unit testing.
    /// </summary>
    public class UltravioletTestApplication : UltravioletApplication, IUltravioletTestApplication
    {
        /// <summary>
        /// Initializes a new instance of the UltravioletTestApplication class.
        /// </summary>
        /// <param name="headless">A value indicating whether to create a headless context.</param>
        /// <param name="serviceMode">A value indicating whether to create a service mode context.</param>
        public UltravioletTestApplication(Boolean headless = false, Boolean serviceMode = false)
            : base("Ultraviolet", "Ultraviolet Unit Tests")
        {
            PreserveApplicationSettings = false;

            this.headless = headless;
            this.serviceMode = serviceMode;
        }

        /// <inheritdoc/>
        protected override UltravioletApplicationAdapter OnCreatingApplicationAdapter()
        {
            this.applicationAdapter = new UltravioletTestApplicationAdapter(this);

            return this.applicationAdapter;
        }

        /// <inheritdoc/>
        public IUltravioletTestApplicationAdapter Adapter => applicationAdapter;

        /// <summary>
        /// 
        /// </summary>
        public Boolean Headless => headless;

        /// <summary>
        /// 
        /// </summary>
        public Boolean ServiceMode => serviceMode;

        // State values.
        private readonly Boolean headless;
        private readonly Boolean serviceMode;

        private UltravioletTestApplicationAdapter applicationAdapter;
    }
}