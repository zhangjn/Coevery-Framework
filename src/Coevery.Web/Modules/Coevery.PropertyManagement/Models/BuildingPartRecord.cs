using Coevery.ContentManagement.Records;
using Coevery.Data.Conventions;

namespace Coevery.PropertyManagement.Models
{
    [Table("Building")]
    public class BuildingPartRecord : ContentPartRecord
    {
        public virtual int Apartment { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }
}