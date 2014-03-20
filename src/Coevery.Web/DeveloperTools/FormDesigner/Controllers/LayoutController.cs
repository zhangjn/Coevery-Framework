using System.Net;
using System.Net.Http;
using System.Web.Http;
using Coevery.ContentManagement.MetaData;

namespace Coevery.DeveloperTools.FormDesigner.Controllers {
    public class LayoutController : ApiController {
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public LayoutController(IContentDefinitionManager contentDefinitionManager) {
            _contentDefinitionManager = contentDefinitionManager;
        }

        // POST api/metadata/field
        public virtual HttpResponseMessage Post(string id, Data data) {
            if (!ModelState.IsValid) {
                return Request.CreateResponse(HttpStatusCode.MethodNotAllowed);
            }

            var contentTypeDefinition = _contentDefinitionManager.GetTypeDefinition(id);
            if (contentTypeDefinition == null) {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            if (contentTypeDefinition.Settings.ContainsKey("Layout")) {
                contentTypeDefinition.Settings["Layout"] = data.Layout;
            }
            else {
                contentTypeDefinition.Settings.Add("Layout", data.Layout);
            }
            _contentDefinitionManager.StoreTypeDefinition(contentTypeDefinition);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }

    public class Data {
        public string Layout { get; set; }
    }
}