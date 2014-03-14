﻿using Coevery.ContentManagement.Records;

namespace Coevery.Core.Relationships.DynamicTypeGeneration {
    public abstract class ContentLinkRecord {
        public virtual int Id { get; set; }
        public virtual ContentItemRecord PrimaryPartRecord { get; set; }
        public virtual ContentItemRecord RelatedPartRecord { get; set; }
    }
}