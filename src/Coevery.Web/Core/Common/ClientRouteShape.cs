using System.IO;
using Coevery.DisplayManagement;
using Coevery.DisplayManagement.Descriptors;
using Coevery.Mvc.ClientRoute;

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

        public void Discover(ShapeTableBuilder builder) {}
    }
}