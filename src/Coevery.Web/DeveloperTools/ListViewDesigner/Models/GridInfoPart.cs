using Coevery.ContentManagement;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.ContentManagement.Utilities;

namespace Coevery.DeveloperTools.ListViewDesigner.Models {
    public class GridInfoPart : ContentPart<GridInfoPartRecord> {
        private readonly LazyField<SettingsDictionary> _settings = new LazyField<SettingsDictionary>();

        public LazyField<SettingsDictionary> GridSettingsField {
            get { return _settings; }
        }

        public string DisplayName {
            get { return Record.DisplayName; }
            set { Record.DisplayName = value; }
        }

        public string ItemContentType {
            get { return Record.ItemContentType; }
            set { Record.ItemContentType = value; }
        }

        public bool IsDefault {
            get { return Record.IsDefault; }
            set { Record.IsDefault = value; }
        }

        public SettingsDictionary GridSettings {
            get { return _settings.Value; }
            set { _settings.Value = value; }
        }
    }
}