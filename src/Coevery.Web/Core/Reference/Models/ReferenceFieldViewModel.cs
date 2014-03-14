using System.Web.Mvc;
using Coevery.Core.Reference.Fields;

namespace Coevery.Core.Reference.Models {
    public class ReferenceFieldViewModel {
        public ReferenceField Field { get; set; }
        public SelectList ItemList { get; set; }
        public int? ContentId { get; set; }
    }
}