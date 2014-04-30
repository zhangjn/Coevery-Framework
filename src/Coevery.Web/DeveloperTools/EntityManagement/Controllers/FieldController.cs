using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Coevery.DeveloperTools.EntityManagement.Services;
using Coevery.Localization;
using Coevery.Utility.Extensions;

namespace Coevery.DeveloperTools.EntityManagement.Controllers {
    public class FieldController : ApiController {
        private readonly IEntityMetadataService _entityMetadataService;

        public FieldController(IEntityMetadataService entityMetadataService) {
            _entityMetadataService = entityMetadataService;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        // GET api/metadata/field
        public object Get(string name, int page, int rows) {
            var fields = _entityMetadataService.GetFields(name)
                .Select(x => new {
                    Name = x.Name,
                    DisplayName = x.DisplayName,
                    FieldType = x.FieldType.CamelFriendly(),
                })
                .ToList();

            var totalRecords = fields.Count();
            return new {
                total = Convert.ToInt32(Math.Ceiling((double) totalRecords/rows)),
                page = page,
                records = totalRecords,
                rows = fields
            };
        }

        // DELETE api/metadata/field/name
        public virtual HttpResponseMessage Delete([FromUri] string[] name, string entityName) {
            var state = true;
            foreach (var n in name) {
                var stateService = _entityMetadataService.DeleteField(n, entityName);
                state = state && stateService;
            }
            return state
                ? Request.CreateResponse(HttpStatusCode.OK)
                : Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid id!");
        }
    }
}