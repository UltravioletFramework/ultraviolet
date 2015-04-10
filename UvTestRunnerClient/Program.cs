using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UvTestRunner.Models;

namespace UvTestRunnerClient
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var succeeded = Task.Run(() => 
            {
                var task1 = Run("Intel", 
                    Settings.Default.UvTestRunnerUrlIntel, Settings.Default.OutputPath, Settings.Default.OutputNameIntel);
                var task2 = Run("Nvidia", 
                    Settings.Default.UvTestRunnerUrlNvidia, Settings.Default.OutputPath, Settings.Default.OutputNameNvidia);
                var task3 = Run("Amd", 
                    Settings.Default.UvTestRunnerUrlAmd, Settings.Default.OutputPath, Settings.Default.OutputNameAmd);

                Task.WaitAll(task1, task2, task3);

                return true;
            });
            Console.WriteLine(succeeded.Result);
        }

        /// <summary>
        /// Spawns a new test run, waits for it to complete, and copies the results to the configured output directory.
        /// </summary>
        /// <param name="vendor">The name of the GPU vendor for which to run rendering tests.</param>
        /// <param name="testRunnerUrl">The URL of the test runner server.</param>
        /// <param name="outputPath">The path to which to output the result file.</param>
        /// <param name="outputName">The name to give to the result file.</param>
        /// <returns>The path to the output result file.</returns>
        private static async Task<String> Run(String vendor, String testRunnerUrl, String outputPath, String outputName)
        {
            if (String.IsNullOrEmpty(testRunnerUrl))
                return null;

            // Kick off a test run.
            var id = await SpawnNewTestRun(testRunnerUrl);

            // Poll until the test run is complete.
            var status = TestRunStatus.Pending;
            while (status != TestRunStatus.Succeeded && status != TestRunStatus.Failed)
            {
                await Task.Delay(1000);
                status = await QueryTestRunStatus(id);
            }

            // Spit out the result file.
            var resultData = await RetrieveTestResult(vendor, id);
            var resultPath = Path.Combine(outputPath, outputName);
            File.WriteAllBytes(resultPath, resultData);

            return resultPath;
        }

        /// <summary>
        /// Posts a request to the server to spawn a new test run.
        /// </summary>
        /// <returns>The identifier of the test run within the server's database.</returns>
        private static async Task<Int64> SpawnNewTestRun(String testRunnerUrl)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(testRunnerUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsync("Api/UvTest", new StringContent(String.Empty));
                if (!response.IsSuccessStatusCode)
                    Environment.Exit(1);

                var responseObject = await response.Content.ReadAsAsync<TestRunCreationResponse>();

                return responseObject.TestRunID;
            }
        }

        /// <summary>
        /// Retrieves the status of the specified test run from the server.
        /// </summary>
        /// <param name="id">The identifier of the test run within the server's database.</param>
        /// <returns>The current status of the specified test run.</returns>
        private static async Task<TestRunStatus> QueryTestRunStatus(Int64 id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Settings.Default.UvTestRunnerUrlNvidia);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync("Api/UvTest/" + id.ToString());
                if (!response.IsSuccessStatusCode)
                    Environment.Exit(1);

                var responseObject = await response.Content.ReadAsAsync<TestRunStatusResponse>();

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
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Settings.Default.UvTestViewerUrl);

                var request  = Path.Combine("TestResults/" + vendor + "/" + id + "/Result.trx");
                var response = await client.GetAsync(request);

                if (!response.IsSuccessStatusCode)
                    Environment.Exit(1);

                var data = await response.Content.ReadAsByteArrayAsync();

                return data;
            }
        }
    }
}
