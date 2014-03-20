using System.Collections.Generic;

namespace Coevery.DeveloperTools.FormDesigner.Models {
    public class Section {
        public Section() {
            Rows = new List<Row>();
        }

        public int SectionColumns { get; set; }
        public string SectionColumnsWidth { get; set; }
        public string SectionTitle { get; set; }
        public IList<Row> Rows { get; set; }
    }
}