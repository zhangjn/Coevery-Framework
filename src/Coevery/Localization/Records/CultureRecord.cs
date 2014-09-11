using Coevery.Data.Conventions;

namespace Coevery.Localization.Records {
    [Table("Framework_CultureRecord")]
    public class CultureRecord {
        public virtual int Id { get; set; }
        public virtual string Culture { get; set; }
    }
}