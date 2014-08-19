using System;
using System.Collections.Generic;
using System.Linq;
using Coevery.Environment.Extensions.Models;
using Coevery.Security;
using Coevery.Security.Permissions;

namespace Coevery.DeveloperTools.EntityManagement {
    public class Permissions : IPermissionProvider {
        public Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions() {
            return new[] {
                StandardPermissions.AccessAdminPanel
            };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes() {
            return new[] {
                new PermissionStereotype {
                    Name = "Administrator",
                    Permissions = new[] {StandardPermissions.AccessAdminPanel}
                }
            };
        }
    }
}