using System.Web.Mvc;
using Coevery.Core.Relationships.Fields;

namespace Coevery.Core.Relationships.Models {
    public class ReferenceFieldViewModel {
        public ReferenceField Field { get; set; }
        public SelectList ItemList { get; set; }
        public int? ContentId { get; set; }
    }
}