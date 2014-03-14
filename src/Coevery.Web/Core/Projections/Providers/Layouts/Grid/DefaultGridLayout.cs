using System.Collections.Generic;
using Coevery.Core.Projections.Descriptors.Layout;
using Coevery.Core.Projections.Services;
using Coevery.DisplayManagement;
using Coevery.Localization;

namespace Coevery.Core.Projections.Providers.Layouts.Grid {
    public class DefaultGridLayout : ILayoutProvider {
        protected dynamic Shape { get; set; }

        public DefaultGridLayout(IShapeFactory shapeFactory) {
            Shape = shapeFactory;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void Describe(DescribeLayoutContext describe) {
            describe.For("Grids", T("Grids"), T("Grid HTML Layouts"))
                .Element("Default", T("Default"), T("Organizes entity items in a grid."),
                    DisplayLayout,
                    RenderLayout,
                    "DefaultGridLayout"
                );
        }

        public LocalizedString DisplayLayout(LayoutContext context) {
            return T("Default sortable grid style for entity with pager.");
        }

        public dynamic RenderLayout(LayoutContext context, IEnumerable<LayoutComponentResult> layoutComponentResults) {
            //int pageRowCount = Convert.ToInt32(context.State["PageRowCount"]);
            //string sortedBy = context.State["SortedBy"];
            //string sortMode = context.State["SortMode"];

            return Shape.DefaultGrid();
        }
    }
}