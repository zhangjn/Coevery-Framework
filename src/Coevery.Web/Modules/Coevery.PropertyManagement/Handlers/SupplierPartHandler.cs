using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.ContentManagement.Handlers;
using Coevery.ContentManagement.Records;
using Coevery.Data;
using Coevery.Localization;
using Coevery.PropertyManagement.Models;
using Coevery.UI.Notify;

namespace Coevery.PropertyManagement.Handlers {
    public class SupplierPartHandler : ContentHandler {
        private readonly Lazy<ISessionLocator> _sessionLocator;
        private readonly ICoeveryServices _coeveryServices;

        public SupplierPartHandler(IRepository<SupplierPartRecord> repository, Lazy<ISessionLocator> sessionLocator, ICoeveryServices coeveryServices) {
            _sessionLocator = sessionLocator;
            _coeveryServices = coeveryServices;
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(DeleteRecordFilter.For(repository));
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

    }
}