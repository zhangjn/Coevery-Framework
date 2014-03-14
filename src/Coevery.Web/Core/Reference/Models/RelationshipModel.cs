using System.Web.Mvc;

namespace Coevery.Core.Reference.Models {
    public class RelationshipModel {
        public SelectListItem[] EntityList { get; set; }
        public string Name { get; set; }
        public string PrimaryEntity { get; set; }
        public string RelatedEntity { get; set; }
        public bool IsCreate { get; set; }
    }
}