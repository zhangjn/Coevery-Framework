using System.Net;
using System.Net.Http;
using System.Web.Http;
using Coevery.ContentManagement;
using Coevery.DeveloperTools.EntityManagement.Services;

namespace Coevery.DeveloperTools.FormDesigner.Controllers {
    public class LayoutController : ApiController {
        private readonly IEntityMetadataService _entityMetadataService;

        public LayoutController(IEntityMetadataService entityMetadataService) {
            _entityMetadataService = entityMetadataService;
        }

        // POST api/metadata/field
        public virtual HttpResponseMessage Post(string id, Data data) {
            if (!ModelState.IsValid) {
                return Request.CreateResponse(HttpStatusCode.MethodNotAllowed);
            }

            var entity = _entityMetadataService.GetEntity(id, VersionOptions.DraftRequired);

            if (entity == null) {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            entity.WithSetting("Layout", data.Layout);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }

    public class Data {
        public string Layout { get; set; }
    }
}