using Coevery.ContentManagement;
using Coevery.ContentManagement.FieldStorage;

namespace Coevery.Core.Fields.Fields {
    public class CurrencyField : ContentField {
        public decimal? Value {
            get { return Storage.Get<decimal?>(Name); }

            set { Storage.Set(value); }
        }
    }
}
