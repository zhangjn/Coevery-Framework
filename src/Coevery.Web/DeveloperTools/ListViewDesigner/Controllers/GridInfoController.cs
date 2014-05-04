using System;
using System.Linq;
using System.Web.Http;
using Coevery.ContentManagement;
using Coevery.DeveloperTools.ListViewDesigner.Models;

namespace Coevery.DeveloperTools.ListViewDesigner.Controllers {
    public class GridInfoController : ApiController {
        private readonly IContentManager _contentManager;

        public GridInfoController(IContentManager contentManager) {
            _contentManager = contentManager;
        }

        public object Get(string id, int page, int rows) {
            var query = _contentManager.Query<GridInfoPart, GridInfoPartRecord>(VersionOptions.Latest)
                .Where(v => v.ItemContentType == id)
                .List()
                .Select(record => new {
                    ContentId = record.Id,
                    DisplayName = record.DisplayName,
                    Default = record.IsDefault
                })
                .ToList();

            var totalRecords = query.Count();
            return new {
                total = Convert.ToInt32(Math.Ceiling((double) totalRecords/rows)),
                page = page,
                records = totalRecords,
                rows = query
            };
        }

        public void Post(dynamic viewPart) {
            int id = int.Parse(viewPart.Id.ToString());
            var listView = _contentManager.Get<GridInfoPart>(id, VersionOptions.Latest);

            string typeName = listView.ItemContentType;
            var listViews = _contentManager.Query<GridInfoPart, GridInfoPartRecord>(VersionOptions.Latest)
                .Where(v => v.ItemContentType == typeName)
                .List();

            foreach (var view in listViews) {
                view.IsDefault = false;
            }
            listView.IsDefault = true;
        }

        public void Delete([FromUri] int[] ids) {
            foreach (var id in ids) {
                var contentItem = _contentManager.Get(id, VersionOptions.Latest);
                if (contentItem != null) {
                    _contentManager.Remove(contentItem);
                }
            }
        }
    }
}