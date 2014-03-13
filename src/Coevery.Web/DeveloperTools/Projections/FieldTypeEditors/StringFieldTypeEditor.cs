using System;
using System.Linq;
using Coevery.ContentManagement;
using Coevery.DeveloperTools.Projections.FilterEditors.Forms;
using Coevery.DeveloperTools.Projections.Models;
using Coevery.Localization;

namespace Coevery.DeveloperTools.Projections.FieldTypeEditors {
    public class StringFieldTypeEditor : IFieldTypeEditor {
        public Localizer T { get; set; }

        public StringFieldTypeEditor() {
            T = NullLocalizer.Instance;
        }

        public bool CanHandle(Type storageType) {
            return new[] {typeof (string), typeof (char)}.Contains(storageType);
        }

        public string FormName {
            get { return StringFilterForm.FormName; }
        }

        public Action<IHqlExpressionFactory> GetFilterPredicate(dynamic formState) {
            return StringFilterForm.GetFilterPredicate(formState, "Value");
        }

        public LocalizedString DisplayFilter(string fieldName, string storageName, dynamic formState) {
            return StringFilterForm.DisplayFilter(fieldName + " " + storageName, formState, T);
        }

        public Action<IAliasFactory> GetFilterRelationship(string aliasName) {
            return x => x.ContentPartRecord<FieldIndexPartRecord>().Property("StringFieldIndexRecords", aliasName);
        }
    }
}