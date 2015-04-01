using System;
using System.Web.Http;
using UvTestRunner.Services;
using UvTestRunner.Models;

namespace UvTestRunner.Controllers
{
    public class TestRunnerController : ApiController
    {
        private readonly TestRunnerService testRunnerService = new TestRunnerService();

        [Route("api/uvtest")]
        public IHttpActionResult Post()
        {
            return Ok(new TestRunCreationResponse() { TestRunID = testRunnerService.Run() });
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
