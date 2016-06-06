using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.Owin.Cors;
using Newtonsoft.Json;
using Owin;

namespace BondErrorWebService
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.UseCors(CorsOptions.AllowAll);
            var httpConfig = new HttpConfiguration();
            httpConfig.Formatters.JsonFormatter.SerializerSettings.TypeNameHandling = TypeNameHandling.All;
            httpConfig.EnableCors(new EnableCorsAttribute("*", "*", "*") { SupportsCredentials = true });
            httpConfig.MapHttpAttributeRoutes();
            appBuilder.UseWebApi(httpConfig);
        }
    }
}