using System.Collections.Generic;

namespace Coevery.DeveloperTools.Projections.Services {
    public class JavascriptLocalizationResourceProvider : IAutoLoadResourceProvider {
        public IEnumerable<dynamic> GetResources(dynamic shapeHelper) {
            yield return shapeHelper.Projection_JavascriptLocalizationResource();
        }
    }
}