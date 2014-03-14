using System.Collections.Generic;
using Coevery.Core.Common.Services;

namespace Coevery.Core.Projections.Services {
    public class JavascriptLocalizationResourceProvider : IAutoLoadResourceProvider {
        public IEnumerable<dynamic> GetResources(dynamic shapeHelper) {
            yield return shapeHelper.Projection_JavascriptLocalizationResource();
        }
    }
}