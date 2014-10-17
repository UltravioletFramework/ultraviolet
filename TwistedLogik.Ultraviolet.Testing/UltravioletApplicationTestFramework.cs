using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace TwistedLogik.Ultraviolet.Testing
{
    /// <summary>
    /// Represents a unit test framework which hosts an instance of the Ultraviolet Framework.
    /// This framework is intended primarily for unit tests which test rendering.
    /// </summary>
    [DeploymentItem(@"TwistedLogik.Ultraviolet.BASS.dll")]
    [DeploymentItem(@"TwistedLogik.Ultraviolet.FMOD.dll")]
    public abstract class UltravioletApplicationTestFramework : UltravioletTestFramework
    {
        /// <summary>
        /// Cleans up after running an Ultraviolet Application test.
        /// </summary>
        [TestCleanup]
        public void UltravioletApplicationTestFrameworkCleanup()
        {
            try
            {
                if (application != null)
                {
                    application.Dispose();
                    application = null;
                }
            }
            catch
            {
                var context = (UltravioletContext)typeof(UltravioletContext).GetField("current", 
                    BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
                if (context != null)
                {
                    context.Dispose();
                }
                throw;
            }
        }

        /// <summary>
        /// Creates an Ultraviolet Framework test application.
        /// </summary>
        /// <returns>The test application that was created.</returns>
        protected IUltravioletTestApplication GivenAnUltravioletApplication()
        {
            if (application != null)
                throw new InvalidOperationException("An application has already been created.");
            
            application = new UltravioletTestApplication();
            
            return application;
        }

        /// <summary>
        /// Creates an Ultraviolet Framework test application with a headless Ultraviolet context.
        /// </summary>
        /// <returns>The test application that was created.</returns>
        protected IUltravioletTestApplication GivenAnUltravioletApplicationWithNoWindow()
        {
            if (application != null)
                throw new InvalidOperationException("An application has already been created.");

            application = new UltravioletTestApplication(true);

            return application;
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected BitmapResult TheResultingImage(Bitmap bitmap)
        {
            return new BitmapResult(bitmap);
        }

        // State values.
        private UltravioletTestApplication application;
    }
}
