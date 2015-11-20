using System.Net.Http.Headers;
using System.Web.Http;
using Owin;

namespace UvTestRunner
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            config.MapHttpAttributeRoutes();

            appBuilder.UseWebApi(config);
        }
    }
}
