using System.ComponentModel.DataAnnotations.Schema;

namespace Coevery.Roles.Models {
    [Table("Permission")]
    public class PermissionRecord {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string FeatureName { get; set; }
        public virtual string Description { get; set; }
    }
}