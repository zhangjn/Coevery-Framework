﻿using Coevery.ContentManagement.Handlers;
using Coevery.ContentManagement.Records;
using Coevery.Data;

namespace Coevery.DeveloperTools.Entities.DynamicTypeGeneration {
    public abstract class DynamicContentsHandler<TRecord> : ContentHandler where TRecord : ContentPartRecord, new() {
        public DynamicContentsHandler(IRepository<TRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}