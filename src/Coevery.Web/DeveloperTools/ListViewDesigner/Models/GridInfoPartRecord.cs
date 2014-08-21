using Coevery.ContentManagement.Records;
using Coevery.Data.Conventions;

namespace Coevery.DeveloperTools.ListViewDesigner.Models {
    public class GridInfoPartRecord : ContentPartVersionRecord {
        public virtual string DisplayName { get; set; }
        public virtual string ItemContentType { get; set; }
        public virtual bool IsDefault { get; set; }

        [StringLengthMax]
        public virtual string Settings { get; set; }
    }
}