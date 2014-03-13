﻿using System;
using System.Collections.Generic;
using System.Linq;
using Coevery.ContentManagement;
using Coevery.DeveloperTools.Projections.Descriptors.Layout;
using Coevery.DeveloperTools.Projections.Models;
using Coevery.DeveloperTools.Projections.Services;
using Coevery.DisplayManagement;
using Coevery.Localization;

namespace Coevery.DeveloperTools.Projections.Providers.Layouts {
    public class ListLayout : ILayoutProvider {
        private readonly IContentManager _contentManager;
        protected dynamic Shape { get; set; }

        public ListLayout(IShapeFactory shapeFactory, IContentManager contentManager) {
            _contentManager = contentManager;
            Shape = shapeFactory;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void Describe(DescribeLayoutContext describe) {
            describe.For("Html", T("Html"),T("Html Layouts"))
                .Element("List", T("Html List"), T("Organizes content items inside a simple list."),
                    DisplayLayout,
                    RenderLayout,
                    "ListLayout"
                );
        }

        public LocalizedString DisplayLayout(LayoutContext context) {
            string order = context.State.Order;

            switch(order) {
                case "ordered":
                    return T("Ordered Html List");
                case "unordered":
                    return T("Unordered Html List");
                default:
                    throw new ArgumentOutOfRangeException("order");
            }
        }

        public dynamic RenderLayout(LayoutContext context, IEnumerable<LayoutComponentResult> layoutComponentResults) {
            string order = context.State.Order;
            string itemClass = context.State.ItemClass;
            string listClass = context.State.ListClass;
            string listId = context.State.ListId;

            string listTag = order == "ordered" ? "ol" : "ul";

            IEnumerable<dynamic> shapes;
            if(context.LayoutRecord.Display == (int) LayoutRecord.Displays.Content) {
                shapes = layoutComponentResults.Select(x => _contentManager.BuildDisplay(x.ContentItem, context.LayoutRecord.DisplayType));
            }
            else {
                shapes = layoutComponentResults.Select(x => x.Properties);
            }

            var classes = String.IsNullOrEmpty(listClass) ? Enumerable.Empty<string>() : new[] { listClass };
            var itemClasses = String.IsNullOrEmpty(itemClass) ? Enumerable.Empty<string>() : new[] { itemClass };

            return Shape.List(Id: listId, Items: shapes, Tag: listTag, Classes: classes, ItemClasses: itemClasses);
        }
    }
}