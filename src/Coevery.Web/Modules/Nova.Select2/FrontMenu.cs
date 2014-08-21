using Coevery.Localization;
using Coevery.UI.Navigation;

namespace Nova.Select2 {
    public class FrontMenu : INavigationProvider {
        public FrontMenu() {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public string MenuName {
            get { return "FrontMenu"; }
        }

        public void GetNavigation(NavigationBuilder builder) {
            builder.Add(T("User"), "1", menu => menu.Url("User").IdHint("User"), new[] {"icon-User"});
            builder.Add(T("Opportunity"), "2", menu => menu.Url("Opportunity").IdHint("Opportunity"), new[] {"icon-Opportunity"});
            builder.Add(T("Lead"), "3", menu => menu.Url("Lead").IdHint("Lead"), new[] {"icon-Lead"});
        }
    }
}