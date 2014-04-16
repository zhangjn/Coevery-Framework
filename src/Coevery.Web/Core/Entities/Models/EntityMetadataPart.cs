using System.Collections.Generic;
using Coevery.ContentManagement;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.ContentManagement.MetaData.Services;
using Coevery.ContentManagement.Utilities;
using Coevery.Core.Common.Services;

namespace Coevery.Core.Entities.Models {
    public class EntityMetadataPart : ContentPart<EntityMetadataRecord> {

        private readonly ComputedField<SettingsDictionary> _entitySettings = new ComputedField<SettingsDictionary>();

        public ComputedField<SettingsDictionary> EntitySettingsField {
            get { return _entitySettings; }
        }

        public string Name {
            get { return Record.Name; }
            set { Record.Name = value; }
        }

        public string DisplayName {
            get { return Record.DisplayName; }
            set { Record.DisplayName = value; }
        }

        public SettingsDictionary EntitySetting {
            get { return _entitySettings.Value; }
            set { _entitySettings.Value = value; }
        }

        public IList<FieldMetadataRecord> FieldMetadataRecords {
            get { return Record.FieldMetadataRecords; }
        }
    }
}