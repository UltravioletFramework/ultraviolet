using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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
            return new UltravioletTestApplication(null);
        }

        /// <summary>
        /// Creates a throwaway Ultraviolet application with no window. The lifetime of this application
        /// must be managed manually by the caller of this method.
        /// </summary>
        /// <returns>The test application that was created.</returns>
        protected static IUltravioletTestApplication GivenAThrowawayUltravioletApplicationWithNoWindow()
        {
            return new UltravioletTestApplication(null, true);
        }

        /// <summary>
        /// Creates an Ultraviolet Framework test application.
        /// </summary>
        /// <returns>The test application that was created.</returns>
        protected IUltravioletTestApplication GivenAnUltravioletApplication()
        {
            if (application != null)
                throw new InvalidOperationException("An application has already been created.");

#if DEBUG
            /* NOTE: We only allow render-to-screen if we're built in DEBUG mode, BUT
             * there is NOT a debugger currently attached. Having an attached debugger
             * causes some kind of issue that prevents the application window from opening,
             * which defeats the purpose of visual debugging. */
            var rtsAttribute = default(RenderToScreenAttribute);
            if (!Debugger.IsAttached)
            {
                rtsAttribute = GetRenderToScreenAttribute();
            }
            application = new UltravioletTestApplication(rtsAttribute);
#else
            application = new UltravioletTestApplication(null);
#endif

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

            application = new UltravioletTestApplication(null, true);

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

        /// <summary>
        /// Gets the test class' <see cref="RenderToScreenAttribute"/>, if it is annotated with one.
        /// </summary>
        /// <returns>The instance of <see cref="RenderToScreenAttribute"/> associated with the test class,
        /// or <c>null</c> if the test class is not annotated with the attribute.</returns>
        private RenderToScreenAttribute GetRenderToScreenAttribute()
        {
            var type = GetType();
            var attr = type.GetCustomAttributes(typeof(RenderToScreenAttribute), true);

            return (RenderToScreenAttribute)attr.SingleOrDefault();
        }

        // State values.
        private UltravioletTestApplication application;
    }
}
