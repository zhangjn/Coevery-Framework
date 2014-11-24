using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.ContentManagement.Handlers;
using Coevery.Data;
using Coevery.PropertyManagement.Models;

namespace Coevery.PropertyManagement.Handlers
{
    public class PaymentPartHandler : ContentHandler
    {
        public PaymentPartHandler(IRepository<PaymentPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}