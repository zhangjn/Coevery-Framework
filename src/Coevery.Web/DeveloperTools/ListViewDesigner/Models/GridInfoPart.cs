using Coevery.ContentManagement;

namespace Coevery.DeveloperTools.ListViewDesigner.Models {
    public class GridInfoPart : ContentPart<GridInfoPartRecord> {
        public string DisplayName {
            get { return Record.DisplayName; }
            set { Record.DisplayName = value; }
        }

        public string ItemContentType {
            get { return Record.ItemContentType; }
            set { Record.ItemContentType = value; }
        }
    }
}