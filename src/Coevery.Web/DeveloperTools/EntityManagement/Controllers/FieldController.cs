using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.ContentManagement.MetaData.Services;
using Coevery.Core.Common.Services;
using Coevery.Core.Fields.Settings;
using Coevery.DeveloperTools.EntityManagement.Services;
using Coevery.Localization;
using Coevery.Utility.Extensions;

namespace Coevery.DeveloperTools.EntityManagement.Controllers {
    public class FieldController : ApiController {
        private readonly IContentMetadataService _contentMetadataService;
        private readonly ISettingsFormatter _settingsFormatter;

        public FieldController(
            IContentMetadataService contentMetadataService, 
            ISettingsFormatter settingsFormatter) {
            _contentMetadataService = contentMetadataService;
            _settingsFormatter = settingsFormatter;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        // GET api/metadata/field
        public object Get(int name, int page, int rows) {
            var metadataTypes = _contentMetadataService.GetFieldsList(name);

            var query = from field in metadataTypes
                let fieldType = field.ContentFieldDefinitionRecord.Name
                let setting = _settingsFormatter.Parse(field.Settings)
                select new {
                    field.Name,
                    field.Id,
                    DisplayName = setting[ContentPartFieldDefinition.DisplayNameKey],
                    FieldType = fieldType.CamelFriendly(),
                    //Type = setting.GetModel<FieldSettings>(fieldType + "Settings").IsSystemField
                    //    ? "System Field" : "User Field",
                    ControllField = string.Empty
                };

            var totalRecords = query.Count();
            return new {
                total = Convert.ToInt32(Math.Ceiling((double) totalRecords/rows)),
                page = page,
                records = totalRecords,
                rows = query
            };
        }

        // DELETE api/metadata/field/name
        public virtual HttpResponseMessage Delete([FromUri] string[] name, string entityName) {
            var state = true;
            foreach (var n in name) {
                var stateService = _contentMetadataService.DeleteField(n, entityName);
                state = state && stateService;
            }
            return state
                ? Request.CreateResponse(HttpStatusCode.OK)
                : Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid id!");
        }
    }
}