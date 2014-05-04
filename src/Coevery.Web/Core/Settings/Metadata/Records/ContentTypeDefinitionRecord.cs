using System.Collections.Generic;
using Coevery.ContentManagement.Records;
using Coevery.Data.Conventions;

namespace Coevery.Core.Settings.Metadata.Records {
    public class ContentTypeDefinitionRecord : ContentPartVersionRecord {
        public ContentTypeDefinitionRecord() {
            ContentTypePartDefinitionRecords = new List<ContentTypePartDefinitionRecord>();
        }

        public virtual string Name { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual bool Customized { get; set; }

        [StringLengthMax]
        public virtual string Settings { get; set; }

        [CascadeAllDeleteOrphan]
        public virtual IList<ContentTypePartDefinitionRecord> ContentTypePartDefinitionRecords { get; set; }
    }
}
