﻿using System;
using System.Linq;
using Coevery.ContentManagement;
using Coevery.Core.Projections.FilterEditors.Forms;
using Coevery.Core.Projections.Models;
using Coevery.Localization;

namespace Coevery.Core.Projections.FieldTypeEditors {
    public class FloatFieldTypeEditor : IFieldTypeEditor {
        public Localizer T { get; set; }

        public FloatFieldTypeEditor() {
            T = NullLocalizer.Instance;
        }

        public bool CanHandle(Type storageType) {
            return new[] {
                typeof(float?), 
                typeof(double?)
            }.Contains(storageType);
        }

        public string FormName {
            get { return NumericFilterForm.FormName; }
        }

        public Action<IHqlExpressionFactory> GetFilterPredicate(dynamic formState) {
            return NumericFilterForm.GetFilterPredicate(formState, "Value");
        }

        public LocalizedString DisplayFilter(string fieldName, string storageName, dynamic formState) {
            return NumericFilterForm.DisplayFilter(fieldName + " " + storageName, formState, T);
        }

        public Action<IAliasFactory> GetFilterRelationship(string aliasName) {
            return x => x.ContentPartRecord<FieldIndexPartRecord>().Property("DoubleFieldIndexRecords", aliasName);
        }
    }
}