using System.Collections.Generic;
using Coevery.Core.Common.Services;

namespace Coevery.Core.Projections.Services {
    public class AutoLoadResourceProvider : IAutoLoadResourceProvider {
        public IEnumerable<dynamic> GetResources(dynamic shapeHelper) {
            yield return shapeHelper.jqGrid_Script();
        }
    }
}