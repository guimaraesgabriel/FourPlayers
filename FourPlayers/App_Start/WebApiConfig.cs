using System.Web.Http;
using System.Web.Http.Cors;

namespace FourPlayers
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var pol = new EnableCorsAttribute(
                origins: "*",
                methods: "*",
                headers: "*"
            );

            config.EnableCors(pol);

            // Web API configuration and services
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("multipart/form-data"));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
