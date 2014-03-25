﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Coevery.ContentManagement;
using Coevery.Core.Common;
using Coevery.Core.Common.Extensions;
using Coevery.Core.Common.Models;
using Coevery.Core.Navigation.Models;
using Coevery.Core.Navigation.Services;
using Coevery.Core.Navigation.ViewModels;
using Coevery.DeveloperTools.Perspectives.Models;
using Coevery.Localization;
using Coevery.Security.Permissions;
using Coevery.UI;
using Coevery.UI.Navigation;

namespace Coevery.DeveloperTools.Perspectives.FrontMenu {
    public class FrontMenu : INavigationProvider {
        private readonly IMenuService _menuService;
        private readonly IContentDefinitionExtension _contentDefinitionExtension;

        public FrontMenu(
            ICoeveryServices coeveryServices,
            IContentDefinitionExtension contentDefinitionExtension,
            IMenuService menuService) {
            _contentDefinitionExtension = contentDefinitionExtension;
            Services = coeveryServices;
            _menuService = menuService;
        }

        public Localizer T { get; set; }

        public string MenuName {
            get { return "FrontMenu"; }
        }

        public ICoeveryServices Services { get; set; }

        public void GetNavigation(NavigationBuilder builder) {
            var menus = Services.ContentManager.Query<PerspectivePart, PerspectivePartRecord>().OrderBy(x => x.Position).ForType("Menu").List().ToList();
            menus.ForEach(c => {
                builder.AddImageSet(c.Title)
                    .Add(T(c.Title), c.Position.ToString(),
                        menu => {
                            var url = string.Format("#/{0}", c.Id);
                            menu.Url(url);
                            menu.Content(c);
                            menu.IdHint(c.Id.ToString(CultureInfo.InvariantCulture));
                            var subMenus = _menuService.GetMenuParts(c.Id)
                                .OrderBy(menuPartEntry => menuPartEntry.MenuPosition, new FlatPositionComparer()).ToList();
                            var childMenus = subMenus.Where(menuPartEntry =>
                                menuPartEntry.MenuPosition.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries).Length == 1)
                                .ToList();
                            foreach (var childMenu in childMenus) {
                                AddChildMenuItem(childMenu, url, subMenus, ref menu);
                            }
                        });
            });
        }

        private static int GetLevel(string position) {
            var orderList = position.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
            return orderList.Length;
        }

        private IEnumerable<MenuPart> GetChildMenuParts(string position, IEnumerable<MenuPart> subMenuParts) {
            var rawList = subMenuParts.Where(menu => menu.MenuPosition.StartsWith(position + "."));
            return !rawList.Any() ? null : rawList.Where(menu => GetLevel(menu.MenuPosition) == GetLevel(position) + 1);
        }

        private void AddChildMenuItem(MenuPart childMenu, string baseUrl, IEnumerable<MenuPart> subMenuParts, ref NavigationItemBuilder builder) {
            var subMenuContent = childMenu;
            var menuItemEntity = CreateMenuItemEntries(childMenu, baseUrl);

            if (!childMenu.Is<ModuleMenuItemPart>()) {
                builder.Add(T(menuItemEntity.Text), menuItemEntity.Position,
                    item => Build(subMenuContent, menuItemEntity, baseUrl, subMenuParts, item));
            }
            else {
                var moduleMenuItem = childMenu.As<ModuleMenuItemPart>();
                var typeDefinition = Services.ContentManager.New(moduleMenuItem.ContentTypeDefinitionRecord.Name).TypeDefinition;
                var permission = DynamicPermissions.CreateDynamicPermission(DynamicPermissions.PermissionTemplates[Permissions.ViewContent.Name], typeDefinition);
                builder.Add(T(menuItemEntity.Text), menuItemEntity.Position,
                    item => Build(subMenuContent, menuItemEntity, baseUrl, subMenuParts, item, permission),
                    new List<string> {moduleMenuItem.IconClass});
            }
        }

        private void Build(MenuPart subMenuContent, MenuItemEntry menuItemEntity, string baseUrl, IEnumerable<MenuPart> subMenuParts, NavigationItemBuilder item, Permission permission = null) {
            item.Url(menuItemEntity.Url)
                .Content(subMenuContent)
                .IdHint(subMenuContent.Id.ToString(CultureInfo.InvariantCulture));

            if (permission != null) {
                item.Permission(permission);
            }

            var grandChildMenus = GetChildMenuParts(subMenuContent.MenuPosition, subMenuParts);
            if (grandChildMenus == null) {
                return;
            }
            foreach (var grandChildMenu in grandChildMenus) {
                AddChildMenuItem(grandChildMenu, baseUrl, subMenuParts, ref item);
            }
        }

        private MenuItemEntry CreateMenuItemEntries(MenuPart menuPart, string parentUrl) {
            string urlFormat = parentUrl + "/{0}";
            string url = "";

            if (menuPart.Is<ModuleMenuItemPart>()) {
                var urlName = _contentDefinitionExtension.GetEntityNames(
                    menuPart.As<ModuleMenuItemPart>().ContentTypeDefinitionRecord.Name).CollectionName;
                url = string.Format(urlFormat, urlName);
            }
                //@todo: add process for other category of navigation item
            else if (menuPart.Is<MenuItemPart>()) {
                url = menuPart.As<MenuItemPart>().Url;
            }

            return new MenuItemEntry {
                MenuItemId = menuPart.Id,
                IsMenuItem = menuPart.Is<MenuItemPart>(),
                Text = menuPart.MenuText,
                Position = menuPart.MenuPosition,
                Url = url,
                ContentItem = menuPart.ContentItem,
            };
        }
    }
}