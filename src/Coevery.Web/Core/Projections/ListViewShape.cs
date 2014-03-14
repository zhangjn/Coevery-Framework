using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using Coevery.DisplayManagement;
using Coevery.DisplayManagement.Descriptors;

namespace Coevery.Core.Projections
{
    public class ListViewShape : IShapeTableProvider {
        public void Discover(ShapeTableBuilder builder) {}

        [Shape]
        public void ngGrid(dynamic Display, TextWriter Output, HtmlHelper Html, IEnumerable<dynamic> Items, String ContentType) {
            Output.WriteLine("<section class=\"gridStyle\" ng-grid=\"gridOptions\"></section>");
        }
    }
}
