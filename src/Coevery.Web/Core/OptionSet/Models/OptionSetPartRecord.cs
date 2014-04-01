using Coevery.ContentManagement.Records;

namespace Coevery.Core.OptionSet.Models {
    public class OptionSetPartRecord : ContentPartRecord {
        public virtual string Name { get; set; }
        public virtual bool IsInternal { get; set; }
    }
}
