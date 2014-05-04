using System.Collections.Generic;
using Coevery.ContentManagement.Records;
using Coevery.Data.Conventions;

namespace Coevery.Core.Settings.Metadata.Records {
    public class ContentPartDefinitionRecord : ContentPartVersionRecord {
        public ContentPartDefinitionRecord() {
            ContentPartFieldDefinitionRecords = new List<ContentPartFieldDefinitionRecord>();
        }

        public virtual string Name { get; set; }

        [StringLengthMax]
        public virtual string Settings { get; set; }

        [CascadeAllDeleteOrphan]
        public virtual IList<ContentPartFieldDefinitionRecord> ContentPartFieldDefinitionRecords { get; set; }
    }
}