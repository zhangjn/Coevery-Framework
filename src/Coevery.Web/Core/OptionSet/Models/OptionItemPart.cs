using System.Collections.Generic;
using System.Linq;
using Coevery.ContentManagement;

namespace Coevery.Core.OptionSet.Models {
    public class OptionItemPart : ContentPart<OptionItemPartRecord> {
        public string Name {
            get { return Record.Name; }
            set { Record.Name = value; }
        }

        public int OptionSetId {
            get { return Record.OptionSetId; }
            set { Record.OptionSetId = value; }
        }

        public bool Selectable {
            get { return Record.Selectable; }
            set { Record.Selectable = value; }
        }

        public int Weight {
            get { return Record.Weight; }
            set { Record.Weight = value; }
        }

        public static IEnumerable<OptionItemPart> Sort(IEnumerable<OptionItemPart> items) {
            var list = items.OrderBy(i => i.Weight).ToList();
            return list;
        }
    }
}