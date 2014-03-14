﻿using System.Collections.Generic;
using Coevery.Environment.Extensions.Models;
using Coevery.Security.Permissions;

namespace Coevery.Core.Projections {
    public class Permissions : IPermissionProvider {
        public static readonly Permission ManageQueries = new Permission { Description = "Manage queries", Name = "ManageQueries", Category = "Projection"};

        public virtual Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions() {
            yield return ManageQueries;
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes() {
            return new[] {
                new PermissionStereotype {
                    Name = "Administrator",
                    Permissions = new[] { ManageQueries }
                },
                new PermissionStereotype {
                    Name = "Editor",
                    Permissions = new[] { ManageQueries }
                }
            };
        }
    }
}