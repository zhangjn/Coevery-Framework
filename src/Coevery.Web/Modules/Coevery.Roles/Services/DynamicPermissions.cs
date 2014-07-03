using System;
using System.Collections.Generic;
using System.Linq;
using Coevery.ContentManagement.MetaData;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.Core.Common;
using Coevery.Environment.Extensions;
using Coevery.Environment.Extensions.Models;
using Coevery.Security.Permissions;

namespace Coevery.Roles.Services {
    [CoeverySuppressDependency("Coevery.Core.Common.DynamicPermissions")]
    public class DynamicPermissions : IPermissionProvider {
        private static readonly Permission PublishContent = new Permission { Description = "创建{0}", Name = "Publish_{0}", ImpliedBy = new[] { Permissions.PublishContent } };
        private static readonly Permission EditContent = new Permission { Description = "修改{0}", Name = "Edit_{0}", ImpliedBy = new[] { PublishContent, Permissions.EditContent } };
        private static readonly Permission DeleteContent = new Permission { Description = "删除{0}", Name = "Delete_{0}", ImpliedBy = new[] { Permissions.DeleteContent } };
        private static readonly Permission ViewContent = new Permission { Description = "查看{0}", Name = "View_{0}", ImpliedBy = new[] { EditContent, Permissions.ViewContent } };

        public static readonly Dictionary<string, Permission> PermissionTemplates = new Dictionary<string, Permission> {
            {Permissions.PublishContent.Name, PublishContent},
            {Permissions.EditContent.Name, EditContent},
            {Permissions.DeleteContent.Name, DeleteContent},
            {Permissions.ViewContent.Name, ViewContent},
        };

        private readonly IContentDefinitionQuery _contentDefinitionQuery;

        public virtual Feature Feature { get; set; }

        public DynamicPermissions(IContentDefinitionQuery contentDefinitionQuery)
        {
            _contentDefinitionQuery = contentDefinitionQuery;
        }

        public IEnumerable<Permission> GetPermissions() {
            // manage rights only for Creatable types
            var creatableTypes = _contentDefinitionQuery.ListTypeDefinitions();

            foreach (var typeDefinition in creatableTypes) {
                foreach (var permissionTemplate in PermissionTemplates.Values) {
                    yield return CreateDynamicPermission(permissionTemplate, typeDefinition);
                }
            }
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes() {
            return Enumerable.Empty<PermissionStereotype>();
        }

        /// <summary>
        /// Returns a dynamic permission for a content type, based on a global content permission template
        /// </summary>
        public static Permission ConvertToDynamicPermission(Permission permission) {
            if (PermissionTemplates.ContainsKey(permission.Name)) {
                return PermissionTemplates[permission.Name];
            }

            return null;
        }

        /// <summary>
        /// Generates a permission dynamically for a content type
        /// </summary>
        public static Permission CreateDynamicPermission(Permission template, ContentTypeDefinition typeDefinition) {
            return new Permission {
                Name = String.Format(template.Name, typeDefinition.Name),
                Description = String.Format(template.Description, typeDefinition.DisplayName),
                Category = typeDefinition.DisplayName,
                ImpliedBy = (template.ImpliedBy ?? new Permission[0]).Select(t => CreateDynamicPermission(t, typeDefinition))
            };
        }
    }
}
