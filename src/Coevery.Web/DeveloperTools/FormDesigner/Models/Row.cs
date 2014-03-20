using System.Collections.Generic;

namespace Coevery.DeveloperTools.FormDesigner.Models {
    public class Row {
        public Row() {
            Columns = new List<Column>();
        }

        public IList<Column> Columns { get; set; }
        public bool IsMerged { get; set; }
    }
}