using System.Web.Http;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Owin;

namespace DotNetBay.SelfHost
{
    /// <summary>
    ///  Minimal Configuration to host the application on a SelfHost
    /// </summary>
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure Web API for self-host. 
            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();

            config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            app.UseWebApi(config); 
        }
    }
}
