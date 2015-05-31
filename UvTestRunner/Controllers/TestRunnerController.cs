using System;
using System.Threading.Tasks;
using System.Web.Http;
using UvTestRunner.Models;
using UvTestRunner.Services;

namespace UvTestRunner.Controllers
{
    public class TestRunnerController : ApiController
    {
        private readonly TestRunnerService testRunnerService = new TestRunnerService();

        [Route("api/uvtest")]
        public async Task<IHttpActionResult> Post()
        {
            var testRunID = await testRunnerService.Run();
            return Ok(new TestRunCreationResponse() { TestRunID = testRunID });
        }

        [Route("api/uvtest/{id}")]
        public IHttpActionResult Get(Int64 id)
        {
            var run = testRunnerService.GetByID(id);
            if (run == null)
                return NotFound();

            return Ok(new TestRunStatusResponse() { TestRunID = id, TestRunStatus = testRunnerService.GetTestRunStatus(id) });
        }
    }
}
