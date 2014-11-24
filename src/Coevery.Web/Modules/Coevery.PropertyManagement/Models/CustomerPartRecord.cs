using Coevery.ContentManagement.Records;
using Coevery.Data.Conventions;

namespace Coevery.PropertyManagement.Models
{
    [Table("Customer")]
    public class CustomerPartRecord : ContentPartRecord
    {
        public virtual string Number { get; set; }
        public virtual string Name { get; set; }
        public virtual CustomerTypeOption? CustomerType { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Description { get; set; }
        public virtual decimal CustomerBalance { get; set; }
    }

    public enum CustomerTypeOption
    {
        业主,
        租户,
    }
}