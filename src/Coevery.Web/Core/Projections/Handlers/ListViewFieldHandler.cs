//using System.Linq;
//using Coevery.ContentManagement;
//using Coevery.Core.Common.Extensions;
//using Coevery.Core.Entities.Events;
//using Coevery.Core.Projections.Models;

//namespace Coevery.Core.Projections.Handlers {
//    public class ListViewFieldHandler : IFieldEvents {
//        public ListViewFieldHandler(ICoeveryServices services) {
//            Services = services;
//        }

//        public ICoeveryServices Services { get; private set; }

//        public void OnDeleting(string entityName, string fieldName) {
//            var parts = Services.ContentManager.Query<ListViewPart, ListViewPartRecord>()
//                .Where(r => r.ItemContentType == entityName).List();

//            foreach (var part in parts) {
//                var projectionPart = part.As<ProjectionPart>().Record;
//                var layout = projectionPart.LayoutRecord;
//                string type = string.Format("{0}.{1}.", entityName.ToPartName(), fieldName);
//                var deletedFields = layout.Properties.Where(x => x.Type == type).ToList();
//                deletedFields.ForEach(record => layout.Properties.Remove(record));
//            }
//        }
//    }
//}