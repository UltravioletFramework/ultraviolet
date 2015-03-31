using System;
using System.Web.Http;
using UvTestRunner.Services;

namespace UvTestRunner.Controllers
{
    public class TestRunnerController : ApiController
    {
        private readonly TestRunnerService testRunnerService = new TestRunnerService();

        [Route("api/uvtest")]
        public IHttpActionResult Post()
        {
            return Ok(new { TestID = testRunnerService.Run() });
        }

        [Route("api/uvtest/{id}")]
        public IHttpActionResult Get(Int64 id)
        {
            var run = testRunnerService.GetByID(id);
            if (run == null)
                return NotFound();

            return Ok(new { TestID = id, TestStatus = run.Status });
        }
    }
}
