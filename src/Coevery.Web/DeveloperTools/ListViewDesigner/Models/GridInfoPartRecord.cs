using Coevery.ContentManagement.Records;

namespace Coevery.DeveloperTools.ListViewDesigner.Models {
    public class GridInfoPartRecord : ContentPartVersionRecord {
        public virtual string DisplayName { get; set; }
        public virtual string ItemContentType { get; set; }
    }
}