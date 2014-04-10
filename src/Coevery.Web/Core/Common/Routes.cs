using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Coevery.Mvc.Routes;

namespace Coevery.Core.Common {
    public class Routes : IRouteProvider {
        public IEnumerable<RouteDescriptor> GetRoutes() {
            return new[] {
                new RouteDescriptor {
                    Priority = -20,
                    Route = new Route("",
                        new RouteValueDictionary {
                            {"area", "Common"},
                            {"controller", "Home"},
                            {"action", "Index"}
                        }, new RouteValueDictionary(),
                        new RouteValueDictionary {{"area", "Common"}},
                        new MvcRouteHandler())
                },
                new RouteDescriptor {
                    Priority = -9999,
                    Route = new Route(
                        "{*path}",
                        new RouteValueDictionary {
                            {"area", "Common"},
                            {"controller", "Error"},
                            {"action", "NotFound"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "Common"}
                        },
                        new MvcRouteHandler())
                }
            };
        }

        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (RouteDescriptor route in GetRoutes()) {
                routes.Add(route);
            }
        }
    }
}