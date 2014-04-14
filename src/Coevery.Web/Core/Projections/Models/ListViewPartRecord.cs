using Coevery.ContentManagement.Records;

namespace Coevery.Core.Projections.Models {
    public class ListViewPartRecord : ContentPartRecord {
        public virtual string Name { get; set; }
        public virtual string ItemContentType { get; set; }
        public virtual bool IsDefault { get; set; }
    }
}