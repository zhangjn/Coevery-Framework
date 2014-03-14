using System;
using System.Linq;
using Coevery.ContentManagement;
using Coevery.Core.Projections.FilterEditors.Forms;
using Coevery.Core.Projections.Models;
using Coevery.Localization;

namespace Coevery.Core.Projections.FieldTypeEditors {
    public class BooleanFieldTypeEditor : IFieldTypeEditor {
        public Localizer T { get; set; }

        public BooleanFieldTypeEditor() {
            T = NullLocalizer.Instance;
        }

        public bool CanHandle(Type storageType) {
            return new[] { typeof(bool), typeof(bool?) }.Contains(storageType);
        }

        public string FormName {
            get { return BooleanFilterForm.FormName; }
        }

        public Action<IHqlExpressionFactory> GetFilterPredicate(dynamic formState) {
            return BooleanFilterForm.GetFilterPredicate(formState, "Value");
        }

        public LocalizedString DisplayFilter(string fieldName, string storageName, dynamic formState) {
            return BooleanFilterForm.DisplayFilter(fieldName + " " + storageName, formState, T);
        }

        public Action<IAliasFactory> GetFilterRelationship(string aliasName) {
            return x => x.ContentPartRecord<FieldIndexPartRecord>().Property("IntegerFieldIndexRecords", aliasName);
        }
    }
}