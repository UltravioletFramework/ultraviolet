using System;
using System.Drawing;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TwistedLogik.Ultraviolet.Testing
{
    /// <summary>
    /// Represents a unit test framework which hosts an instance of the Ultraviolet Framework.
    /// This framework is intended primarily for unit tests which test rendering.
    /// </summary>
    [DeploymentItem(@"..\..\..\Dependencies\SDL2\x86\", "x86")]
    [DeploymentItem(@"..\..\..\Dependencies\BASS\x86\", "x86")]
    [DeploymentItem(@"Resources\", "Resources")]
    [DeploymentItem(@"Content\", "Content")]
    [DeploymentItem(@"TwistedLogik.Ultraviolet.BASS.dll")]
    public abstract class UltravioletApplicationTestFramework : UltravioletTestFramework
    {
        /// <summary>
        /// Cleans up after running an Ultraviolet Application test.
        /// </summary>
        [TestCleanup]
        public void UltravioletApplicationTestFrameworkCleanup()
        {
            DestroyUltravioletApplication(application);
            application = null;
        }

        /// <summary>
        /// Destroys the specified test application.
        /// </summary>
        /// <param name="application">The test application to destroy.</param>
        protected static void DestroyUltravioletApplication(IUltravioletTestApplication application)
        {
            try
            {
                if (application != null)
                {
                    application.Dispose();
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
        /// Creates a throwaway ultraviolet application. The lifetime of this application
        /// must be managed manually by the caller of this method.
        /// </summary>
        /// <returns>The test application that was created.</returns>
        protected static IUltravioletTestApplication GivenAThrowawayUltravioletApplication()
        {
            return new UltravioletTestApplication();
        }

        /// <summary>
        /// Creates a throwaway Ultraviolet application with no window. The lifetime of this application
        /// must be managed manually by the caller of this method.
        /// </summary>
        /// <returns>The test application that was created.</returns>
        protected static IUltravioletTestApplication GivenAThrowawayUltravioletApplicationWithNoWindow()
        {
            return new UltravioletTestApplication(true);
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

            application = new UltravioletTestApplication(headless: true);

            return application;
        }

        /// <summary>
        /// Creates an Ultraviolet Framework test application with an Ultraviolet context in service mode.
        /// </summary>
        /// <returns>The test application that was created.</returns>
        protected IUltravioletTestApplication GivenAnUltravioletApplicationInServiceMode()
        {
            if (application != null)
                throw new InvalidOperationException("An application has already been created.");

            application = new UltravioletTestApplication(headless: true, serviceMode: true);

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
