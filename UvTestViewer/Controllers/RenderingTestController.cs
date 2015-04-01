using System.Web.Mvc;
using UvTestViewer.Models;
using UvTestViewer.Services;

namespace UvTestViewer.Controllers
{
    public class RenderingTestController : Controller
    {
        // GET: RenderingTest
        public ActionResult Index()
        {
            var service  = new RenderingTestService();
            var overview = service.GetMostRecentRenderingTestOverview();

            return View(overview ?? new RenderingTestOverview());
        }
    }
}