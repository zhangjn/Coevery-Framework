using System.ComponentModel.DataAnnotations.Schema;

namespace Coevery.Roles.Models {
    [Table("RolesPermissions")]
    public class RolesPermissionsRecord {
        public virtual int Id { get; set; }
        public virtual RoleRecord Role { get; set; }
        public virtual PermissionRecord Permission { get; set; }
    }
}