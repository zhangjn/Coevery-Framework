using Coevery.Localization;
using Coevery.UI.Navigation;

namespace Coevery.DeveloperTools.EntityManagement.Services {
    public class AdminMenu : INavigationProvider {
        private readonly IEntityMetadataService _entityDefinitionManager;
        public Localizer T { get; set; }

        public string MenuName {
            get { return "admin"; }
        }

        public AdminMenu(IEntityMetadataService entityDefinitionManager) {
            _entityDefinitionManager = entityDefinitionManager;
        }

        public void GetNavigation(NavigationBuilder builder) {
            builder.Add(T("Home"), "1", menu => menu.Url(""), new[] { "icon-home" });

            builder.AddImageSet("Entities")
                .Add(T("Entities"), "2",
                    menu => {
                        int menuIdex = 0;
                        menu.Add(T("All Entities"), (++menuIdex).ToString(), item => item.Url("DevTools#/Entities"));
                        var userDefinedTypes = _entityDefinitionManager.GetEntities();
                        foreach (var type in userDefinedTypes) {
                            var typeModel = type;
                            menu.Add(T(typeModel.DisplayName), (++menuIdex).ToString(), item => item.Url("DevTools#/Entities/" + typeModel.Name));
                        }
                    }, new[] {"icon-list"});
        }
    }
}