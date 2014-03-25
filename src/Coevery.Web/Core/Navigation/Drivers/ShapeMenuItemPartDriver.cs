﻿using System;
using Coevery.ContentManagement;
using Coevery.ContentManagement.Drivers;
using Coevery.ContentManagement.Handlers;
using Coevery.Core.Navigation.Models;
using Coevery.Localization;

namespace Coevery.Core.Navigation.Drivers {
    public class ShapeMenuItemPartDriver : ContentPartDriver<ShapeMenuItemPart> {
        private const string TemplateName = "Parts.ShapeMenuItemPart.Edit";

        public ShapeMenuItemPartDriver(ICoeveryServices services) {
            T = NullLocalizer.Instance;
            Services = services;
        }

        public Localizer T { get; set; }
        public ICoeveryServices Services { get; set; }

        protected override string Prefix { get { return "ShapeMenuItemPart"; } }

        protected override DriverResult Editor(ShapeMenuItemPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_ShapeMenuItemPart_Edit", () => {

                return shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: part, Prefix: Prefix);
            });
        }

        protected override DriverResult Editor(ShapeMenuItemPart part, IUpdateModel updater, string displayType, dynamic shapeHelper)
        {
            if (updater.TryUpdateModel(part, Prefix, null, null)) {
                if (String.IsNullOrWhiteSpace(part.ShapeType)) {
                    updater.AddModelError("ShapeType", T("The Shape Type is mandatory."));
                }
            }

            return Editor(part,displayType, shapeHelper);
        }

        protected override void Importing(ShapeMenuItemPart part, ImportContentContext context) {
            IfNotNull(context.Attribute(part.PartDefinition.Name, "ShapeType"), x => part.Record.ShapeType = x);
        }

        private static void IfNotNull<T>(T value, Action<T> then) where T : class {
            if(value != null) {
                then(value);
            }
        }

        protected override void Exporting(ShapeMenuItemPart part, ExportContentContext context) {
            context.Element(part.PartDefinition.Name).SetAttributeValue("ShapeType", part.Record.ShapeType);
        }
    }
}
