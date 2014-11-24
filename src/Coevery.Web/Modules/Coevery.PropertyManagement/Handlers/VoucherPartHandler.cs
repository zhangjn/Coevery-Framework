using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.ContentManagement.Handlers;
using Coevery.Data;
using Coevery.PropertyManagement.Models;

namespace Coevery.PropertyManagement.Handlers {
    public class VoucherPartHandler : ContentHandler {
        public VoucherPartHandler(IRepository<VoucherPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(DeleteRecordFilter.For(repository));
        }
    }
}