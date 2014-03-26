﻿using System.Web;
using Coevery.ContentManagement;
using Coevery.ContentManagement.Aspects;
using Coevery.Core.Navigation.Models;
using Coevery.Localization;
using Coevery.UI.Navigation;

namespace Coevery.Core.Navigation.Services {
    public class DefaultMenuProvider : IMenuProvider {
        private readonly IContentManager _contentManager;

        public DefaultMenuProvider(IContentManager contentManager) {
            _contentManager = contentManager;
        }

        public void GetMenu(IContent menu, NavigationBuilder builder) {
            var menuParts = _contentManager
                .Query<MenuPart, MenuPartRecord>()
                .Where(x => x.MenuId == menu.Id)
                .WithQueryHints(new QueryHints().ExpandRecords<MenuItemPartRecord>())
                .List();

            foreach (var menuPart in menuParts) {
                if (menuPart != null) {
                    var part = menuPart;

                    string culture = null;
                    // fetch the culture of the content menu item, if any
                    var localized = part.As<ILocalizableAspect>();
                    if (localized != null) {
                        culture = localized.Culture;
                    }

                    if (part.Is<MenuItemPart>())
                        builder.Add(new LocalizedString(HttpUtility.HtmlEncode(part.MenuText)), part.MenuPosition, item => item.Url(part.As<MenuItemPart>().Url).Content(part).Culture(culture).Permission(Common.Permissions.ViewContent));
                    else
                        builder.Add(new LocalizedString(HttpUtility.HtmlEncode(part.MenuText)), part.MenuPosition, item => item.Action(_contentManager.GetItemMetadata(part.ContentItem).DisplayRouteValues).Content(part).Culture(culture).Permission(Common.Permissions.ViewContent));
                }
            }
        }
    }
}