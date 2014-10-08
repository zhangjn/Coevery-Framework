using System;
using System.Collections.Generic;
using System.Linq;
using Coevery.Environment.Extensions.Models;
using Coevery.Security.Permissions;

namespace Coevery.Security {
    public class StandardPermissions : IPermissionProvider {
        public static readonly Permission AccessAdminPanel = new Permission { Name = "AccessAdminPanel", Description = "Access admin panel" };
        public static readonly Permission AccessFrontEnd = new Permission { Name = "AccessFrontEnd", Description = "Access site front-end" };
        public static readonly Permission SiteOwner = new Permission { Name = "SiteOwner", Description = "Site Owners Permission" };
        public static readonly Permission Create = new Permission { Description = "Create", Name = "Create" };
        public static readonly Permission Edit = new Permission { Description = "Edit", Name = "Edit", ImpliedBy = new[] { Create } };
        public static readonly Permission Delete = new Permission { Description = "Delete", Name = "Delete" };
        public static readonly Permission View = new Permission { Description = "View", Name = "View", ImpliedBy = new[] { Edit } };

        public Feature Feature {
            get {
                // This is a lie, but it enables the permissions and stereotypes to be created
                return new Feature {
                    Descriptor = new FeatureDescriptor {
                        Id = "Coevery.Framework",
                        Category = "Core",
                        Dependencies = Enumerable.Empty<string>(),
                        Description = "",
                        Extension = new ExtensionDescriptor {
                            Id = "Coevery.Framework"
                        }
                    },
                    ExportedTypes = Enumerable.Empty<Type>()
                };
            }
        }

        public IEnumerable<Permission> GetPermissions() {
            return new[] {
                AccessAdminPanel,
                AccessFrontEnd
            };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes() {
            return new[] {
                new PermissionStereotype {
                    Name = "Administrator",
                    Permissions = new[] {AccessAdminPanel}
                },
                new PermissionStereotype {
                    Name = "Anonymous"
                },
                new PermissionStereotype {
                    Name = "Authenticated",
                    Permissions = new[] {AccessFrontEnd}
                }
            };
        }

    }
}