using System.Web.Mvc;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.Core.Relationships.Fields;
using Coevery.Core.Settings.Metadata;

namespace Coevery.Core.Relationships.Models {
    public class ReferenceFieldViewModel {
        public ReferenceField Field { get; set; }
        public string SelectedText { get; set; }
        public int? ContentId { get; set; }
        public ContentTypeDefinition RelatedEntityDefinition { get; set; }
    }
}