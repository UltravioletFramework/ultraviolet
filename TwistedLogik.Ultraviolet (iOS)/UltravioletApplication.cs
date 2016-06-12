namespace TwistedLogik.Ultraviolet
{
    partial class UltravioletApplication
    {
        /// <summary>
        /// Loads the application's settings.
        /// </summary>
        partial void LoadSettings()
        {
            if (!PreserveApplicationSettings)
                return;

            // TODO
        }

        /// <summary>
        /// Saves the application's settings.
        /// </summary>
        partial void SaveSettings()
        {
            if (!PreserveApplicationSettings)
                return;

            // TODO
        }

        /// <summary>
        /// Applies the application's settings.
        /// </summary>
        partial void ApplySettings()
        {
            if (this.settings == null)
                return;

            this.settings.Apply(uv);
        }

        // The application's settings.
        private UltravioletApplicationSettings settings;
    }
}