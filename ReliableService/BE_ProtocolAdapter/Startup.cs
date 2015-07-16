using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolAdapter
{
    using global::ProtocolAdapter.App_Start;
    using global::ProtocolAdapter.WebApi;
    using Owin;
    using System.Web.Http;

    public class Startup : IOwinAppBuilder
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();

            //Enable Attribute Based Routing
            config.MapHttpAttributeRoutes(); 

            FormatterConfig.ConfigureFormatters(config.Formatters);
            RouteConfig.RegisterRoutes(config.Routes);
            
            appBuilder.UseWebApi(config);
        }
    }
}
