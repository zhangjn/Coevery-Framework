using System.Collections.Generic;

namespace Coevery.DeveloperTools.ListViewDesigner.ViewModels {
    public class GridViewModel {
        public GridViewModel() {
            Fields = new List<GridColumnViewModel>();
            SelectedColumns = new List<string>();
        }

        public int Id { get; set; }
        public string ItemContentType { get; set; }
        public string DisplayName { get; set; }
        public IEnumerable<GridColumnViewModel> Fields { get; set; }

        public IEnumerable<string> SelectedColumns { get; set; }
        public string SortColumn { get; set; }
        public string SortMode { get; set; }
        public int PageRowCount { get; set; }
    }
}