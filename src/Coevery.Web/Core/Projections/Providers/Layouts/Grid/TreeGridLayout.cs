﻿using System.Collections.Generic;
using Coevery.Core.Projections.Descriptors.Layout;
using Coevery.Core.Projections.Services;
using Coevery.DisplayManagement;
using Coevery.Localization;

namespace Coevery.Core.Projections.Providers.Layouts.Grid {
    public class TreeGridLayout : ILayoutProvider {
        protected dynamic Shape { get; set; }

        public TreeGridLayout(IShapeFactory shapeFactory) {
            Shape = shapeFactory;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void Describe(DescribeLayoutContext describe) {
            describe.For("Grids", T("Grids"), T("Grid HTML Layouts"))
                .Element("Tree", T("Tree"), T("Organizes entity items in a tree grid."),
                    DisplayLayout,
                    RenderLayout,
                    "TreeGridLayout"
                );
        }

        public LocalizedString DisplayLayout(LayoutContext context) {
            return T("Tree grid style for entity.");
        }

        public dynamic RenderLayout(LayoutContext context, IEnumerable<LayoutComponentResult> layoutComponentResults) {
            string expandField = context.State["ExpandField"];
            string parentField = context.State["ParentField"];
            return Shape.TreeGrid(State:
                new {
                    ExpandColumn = expandField.GetFieldName(),
                    ExpandColClick = false,
                    treeGridModel = "adjacency",
                    treeGrid = true,
                    loadonce = true,
                    cmTemplate = new {
                        sortable = false
                    }
                });
        }
    }
}