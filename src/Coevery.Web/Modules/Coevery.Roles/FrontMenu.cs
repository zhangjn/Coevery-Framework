using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Coevery.Localization;
using Coevery.Security;
using Coevery.UI.Navigation;

namespace Coevery.Roles {
    public class FrontMenu : INavigationProvider {
        public FrontMenu(ICoeveryServices services) {
            T = NullLocalizer.Instance;
            CoeveryServices = services;
        }

        public Localizer T { get; set; }
        public ICoeveryServices CoeveryServices { get; set; }

        public void GetNavigation(NavigationBuilder builder) {
            var requestContext = new RequestContext(CoeveryServices.WorkContext.HttpContext, new RouteData());
            var urlhelper = new UrlHelper(requestContext);

            //builder.AddImageSet("users")
            builder.Add(T("用户角色管理"), "100",
                menu => menu.Add(T("Roles"), "1", item => item.Url(urlhelper.Action("Index", "Role", new {area = "Coevery.Roles"}))
                    .Permission(StandardPermissions.SiteOwner), new[] { "icon-users" }));
        }

        public string MenuName {
            get { return "FrontMenu"; }
        }
    }
}