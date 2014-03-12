﻿using System;
using Coevery.ContentManagement;
using Coevery.ContentManagement.FieldStorage;

namespace Coevery.DeveloperTools.Fields.Fields {
    public class TextField : ContentField {
        public string Value {
            get { return Storage.Get<string>(Name); }
            set { Storage.Set(value ?? String.Empty); }
        }
    }
}