using System.Web.Http;

public static class WebApiConfig
{
    public static string UrlPrefix { get { return "api"; } }
    public static string UrlPrefixRelative { get { return "~/api"; } }


    public static void Register(HttpConfiguration config)
    {
        // Web API configuration and services

        // Web API routes
        config.MapHttpAttributeRoutes();

        config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional }
        );
    }
}