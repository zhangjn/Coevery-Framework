using System.Collections.Generic;

namespace Coevery.Core.Common.Services {
    public interface IAutoLoadResourceProvider : IDependency {
        IEnumerable<dynamic> GetResources(dynamic shapeHelper);
    }
}