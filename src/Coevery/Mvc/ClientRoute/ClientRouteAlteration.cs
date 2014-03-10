﻿using System;
using System.Collections.Generic;
using Coevery.Environment.Extensions.Models;

namespace Coevery.Mvc.ClientRoute {
    public class ClientRouteAlteration {
        private readonly IList<Action<ClientRouteDescriptor>> _configurations;

        public ClientRouteAlteration(string routeName, Feature feature, IList<Action<ClientRouteDescriptor>> configurations) {
            _configurations = configurations;
            RouteName = routeName;
            Feature = feature;
        }

        public string RouteName { get; private set; }
        public Feature Feature { get; private set; }

        public void Alter(ClientRouteDescriptor descriptor) {
            foreach (var configuration in _configurations) {
                configuration(descriptor);
            }
        }
    }
}