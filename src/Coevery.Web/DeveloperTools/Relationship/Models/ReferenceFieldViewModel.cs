using System.Web.Mvc;
using Coevery.DeveloperTools.Relationship.Fields;

namespace Coevery.DeveloperTools.Relationship.Models {
    public class ReferenceFieldViewModel {
        public ReferenceField Field { get; set; }
        public SelectList ItemList { get; set; }
        public int? ContentId { get; set; }
    }
}