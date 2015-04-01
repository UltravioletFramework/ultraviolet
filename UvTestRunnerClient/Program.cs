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
            var resultPath = Task.Run(() => Run());
            Console.WriteLine(resultPath.Result);
        }

        /// <summary>
        /// Spawns a new test run, waits for it to complete, and copies the results to the configured output directory.
        /// </summary>
        /// <returns>The path to the output result file.</returns>
        private static async Task<String> Run()
        {
            // Kick off a test run.
            var id = await SpawnNewTestRun();

            // Poll until the test run is complete.
            var status = TestRunStatus.Pending;
            while (status != TestRunStatus.Succeeded && status != TestRunStatus.Failed)
            {
                Task.Delay(1000).Wait();
                status = await QueryTestRunStatus(id);
            }

            // Spit out the result file.
            var resultData = await RetrieveTestResult(id);
            var resultPath = Path.Combine(Settings.Default.OutputPath, Settings.Default.OutputName);
            File.WriteAllBytes(resultPath, resultData);

            return resultPath;
        }

        /// <summary>
        /// Posts a request to the server to spawn a new test run.
        /// </summary>
        /// <returns>The identifier of the test run within the server's database.</returns>
        private static async Task<Int64> SpawnNewTestRun()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Settings.Default.UvTestRunnerUrl);
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
                client.BaseAddress = new Uri(Settings.Default.UvTestRunnerUrl);
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
        /// <param name="id">The identifier of the test run within the server's database.</param>
        /// <returns>The contents of the test result file associated with the specified test run.</returns>
        private static async Task<Byte[]> RetrieveTestResult(Int64 id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Settings.Default.UvTestViewerUrl);

                var response = await client.GetAsync("TestResults/" + id + "/Result.trx");
                if (!response.IsSuccessStatusCode)
                    Environment.Exit(1);

                var data = await response.Content.ReadAsByteArrayAsync();

                return data;
            }
        }
    }
}
