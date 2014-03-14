using System.Collections.Generic;
using Coevery.Core.Projections.Descriptors.Layout;

namespace Coevery.Core.Projections.ViewModels {
    public class EntityViewListModel {
        public IEnumerable<LayoutDescriptor> Layouts { get; set; }
    }
}