using System;
using System.Linq;
using System.Web.Http;
using Coevery.ContentManagement;
using Coevery.Core.Projections.Models;

namespace Coevery.Core.Projections.Controllers {
    public class ProjectionController : ApiController {
        private readonly IContentManager _contentManager;

        public ProjectionController(
            IContentManager contentManager,
            ICoeveryServices coeveryServices) {
            _contentManager = contentManager;
            Services = coeveryServices;
        }

        public ICoeveryServices Services { get; private set; }

        public object Get(string id, int page, int rows) {
            var query = Services.ContentManager.Query<ListViewPart, ListViewPartRecord>("ListViewPage")
                .Where(v => v.ItemContentType == id).List().Select(record => new {
                    ContentId = record.Id,
                    DisplayName = record.Name,
                    Default = record.IsDefault
                }).ToList();

            var totalRecords = query.Count();
            return new {
                total = Convert.ToInt32(Math.Ceiling((double)totalRecords / rows)),
                page = page,
                records = totalRecords,
                rows = query
            };
        }

        public void Post(dynamic viewPart) {
            int id = int.Parse(viewPart.Id.ToString());
            var listView = Services.ContentManager.Get<ListViewPart>(id);

            var queries = Services.ContentManager.Query<ListViewPart, ListViewPartRecord>("ListViewPage")
                .Where(v => v.ItemContentType == listView.ItemContentType);
            var listViews = queries.List().ToList();
            foreach (var view in listViews) {
                view.IsDefault = false;
            }
            listView.IsDefault = true;
        }

        public void Delete(int id) {
            var contentItem = _contentManager.Get(id);
            _contentManager.Remove(contentItem);
        }
    }
}