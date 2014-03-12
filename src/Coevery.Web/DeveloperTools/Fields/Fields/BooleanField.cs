﻿using Coevery.ContentManagement;
using Coevery.ContentManagement.FieldStorage;

namespace Coevery.DeveloperTools.Fields.Fields {
    public class BooleanField : ContentField {
        public bool? Value {
            get { return Storage.Get<bool?>(Name); }
            set { Storage.Set(value); }
        }
    }
}
