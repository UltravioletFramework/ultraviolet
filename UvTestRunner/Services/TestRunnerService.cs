using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UvTestRunner.Data;
using UvTestRunner.Models;

namespace UvTestRunner.Services
{
    /// <summary>
    /// Represents a service which can run the test suite and retrieve data about
    /// previous test suite runs.
    /// </summary>
    public class TestRunnerService
    {
        /// <summary>
        /// Gets the test run with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the test run within the database.</param>
        /// <returns>The test run with the specified identifier, or <c>null</c> if no such test run exists.</returns>
        public TestRun GetByID(Int64 id)
        {
            using (var testRunContext = new TestRunContext())
            {
                return testRunContext.TestRuns.Where(x => x.ID == id).SingleOrDefault();
            }
        }

        /// <summary>
        /// Runs the test suite.
        /// </summary>
        /// <returns>The identifier of the test run within the database.</returns>
        public Int64 Run()
        {
            var id = CreateTestRun();

            Console.WriteLine("Starting test run #{0}", id);

            Task.Run(() =>
            {
                // Start by spawning the MSTest process and running the unit test suite.
                UpdateTestRunStatus(id, TestRunStatus.Running);
                var psi = new ProcessStartInfo(Settings.Default.TestHostExecutable, Settings.Default.TestHostArgs)
                {
                    WorkingDirectory = Settings.Default.TestRootDirectory
                };
                var proc = Process.Start(psi);
                proc.WaitForExit();

                // If MSTest exited with an error, log it to the database and bail out.
                if (proc.ExitCode != 0 && proc.ExitCode != 1)
                {
                    UpdateTestRunStatus(id, TestRunStatus.Failed);
                    return;
                }

                // If the tests ran successfully, find the folder that contains the test results.
                /* TODO: The way we do this currently introduces a race condition if the test suite is being run simultaneously
                 * in multiple threads, which shouldn't realistically happen, but this case probably
                 * ought to be handled anyway for robustness. */
                var testResultsRoot = Path.Combine(Settings.Default.TestRootDirectory, "TestResults");
                var testResultsDirs = Directory.GetDirectories(testResultsRoot).Select(x => new DirectoryInfo(x));

                var relevantTestResult = testResultsDirs.OrderByDescending(x => x.CreationTimeUtc).FirstOrDefault();
                if (relevantTestResult == null)
                {
                    UpdateTestRunStatus(id, TestRunStatus.Failed);
                    return;
                }

                // Create a directory to hold this test's artifacts.
                var outputDirectory = Path.Combine(Settings.Default.TestResultDirectory, id.ToString());
                Directory.CreateDirectory(outputDirectory);

                // Copy the TRX file and any outputted PNG files to the artifact directory.
                var trxFileSrc = Path.ChangeExtension(Path.Combine(relevantTestResult.Parent.FullName, relevantTestResult.Name), "trx");
                var trxFileDst = Path.Combine(outputDirectory, "Result.trx");
                File.Copy(trxFileSrc, trxFileDst, true);

                var pngFiles = Directory.GetFiles(Path.Combine(relevantTestResult.FullName, "Out"), "*.png");
                foreach (var pngFile in pngFiles)
                {
                    var pngFileSrc = pngFile;
                    var pngFileDst = Path.Combine(outputDirectory, Path.GetFileName(pngFileSrc));
                    File.Copy(pngFileSrc, pngFileDst, true);
                }
                UpdateTestRunStatus(id, TestRunStatus.Succeeded);
            });

            return id;
        }

        /// <summary>
        /// Creates a new test run and places it into pending status.
        /// </summary>
        /// <returns>The identifier of the test run within the database.</returns>
        public Int64 CreateTestRun()
        {
            using (var testRunContext = new TestRunContext())
            {
                var run = new TestRun() { Status = TestRunStatus.Pending };

                testRunContext.TestRuns.Add(run);
                testRunContext.SaveChanges();

                return run.ID;
            }
        }

        /// <summary>
        /// Gets the status of the specified test run.
        /// </summary>
        /// <param name="id">The identifier of the test run within the database.</param>
        /// <returns>A <see cref="TestRunStatus"/> value specifying the test run's current status.</returns>
        public TestRunStatus GetTestRunStatus(Int64 id)
        {
            using (var testRunContext = new TestRunContext())
            {
                var run = testRunContext.TestRuns.Where(x => x.ID == id).Single();
                return run.Status;
            }
        }

        /// <summary>
        /// Updates the status of the specified test run.
        /// </summary>
        /// <param name="id">The identifier of the test run within the database.</param>
        /// <param name="status">The status to set for the specified test run.</param>
        /// <returns>A <see cref="TestRunStatus"/> value specifying the test run's previous status.</returns>
        public TestRunStatus UpdateTestRunStatus(Int64 id, TestRunStatus status)
        {
            using (var testRunContext = new TestRunContext())
            {
                var run = testRunContext.TestRuns.Where(x => x.ID == id).Single();
                var previousStatus = run.Status;
                run.Status = status;
                testRunContext.SaveChanges();

                return previousStatus;
            }
        }
    }
}
