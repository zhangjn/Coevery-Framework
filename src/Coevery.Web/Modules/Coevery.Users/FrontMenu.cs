using System.Web.Mvc;
using System.Web.Routing;
using Coevery.Localization;
using Coevery.Security;
using Coevery.UI.Navigation;

namespace Coevery.Users {
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
                menu => menu.Add(T("Users"), "0", item => item.Url(urlhelper.Action("Index", "User", new {area = "Coevery.Users"}))
                    .Permission(StandardPermissions.SiteOwner), new[] { "icon-user" }));
        }

        public string MenuName {
            get { return "FrontMenu"; }
        }
    }
}