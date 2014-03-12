using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using Coevery.Environment.ShellBuilders.Models;
using Coevery.Mvc.Routes;

namespace Coevery.DeveloperTools.Entities {
    public class Routes : IRouteProvider {

        private readonly ShellBlueprint _blueprint;

        public Routes(ShellBlueprint blueprint)
        {
            _blueprint = blueprint;
        }

        public IEnumerable<RouteDescriptor> GetRoutes() {
            var displayPathsPerArea = _blueprint.Controllers.GroupBy(
                x => x.AreaName,
                x => x.Feature.Descriptor.Extension);

            yield return new RouteDescriptor {
                Route = new Route(
                    "DevTools",
                    new RouteValueDictionary {
                        {"area", "Entities"},
                        {"controller", "Home"},
                        {"action", "Index"}
                    },
                    new RouteValueDictionary(),
                    new RouteValueDictionary {{"area", "Entities"}},
                    new MvcRouteHandler())
            };

            foreach (var item in displayPathsPerArea)
            {
                var areaName = item.Key;
                var extensionDescriptor = item.Distinct().Single();
                var displayPath = extensionDescriptor.Path;
                SessionStateBehavior defaultSessionState;
                Enum.TryParse(extensionDescriptor.SessionState, true /*ignoreCase*/, out defaultSessionState);

                yield return new RouteDescriptor
                {
                    Priority = -10,
                    SessionState = defaultSessionState,
                    Route = new Route(
                        "DevTools/" + displayPath + "/{action}/{id}",
                        new RouteValueDictionary {
                            {"area", areaName},
                            {"controller", "Admin"},
                            {"action", "index"},
                            {"id", ""}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", areaName}
                        },
                        new MvcRouteHandler())
                };
            }
        }

        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (RouteDescriptor route in GetRoutes()) {
                routes.Add(route);
            }
        }
    }
}