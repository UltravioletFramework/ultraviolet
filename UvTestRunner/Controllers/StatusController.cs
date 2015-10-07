using System.Net;
using System.Net.Http;
using System.Web.Http;
using UvTestRunner.Services;

namespace UvTestRunner.Controllers
{
    public class StatusController : ApiController
    {
        [Route("api/status")]
        public HttpResponseMessage Get()
        {
            switch (TestRunnerService.MostRecentTestRunStatus)
            {
                case Models.TestRunStatus.Succeeded:
                    return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("#00ff00") };

                case Models.TestRunStatus.Failed:
                    return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("#ff0000") };
            }

            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("#ffffff") };
        }
    }
}
