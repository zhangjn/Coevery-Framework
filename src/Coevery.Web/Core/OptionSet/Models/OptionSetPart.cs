﻿using System.Collections.Generic;
using Coevery.ContentManagement;
using Coevery.ContentManagement.Utilities;

namespace Coevery.Core.OptionSet.Models {
    public class OptionSetPart : ContentPart<OptionSetPartRecord> {
        internal LazyField<IEnumerable<OptionItemPart>> OptionItemsField = new LazyField<IEnumerable<OptionItemPart>>();

        public IEnumerable<OptionItemPart> OptionItems {
            get { return OptionItemsField.Value; }
        }

        public string Name {
            get { return Record.Name; }
            set { Record.Name = value; }
        }

        public bool IsInternal {
            get { return Record.IsInternal; }
            set { Record.IsInternal = value; }
        }

    }
}
