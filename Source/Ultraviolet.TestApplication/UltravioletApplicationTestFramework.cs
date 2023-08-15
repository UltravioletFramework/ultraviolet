using System;
using System.IO;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Image;
using Ultraviolet.TestFramework;

namespace Ultraviolet.TestApplication
{
    /// <summary>
    /// Represents a unit test framework which hosts an instance of the Ultraviolet Framework.
    /// This framework is intended primarily for unit tests which test rendering.
    /// </summary>
    public abstract class UltravioletApplicationTestFramework : UltravioletTestFramework
    {
        /// <summary>
        /// Cleans up after running an Ultraviolet Application test.
        /// </summary>
        [TearDown]
        public void UltravioletApplicationTestFrameworkCleanup()
        {
            try
            {
                DestroyUltravioletApplication(application);
                application = null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception while tearing down {TestContext.CurrentContext.Test.MethodName}; " +
                    $"test status was {TestContext.CurrentContext.Result.Outcome.Status}", ex);
            }
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
            catch (Exception e1)
            {
                try
                {
                    var context = (UltravioletContext)typeof(UltravioletContext).GetField("current",
                        BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
                    if (context != null)
                    {
                        context.Dispose();
                    }
                }
                catch (Exception e2)
                {
                    var error = new StringBuilder();
                    error.AppendLine($"An exception occurred while destroying the Ultraviolet application, and test framework failed to perform a clean teardown.");
                    error.AppendLine();
                    error.AppendLine($"Exception which occurred during cleanup:");
                    error.AppendLine();
                    error.AppendLine(e1.ToString());
                    error.AppendLine();
                    error.AppendLine($"Exception which occurred during teardown:");
                    error.AppendLine();
                    error.AppendLine(e2.ToString());

                    try
                    {
                        File.WriteAllText($"uv-test-error-{DateTime.Now:yyyy-MM-dd-HH-mm-ss-fff}.txt", error.ToString());
                    }
                    catch (IOException) { }
                }
                throw;
            }
        }

        /// <summary>
        /// Creates a throwaway ultraviolet application. The lifetime of this application
        /// must be managed manually by the caller of this method.
        /// </summary>
        /// <returns>The test application that was created.</returns>
        protected static IUltravioletTestApplicationAdapter GivenAThrowawayUltravioletApplication()
        {
            return (new UltravioletTestApplication()).Adapter;
        }

        /// <summary>
        /// Creates a throwaway Ultraviolet application with no window. The lifetime of this application
        /// must be managed manually by the caller of this method.
        /// </summary>
        /// <returns>The test application that was created.</returns>
        protected static IUltravioletTestApplicationAdapter GivenAThrowawayUltravioletApplicationWithNoWindow()
        {
            return (new UltravioletTestApplication(true)).Adapter;
        }

        /// <summary>
        /// Creates an Ultraviolet Framework test application.
        /// </summary>
        /// <returns>The test application that was created.</returns>
        protected IUltravioletTestApplicationAdapter GivenAnUltravioletApplication()
        {
            if (application != null)
                throw new InvalidOperationException("An application has already been created.");

            application = new UltravioletTestApplication();

            return application.Adapter;
        }

        /// <summary>
        /// Creates an Ultraviolet Framework test application with a headless Ultraviolet context.
        /// </summary>
        /// <returns>The test application that was created.</returns>
        protected IUltravioletTestApplicationAdapter GivenAnUltravioletApplicationWithNoWindow()
        {
            if (application != null)
                throw new InvalidOperationException("An application has already been created.");

            application = new UltravioletTestApplication(headless: true);

            return application.Adapter;
        }

        /// <summary>
        /// Creates an Ultraviolet Framework test application with an Ultraviolet context in service mode.
        /// </summary>
        /// <returns>The test application that was created.</returns>
        protected IUltravioletTestApplicationAdapter GivenAnUltravioletApplicationInServiceMode()
        {
            if (application != null)
                throw new InvalidOperationException("An application has already been created.");

            application = new UltravioletTestApplication(headless: true, serviceMode: true);

            return application.Adapter;
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="bitmap">The bitmap to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected ImageResult TheResultingImage(UltravioletImage bitmap)
        {
            return new ImageResult(bitmap);
        }

        /// <summary>
        /// Creates a copy of the specified asset which is specific to the machine that is currently
        /// executing the test.
        /// </summary>
        /// <param name="content">The content manager.</param>
        /// <param name="asset">The asset to copy.</param>
        /// <returns>The asset path of the new asset file which was created.</returns>
        protected String CreateMachineSpecificAssetCopy(ContentManager content, String asset)
        {
            Contract.Require(content, nameof(content));
            Contract.Require(asset, nameof(asset));

            var resolvedSourceFile = content.ResolveAssetFilePath(asset);
            var resolvedSourceExtension = Path.GetExtension(resolvedSourceFile);

            var copiedFileName = $"{Path.GetFileNameWithoutExtension(resolvedSourceFile)}-{Environment.MachineName}{resolvedSourceExtension}";
            var copiedFilePath = Path.Combine(Path.GetDirectoryName(resolvedSourceFile), copiedFileName);

            var copiedAssetName = Path.GetFileNameWithoutExtension(copiedFileName);
            var copiedAssetPath = Path.Combine(Path.GetDirectoryName(asset), copiedAssetName);

            File.Copy(resolvedSourceFile, copiedFilePath, true);

            return copiedAssetPath;
        }

        // State values.
        private UltravioletTestApplication application;
    }
}