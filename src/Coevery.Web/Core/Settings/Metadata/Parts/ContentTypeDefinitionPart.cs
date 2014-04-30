using System.Collections.Generic;
using Coevery.ContentManagement;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.Core.Common.Utilities;
using Coevery.Core.Settings.Metadata.Records;

namespace Coevery.Core.Settings.Metadata.Parts {
    public class ContentTypeDefinitionPart : ContentPart<ContentTypeDefinitionRecord> {
        public readonly LazyField<SettingsDictionary> _entitySettings = new LazyField<SettingsDictionary>();

        public string Name {
            get { return Record.Name; }
            set { Record.Name = value; }
        }

        public string DisplayName {
            get { return Record.DisplayName; }
            set { Record.DisplayName = value; }
        }

        public bool Customized {
            get { return Record.Customized; }
            set { Record.Customized = value; }
        }

        public SettingsDictionary EntitySettings {
            get { return _entitySettings.Value; }
            set { _entitySettings.Value = value; }
        }

        public IList<ContentTypePartDefinitionRecord> ContentTypePartDefinitionRecords {
            get { return Record.ContentTypePartDefinitionRecords; }
        }
    }
}