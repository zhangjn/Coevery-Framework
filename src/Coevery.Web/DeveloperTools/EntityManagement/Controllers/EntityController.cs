using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Coevery.ContentManagement;
using Coevery.DeveloperTools.EntityManagement.Services;
using Coevery.Localization;

namespace Coevery.DeveloperTools.EntityManagement.Controllers {
    public class EntityController : ApiController {
        private readonly IEntityMetadataService _entityMetadataService;

        public EntityController(IEntityMetadataService entityMetadataService) {
            _entityMetadataService = entityMetadataService;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        //GET api/Entities/Entity
        public object Get(int rows, int page) {
            var entities = _entityMetadataService.GetEntities()
                .Select(x => new {
                    Id = x.Id,
                    Name = x.Name,
                    DisplayName = x.DisplayName,
                    Modified = !x.IsPublished()
                })
                .ToList();

            var totalRecords = entities.Count();

            return new {
                total = Convert.ToInt32(Math.Ceiling((double) totalRecords/rows)),
                page = page,
                records = totalRecords,
                rows = entities
            };
        }

        // DELETE api/Entities/Entity/:entityName
        public virtual HttpResponseMessage Delete(string name) {
            bool result = _entityMetadataService.DeleteEntity(name);
            return result
                ? Request.CreateResponse(HttpStatusCode.OK)
                : Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}