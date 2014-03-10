﻿using System.IO;
using Coevery.DisplayManagement;
using Coevery.DisplayManagement.Descriptors;
using Coevery.Mvc.ClientRoute;
using Coevery.UI.Navigation;

namespace Coevery.Core.Common {
    public class ClientRouteShape : IShapeTableProvider {
        private readonly IClientRouteTableManager _clientRouteTableManager;

        public ClientRouteShape(IClientRouteTableManager clientRouteTableManager) {
            _clientRouteTableManager = clientRouteTableManager;
        }

        [Shape]
        public void ClientRoute(dynamic Display, dynamic Shape, TextWriter Output) {
            var isFrontEnd = Shape.IsFrontEnd;
            var routes = _clientRouteTableManager.GetRouteTable(isFrontEnd);
            var result = Display.ClientBootstrapScript(IsFrontEnd: isFrontEnd, Routes: routes);
            Output.Write(result);
        }

        [Shape]
        public void NavigationMenuItem(dynamic Display, MenuItem Shape, TextWriter Output) {}

        public void Discover(ShapeTableBuilder builder) {}
    }
}