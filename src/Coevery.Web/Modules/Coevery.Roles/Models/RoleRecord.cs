using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Coevery.Data.Conventions;

namespace Coevery.Roles.Models {
    [Table("Role")]
    public class RoleRecord {
        public RoleRecord() {
            RolesPermissions = new List<RolesPermissionsRecord>();
        }

        public virtual int Id { get; set; }
        public virtual string Name { get; set; }

        [CascadeAllDeleteOrphan]
        public virtual IList<RolesPermissionsRecord> RolesPermissions { get; set; }
    }
}