using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UvTestRunner.Models;

namespace UvTestRunnerClient
{
    public class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length < 1)
                ExitWithError(1, "Unable to execute tests. Missing value for ${bamboo.agentWorkingDirectory}.");
            if (args.Length < 2)
                ExitWithError(2, "Unable to execute tests. Missing value for ${bamboo.build.working.directory}.");

            var agentWorkingDirectory = args[0];
            var buildWorkingDirectory = args[1];

            var succeeded = Task.Run(() =>
            {
                var task1 = Run("Intel", Settings.Default.UvTestRunnerUrlIntel,
                    agentWorkingDirectory, buildWorkingDirectory, Settings.Default.OutputNameIntel);
                var task2 = Run("Nvidia", Settings.Default.UvTestRunnerUrlNvidia,
                    agentWorkingDirectory, buildWorkingDirectory, Settings.Default.OutputNameNvidia);
                var task3 = Run("Amd", Settings.Default.UvTestRunnerUrlAmd,
                    agentWorkingDirectory, buildWorkingDirectory, Settings.Default.OutputNameAmd);

                Task.WaitAll(task1, task2, task3);

                return true;
            });
            Console.WriteLine(succeeded.Result);
        }

        /// <summary>
        /// Terminates the program and displays the specified error message.
        /// </summary>
        /// <param name="exitCode">The application's exit code.</param>
        /// <param name="message">The error message to display to the console.</param>
        private static void ExitWithError(Int32 exitCode, String message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);

            Environment.Exit(exitCode);
        }

        /// <summary>
        /// Spawns a new test run, waits for it to complete, and copies the results to the configured output directory.
        /// </summary>
        /// <param name="vendor">The name of the GPU vendor for which to run rendering tests.</param>
        /// <param name="testRunnerUrl">The URL of the test runner server.</param>
        /// <param name="agentWorkingDirectory">The working directory for the current build agent.</param>
        /// <param name="buildWorkingDirectory">The working directory for the current build.</param>
        /// <param name="outputName">The name to give to the result file.</param>
        /// <returns>The path to the output result file.</returns>
        private static async Task<String> Run(String vendor, String testRunnerUrl, String agentWorkingDirectory, String buildWorkingDirectory, String outputName)
        {
            if (String.IsNullOrEmpty(testRunnerUrl))
                return null;

            try
            {
                // Kick off a test run.
                var id = await SpawnNewTestRun(testRunnerUrl, agentWorkingDirectory, buildWorkingDirectory);

                // Poll until the test run is complete.
                var status = TestRunStatus.Pending;
                while (status != TestRunStatus.Succeeded && status != TestRunStatus.Failed)
                {
                    await Task.Delay(1000);
                    status = await QueryTestRunStatus(testRunnerUrl, id);
                }

                // Spit out the result file.
                var resultData = await RetrieveTestResult(vendor, id);
                var resultPath = Path.Combine(buildWorkingDirectory, outputName);
                File.WriteAllBytes(resultPath, resultData);

                return resultPath;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Posts a request to the server to spawn a new test run.
        /// </summary>
        /// <param name="testRunnerUrl">The URL of the test runner server.</param>
        /// <param name="agentWorkingDirectory">The working directory for the current build agent.</param>
        /// <param name="buildWorkingDirectory">The working directory for the current build.</param>
        /// <returns>The identifier of the test run within the server's database.</returns>
        private static async Task<Int64> SpawnNewTestRun(String testRunnerUrl, String agentWorkingDirectory, String buildWorkingDirectory)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(15);
                client.BaseAddress = new Uri(testRunnerUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                var dirRelative =
                    new Uri(AddTrailingSlashToPath(agentWorkingDirectory)).MakeRelativeUri(
                    new Uri(AddTrailingSlashToPath(buildWorkingDirectory)));
                
                var response = await client.PostAsync("Api/UvTest", new StringContent("\"" + dirRelative + "\"", Encoding.UTF8, "application/json"));
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Failed to POST to test server at {0}: {1} {2}.", testRunnerUrl, (Int32)response.StatusCode, response.ReasonPhrase);
                    Environment.Exit(1);
                }

                var responseObject = await response.Content.ReadAsAsync<TestRunCreationResponse>();

                return responseObject.TestRunID;
            }
        }

        /// <summary>
        /// Retrieves the status of the specified test run from the server.
        /// </summary>
        /// <param name="testRunnerUrl">The URL of the test runner server.</param>
        /// <param name="id">The identifier of the test run within the server's database.</param>
        /// <returns>The current status of the specified test run.</returns>
        private static async Task<TestRunStatus> QueryTestRunStatus(String testRunnerUrl, Int64 id)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(15);
                client.BaseAddress = new Uri(testRunnerUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync("Api/UvTest/" + id.ToString());
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Failed to GET from test server at {0}: {1} {2}.", testRunnerUrl, (Int32)response.StatusCode, response.ReasonPhrase);
                    Environment.Exit(1);
                }

                var responseObject = await response.Content.ReadAsAsync<TestRunStatusResponse>();
                Console.WriteLine("Received response from {0}: {1} / {2}", testRunnerUrl, responseObject.TestRunID, responseObject.TestRunStatus);

                return responseObject.TestRunStatus;
            }
        }

        /// <summary>
        /// Retrieves the test result file associated with the specified test run.
        /// </summary>
        /// <param name="vendor">The vendor for which to retrieve test results.</param>
        /// <param name="id">The identifier of the test run within the server's database.</param>
        /// <returns>The contents of the test result file associated with the specified test run.</returns>
        private static async Task<Byte[]> RetrieveTestResult(String vendor, Int64 id)
        {
            Console.WriteLine("Retreiving test result for {0}...", vendor);

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(15);
                client.BaseAddress = new Uri(Settings.Default.UvTestViewerUrl);

                var request  = Path.Combine("TestResults/" + vendor + "/" + id + "/Result.trx");
                var response = await client.GetAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Failed to retrieve {0} test results: {1} {2}.", vendor, (Int32)response.StatusCode, response.ReasonPhrase);

                    var error = await response.Content.ReadAsStringAsync();
                    File.WriteAllText(String.Format("Error_{0}_{1:yyyy_MM_dd_HH_mm_ss}.html", vendor, DateTime.Now), error);

                    Environment.Exit(1);
                }

                var data = await response.Content.ReadAsByteArrayAsync();
                Console.WriteLine("Received {0} bytes from {1}", data.Length, vendor);

                return data;
            }
        }

        /// <summary>
        /// Adds a trailing slash to the specified path, if it doesn't already have one.
        /// </summary>
        /// <param name="path">The path to which a trailing slash will be added.</param>
        /// <returns>The specified path with a trailing slash added, if it didn't already have one.</returns>
        private static String AddTrailingSlashToPath(String path)
        {
            var separator1 = Path.DirectorySeparatorChar.ToString();
            var separator2 = Path.AltDirectorySeparatorChar.ToString();

            if (path.EndsWith(separator1) || path.EndsWith(separator2))
                return path;

            if (path.Contains(separator2))
                return path + separator2;

            return path + separator1;
        }
    }
}
