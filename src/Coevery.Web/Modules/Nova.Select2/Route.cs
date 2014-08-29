using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Coevery.Mvc.Routes;

namespace Nova.Select2 {
    public class Routes : IRouteProvider {
        public IEnumerable<RouteDescriptor> GetRoutes() {
            yield return new RouteDescriptor {
				Priority = -19,
                Route = new Route(
                    "{controller}/{action}/{id}",
                    new RouteValueDictionary {
                        {"area", "Nova.Select2"},
						{"controller", "Lead"},
                        {"action", "Index"},
                        {"id", ""}
                    },
                    new RouteValueDictionary(),
                    new RouteValueDictionary {{"area", "Nova.Select2"}},
                    new MvcRouteHandler())
            };
        }

        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (RouteDescriptor route in GetRoutes()) {
                routes.Add(route);
            }
        }
    }
}