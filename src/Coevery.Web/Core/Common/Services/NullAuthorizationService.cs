using Coevery.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevery.Core.Common.Services
{
    public class NullAuthorizationService : IAuthorizationService
    {
        public void CheckAccess(Security.Permissions.Permission permission, IUser user, ContentManagement.IContent content)
        {
        }

        public bool TryCheckAccess(Security.Permissions.Permission permission, IUser user, ContentManagement.IContent content)
        {
            return true;
        }
    }
}