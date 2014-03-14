﻿using Coevery.ContentManagement.Records;

namespace Coevery.Core.OptionSet.Models {
    /// <summary>
    /// Represents a Term of a Taxonomy
    /// </summary>
    public class OptionItemPartRecord : ContentPartRecord {
        public virtual int OptionSetId { get; set; }

        public virtual string Name { get; set; }
        public virtual bool Selectable { get; set; }
        public virtual int Weight { get; set; }
    }
}
