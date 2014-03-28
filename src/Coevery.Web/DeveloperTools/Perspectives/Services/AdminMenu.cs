//using Coevery.Localization;
//using Coevery.UI.Navigation;

//namespace Coevery.DeveloperTools.Perspectives.Services {
//    public class AdminMenu : INavigationProvider {
//        public Localizer T { get; set; }

//        public string MenuName {
//            get { return "admin"; }
//        }

//        public void GetNavigation(NavigationBuilder builder) {
//            builder.AddImageSet("Admin")
//                .Add(T("Admin"), "3",
//                    menu => menu.Add(T("Manage Perspectives"), "1", item => item.Url("~/DevTools#/Perspectives")),
//                    new[] {"icon-cogs"});
//        }
//    }
//}