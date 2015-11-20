using System;
using System.Web.Mvc;
using UvTestViewer.Models;
using UvTestViewer.Services;

namespace UvTestViewer.Controllers
{
    public class RenderingTestController : Controller
    {
        // GET: RenderingTest
        public ActionResult Index(String vendor = null)
        {
            var vendorValue = GpuVendor.Nvidia;
            if (vendor != null)
            {
                switch (vendor)
                {
                    case "intel":
                        vendorValue = GpuVendor.Intel;
                        break;

                    case "nvidia":
                        vendorValue = GpuVendor.Nvidia;
                        break;

                    case "amd":
                        vendorValue = GpuVendor.Amd;
                        break;

                    default:
                        return HttpNotFound("Unrecognized GPU vendor.");
                }
            }

            var service  = new RenderingTestService();
            var overview = service.GetMostRecentRenderingTestOverview(vendorValue);

            return View(overview ?? new RenderingTestOverview() { Vendor = vendorValue });
        }
    }
}