﻿using System;
using JetBrains.Annotations;
using Coevery.ContentManagement;
using Coevery.ContentManagement.Drivers;
using Coevery.Core.Navigation.Models;
using Coevery.Core.Navigation.Settings;
using Coevery.Localization;
using Coevery.Security;
using Coevery.UI.Navigation;
using Coevery.Utility;

namespace Coevery.Core.Navigation.Drivers {
    [UsedImplicitly]
    public class AdminMenuPartDriver : ContentPartDriver<AdminMenuPart> {
        private readonly IAuthorizationService _authorizationService;
        private readonly INavigationManager _navigationManager;
        private readonly ICoeveryServices _coeveryServices;

        public AdminMenuPartDriver(IAuthorizationService authorizationService, INavigationManager navigationManager, ICoeveryServices coeveryServices) {
            _authorizationService = authorizationService;
            _navigationManager = navigationManager;
            _coeveryServices = coeveryServices;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        private string GetDefaultPosition(ContentPart part) {
            var settings = part.Settings.GetModel<AdminMenuPartTypeSettings>();
            var defaultPosition = settings == null ? "" : settings.DefaultPosition;
            var adminMenu = _navigationManager.BuildMenu("admin");
            if (!string.IsNullOrEmpty(defaultPosition)) {
                int major;
                return int.TryParse(defaultPosition, out major) ? Position.GetNextMinor(major, adminMenu) : defaultPosition;
            }
            return Position.GetNext(adminMenu);
        }

        protected override DriverResult Editor(AdminMenuPart part, string displayType, dynamic shapeHelper)
        {
            // todo: we need a 'ManageAdminMenu' too?
            if (!_authorizationService.TryCheckAccess(Permissions.ManageMainMenu, _coeveryServices.WorkContext.CurrentUser, part)) {
                return null;
            }

            if (string.IsNullOrEmpty(part.AdminMenuPosition)) {
                part.AdminMenuPosition = GetDefaultPosition(part);
            }

            return ContentShape("Parts_Navigation_AdminMenu_Edit",
                                () => shapeHelper.EditorTemplate(TemplateName: "Parts.Navigation.AdminMenu.Edit", Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(AdminMenuPart part, IUpdateModel updater, string displayType, dynamic shapeHelper)
        {
            if (!_authorizationService.TryCheckAccess(Permissions.ManageMainMenu, _coeveryServices.WorkContext.CurrentUser, part))
                return null;

            updater.TryUpdateModel(part, Prefix, null, null);

            if (part.OnAdminMenu) {
                if (string.IsNullOrEmpty(part.AdminMenuText)) {
                    updater.AddModelError("AdminMenuText", T("The AdminMenuText field is required"));
                }

                if (string.IsNullOrEmpty(part.AdminMenuPosition)) {
                    part.AdminMenuPosition = GetDefaultPosition(part);
                }
            }
            else {
                part.AdminMenuPosition = "";
            }

            return Editor(part, displayType, shapeHelper);
        }

        protected override void Importing(AdminMenuPart part, ContentManagement.Handlers.ImportContentContext context) {
            var adminMenuText = context.Attribute(part.PartDefinition.Name, "AdminMenuText");
            if (adminMenuText != null) {
                part.AdminMenuText = adminMenuText;
            }

            var position = context.Attribute(part.PartDefinition.Name, "AdminMenuPosition");
            if (position != null) {
                part.AdminMenuPosition = position;
            }

            var onAdminMenu = context.Attribute(part.PartDefinition.Name, "OnAdminMenu");
            if (onAdminMenu != null) {
                part.OnAdminMenu = Convert.ToBoolean(onAdminMenu);
            }
        }

        protected override void Exporting(AdminMenuPart part, ContentManagement.Handlers.ExportContentContext context) {
            context.Element(part.PartDefinition.Name).SetAttributeValue("AdminMenuText", part.AdminMenuText);
            context.Element(part.PartDefinition.Name).SetAttributeValue("AdminMenuPosition", part.AdminMenuPosition);
            context.Element(part.PartDefinition.Name).SetAttributeValue("OnAdminMenu", part.OnAdminMenu);
        }
    }
}