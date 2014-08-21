using System.Globalization;
using System.Linq;
using Coevery.UI.Resources;

namespace Coevery.Core.Relationships {
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();
            manifest.DefineScript("tagsinput").SetUrl("jquery.tagsinput.js").SetDependencies("jQuery");
            manifest.DefineStyle("tagsinput").SetUrl("jquery.tagsinput.css");
        }
    }
}