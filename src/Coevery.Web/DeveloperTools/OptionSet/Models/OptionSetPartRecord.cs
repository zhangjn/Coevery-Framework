using Coevery.ContentManagement.Records;

namespace Coevery.DeveloperTools.OptionSet.Models {
    public class OptionSetPartRecord : ContentPartRecord {
        public virtual string TermTypeName { get; set; }
        public virtual string Name { get; set; }
        public virtual bool IsInternal { get; set; }
    }
}
