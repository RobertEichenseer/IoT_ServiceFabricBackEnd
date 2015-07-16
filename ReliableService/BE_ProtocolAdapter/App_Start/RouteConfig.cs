using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolAdapter.App_Start
{
    using System.Web.Http;

    public static class RouteConfig
    {
        public static void RegisterRoutes(HttpRouteCollection routes)
        {
            //Register routes 
            //in this example Attribute base Routing is used
            //routes.MapHttpRoute(
            //        name: "PartialIngest",
            //        routeTemplate: "api/{controller}/{id}",
            //        defaults: new { controller = "PartialIngest", id = RouteParameter.Optional }
            //);

        }
    }
}
