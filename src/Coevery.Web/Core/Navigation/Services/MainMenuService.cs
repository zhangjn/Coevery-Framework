﻿using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Coevery.ContentManagement;
using Coevery.Core.Navigation.Models;

namespace Coevery.Core.Navigation.Services {
    [UsedImplicitly]
    public class MainMenuService : IMenuService {
        private readonly IContentManager _contentManager;

        public MainMenuService(IContentManager contentManager) {
            _contentManager = contentManager;
        }

        public IEnumerable<MenuPart> Get() {
            return _contentManager.Query<MenuPart, MenuPartRecord>().List();
        }

        public IEnumerable<MenuPart> GetMenuParts(int menuId) {
            return _contentManager
                .Query<MenuPart, MenuPartRecord>()
                .Where( x => x.MenuId == menuId)
                .List();
        }

        public IContent GetMenu(string menuName) {
            if(string.IsNullOrWhiteSpace(menuName)) {
                return null;
            }

            return _contentManager.Query<MenuPart, MenuPartRecord>()
                .Where(x => x.Name == menuName)
                .ForType("Menu")
                .Slice(0, 1)
                .FirstOrDefault();
        }

        public IContent GetMenu(int menuId) {
            return _contentManager.Get(menuId, VersionOptions.Published);    
        }

        public MenuPart Get(int menuPartId) {
            return _contentManager.Get<MenuPart>(menuPartId);
        }

        public IContent Create(string name) {
            
            if(string.IsNullOrWhiteSpace(name)) {
                throw new ArgumentNullException(name);
            }
            
            var menu = _contentManager.Create("Menu");
            menu.As<MenuPart>().Name = name;

            return menu;
        }

        public void Delete(MenuPart menuPart) {
            _contentManager.Remove(menuPart.ContentItem);
        }

        public IEnumerable<ContentItem> GetMenus() {
            return _contentManager.Query().ForType("Menu").Join<MenuPartRecord>().OrderBy(x => x.Name).List();
        }
    }
}