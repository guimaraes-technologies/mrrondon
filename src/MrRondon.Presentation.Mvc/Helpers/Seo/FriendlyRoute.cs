using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;

namespace MrRondon.Presentation.Mvc.Helpers.Seo
{
    public class FriendlyRoute : Route
    {
        public FriendlyRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler) : base(url, defaults, routeHandler) { }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var routeData = base.GetRouteData(httpContext);

            if (routeData == null) return null;

            if (routeData.Values.ContainsKey("id")) routeData.Values["id"] = GetIdValue(routeData.Values["id"]);

            return routeData;
        }

        private static object GetIdValue(object id)
        {
            if (id == null) return null;

            var idValue = id.ToString();

            var regex = new Regex(@"^(?<id>\d+).*$");
            var match = regex.Match(idValue);

            return match.Success ? match.Groups["id"].Value : id;
        }
    }
}