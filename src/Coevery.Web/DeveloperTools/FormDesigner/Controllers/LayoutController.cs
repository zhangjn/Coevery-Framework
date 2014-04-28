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
                .Query<EntityMetadataPart>(VersionOptions.DraftRequired, "EntityMetadata")
                .List().FirstOrDefault(x => x.Name == id);
            if (entityMetadataPart == null) {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            // computed field trick, the property have to be setted again when something changed.
            var settings = entityMetadataPart.EntitySetting;
            settings["Layout"] = data.Layout;
            entityMetadataPart.EntitySetting = settings;

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }

    public class Data {
        public string Layout { get; set; }
    }
}