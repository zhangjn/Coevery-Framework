using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.Environment.Extensions.Models;
using Coevery.Roles;
using Coevery.Security.Permissions;

namespace Coevery.PropertyManagement.Security {
    public class Permissions : IPermissionProvider {
        public static readonly Permission BillManage = new Permission { Description = "管理未出账单", Name = "BillManage", Category = "账单" };
        public virtual Feature Feature { get; set; }

        public IEnumerable<Coevery.Security.Permissions.PermissionStereotype> GetDefaultStereotypes() {
            return new[] {
                new PermissionStereotype {
                    Name = "Administrator",
                    Permissions = new[] {BillManage}
                }
            };
        }

        public IEnumerable<Coevery.Security.Permissions.Permission> GetPermissions() {
            return new[] {
                BillManage
            };
        }
    }
}