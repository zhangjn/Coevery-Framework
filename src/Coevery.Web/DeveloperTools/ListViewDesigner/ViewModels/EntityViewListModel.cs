using System.Collections.Generic;
using Coevery.Core.Projections.Descriptors.Layout;

namespace Coevery.DeveloperTools.ListViewDesigner.ViewModels {
    public class EntityViewListModel {
        public IEnumerable<LayoutDescriptor> Layouts { get; set; }
    }
}