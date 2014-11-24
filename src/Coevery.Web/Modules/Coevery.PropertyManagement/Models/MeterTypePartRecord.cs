using Coevery.ContentManagement.Records;
using Coevery.Data.Conventions;

namespace Coevery.PropertyManagement.Models
{
    [Table("MeterType")]
    public class MeterTypePartRecord : ContentPartRecord
    {
        public virtual string Name { get; set; }
        public virtual string Unit { get; set; }
        public virtual string Description { get; set; }
    }
}