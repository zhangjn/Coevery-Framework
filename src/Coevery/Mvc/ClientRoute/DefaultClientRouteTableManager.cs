﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Autofac.Features.Metadata;
using Coevery.Caching;
using Coevery.Environment.Extensions.Models;
using Coevery.Logging;
using Coevery.Utility.Extensions;

namespace Coevery.Mvc.ClientRoute {
    public class DefaultClientRouteTableManager : IClientRouteTableManager {
        private readonly IEnumerable<Meta<IClientRouteProvider>> _clientRouteProviders;
        private readonly IParallelCacheContext _parallelCacheContext;

        public DefaultClientRouteTableManager(
            IEnumerable<Meta<IClientRouteProvider>> clientRouteProviders,
            IParallelCacheContext parallelCacheContext) {
            _clientRouteProviders = clientRouteProviders;
            _parallelCacheContext = parallelCacheContext;
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        public IEnumerable<ClientRouteDescriptor> GetRouteTable(bool isFrontEnd) {
            Logger.Information("Start building shape table");

            var alterationSets = _parallelCacheContext.RunInParallel(
                _clientRouteProviders.Where(provider => provider.Value.IsFrontEnd == isFrontEnd), provider => {
                    var feature = provider.Metadata.ContainsKey("Feature") ? (Feature) provider.Metadata["Feature"] : null;

                    var builder = new ClientRouteTableBuilder(feature);
                    provider.Value.Discover(builder);
                    return builder.BuildAlterations().ToReadOnlyCollection();
                });

            var alterations = alterationSets
                .SelectMany(shapeAlterations => shapeAlterations)
                .ToList();
            var distinctRouteNames = alterations.GroupBy(item => item.RouteName, StringComparer.OrdinalIgnoreCase)
                .Select(item => item.Key).ToList();

            var routes = GenerateRoutes(distinctRouteNames, alterations);
            Logger.Information("Done building shape table");
            return routes;
        }

        private IEnumerable<ClientRouteDescriptor> GenerateRoutes(List<string> distinctRouteNames, List<ClientRouteAlteration> alterations) {
            var routes = new List<ClientRouteDescriptor>();
            foreach (var routeName in distinctRouteNames) {
                var descriptor = new ClientRouteDescriptor {RouteName = routeName};
                foreach (var alteration in alterations.Where(item => item.RouteName == routeName)) {
                    alteration.Alter(descriptor);
                }
                routes.Add(descriptor);
            }

            routes = routes.OrderBy(item => new Regex(@"\.").Matches(item.RouteName).Count).ThenByDescending(item => item.Priority).ToList();
            return routes;
        }
    }
}