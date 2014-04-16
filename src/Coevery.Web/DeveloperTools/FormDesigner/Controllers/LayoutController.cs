using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Coevery.ContentManagement;
using Coevery.Core.Entities.Models;

namespace Coevery.DeveloperTools.FormDesigner.Controllers {
    public class LayoutController : ApiController {

        public LayoutController(ICoeveryServices coeveryServices) {
            Services = coeveryServices;
        }

        public ICoeveryServices Services { get; private set; }

        // POST api/metadata/field
        public virtual HttpResponseMessage Post(string id, Data data) {
            if (!ModelState.IsValid) {
                return Request.CreateResponse(HttpStatusCode.MethodNotAllowed);
            }

            var entityMetadataPart = Services.ContentManager
                .Query<EntityMetadataPart>(VersionOptions.Latest, "EntityMetadata")
                .List().FirstOrDefault(x => x.Name == id);
            if (entityMetadataPart == null) {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            entityMetadataPart.Settings["Layout"] = data.Layout;

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }

    public class Data {
        public string Layout { get; set; }
    }
}