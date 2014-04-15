using System.Collections.Generic;
using Coevery.ContentManagement;
using Coevery.ContentManagement.Records;
using Coevery.Data.Conventions;

namespace Coevery.Core.Settings.Metadata.Records {
    public class ContentPartDefinitionPart : ContentPart<ContentPartDefinitionRecord> {
        public string Name {
            get { return Record.Name; }
            set { Record.Name = value; }
        }
    }
}
