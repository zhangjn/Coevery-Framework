using System.Collections.Generic;
using Coevery.DeveloperTools.Projections.Descriptors.Layout;

namespace Coevery.DeveloperTools.Projections.ViewModels {
    public class EntityViewListModel {
        public IEnumerable<LayoutDescriptor> Layouts { get; set; }
    }
}