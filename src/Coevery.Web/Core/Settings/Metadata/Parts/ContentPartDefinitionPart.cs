using System.Collections.Generic;
using Coevery.ContentManagement;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.Core.Common.Utilities;
using Coevery.Core.Settings.Metadata.Records;

namespace Coevery.Core.Settings.Metadata.Parts {
    public class ContentPartDefinitionPart : ContentPart<ContentPartDefinitionRecord> {
        public readonly LazyField<IEnumerable<FieldDefinition>> _fieldDefinitions = new LazyField<IEnumerable<FieldDefinition>>();

        public string Name {
            get { return Record.Name; }
            set { Record.Name = value; }
        }

        public IList<ContentPartFieldDefinitionRecord> ContentPartFieldDefinitionRecords {
            get { return Record.ContentPartFieldDefinitionRecords; }
        }

        public IEnumerable<FieldDefinition> FieldDefinitions {
            get { return _fieldDefinitions.Value; }
        }
    }

    public class FieldDefinition {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string FieldType { get; set; }
        public SettingsDictionary Settings { get; set; }
    }
}