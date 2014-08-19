using System;
using System.Collections.Generic;
using System.Linq;
using Coevery.ContentManagement.MetaData;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.Core.Settings.Metadata;
using Coevery.Environment.Extensions.Models;
using Coevery.Security;
using Coevery.Security.Permissions;

namespace Coevery.Roles {
    public class DynamicPermissions : IPermissionProvider {
        private static readonly Permission Create = new Permission {Description = "Create {0}", Name = "Create_{0}", ImpliedBy = new[] {StandardPermissions.Create}};
        private static readonly Permission Edit = new Permission {Description = "Edit {0}", Name = "Edit_{0}", ImpliedBy = new[] {Create, StandardPermissions.Edit}};
        private static readonly Permission Delete = new Permission {Description = "Delete {0}", Name = "Delete_{0}", ImpliedBy = new[] {StandardPermissions.Delete}};
        private static readonly Permission View = new Permission {Description = "View {0}", Name = "View_{0}", ImpliedBy = new[] {Delete, StandardPermissions.View}};

        public static readonly Dictionary<string, Permission> PermissionTemplates = new Dictionary<string, Permission> {
            {StandardPermissions.Create.Name, Create},
            {StandardPermissions.Edit.Name, Edit},
            {StandardPermissions.Delete.Name, Delete},
            {StandardPermissions.View.Name, View},
        };

        private readonly IContentDefinitionQuery _contentDefinitionQuery;

        public virtual Feature Feature { get; set; }

        public DynamicPermissions(IContentDefinitionQuery contentDefinitionQuery) {
            _contentDefinitionQuery = contentDefinitionQuery;
        }

        public IEnumerable<Permission> GetPermissions() {

            var creatableTypes = _contentDefinitionQuery.ListTypeDefinitions().Where(x => x.Settings.GetModel<ContentTypeSettings>().Creatable);

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
