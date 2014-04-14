using Coevery.ContentManagement;

namespace Coevery.Core.Projections.Models {
    public class ListViewPart : ContentPart<ListViewPartRecord> {
        public string Name {
            get { return Record.Name; }
            set { Record.Name = value; }
        }

        public string ItemContentType {
            get { return Record.ItemContentType; }
            set { Record.ItemContentType = value; }
        }

        public bool IsDefault {
            get { return Record.IsDefault; }
            set { Record.IsDefault = value; }
        }
    }
}