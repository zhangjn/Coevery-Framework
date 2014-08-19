using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.Security;
using Coevery.Security.Permissions;
using JetBrains.Annotations;

namespace Coevery.Roles.Services
{
    [UsedImplicitly]
    public class AuthorizationEventHandler : IAuthorizationServiceEventHandler
    {
        public void Checking(CheckAccessContext context) { }
        public void Complete(CheckAccessContext context) { }

        public void Adjust(CheckAccessContext context) {
            if (!context.Granted && context.Content != null) {

                var typeDefinition = context.Content.ContentItem.TypeDefinition;

                var permission = GetContentTypeVariation(context.Permission);

                if (permission != null) {
                    context.Adjusted = true;
                    context.Permission = DynamicPermissions.CreateDynamicPermission(permission, typeDefinition);
                }
            }
        }

        private static Permission GetContentTypeVariation(Permission permission)
        {
            return DynamicPermissions.ConvertToDynamicPermission(permission);
        }
    }
}