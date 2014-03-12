﻿using Coevery.ContentManagement;
using Coevery.ContentManagement.FieldStorage;

namespace Coevery.DeveloperTools.Fields.Fields {
    public class CurrencyField : ContentField {
        public decimal? Value {
            get { return Storage.Get<decimal?>(Name); }

            set { Storage.Set(value); }
        }
    }
}
