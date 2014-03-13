using System;
using Coevery.ContentManagement;
using Coevery.DeveloperTools.Projections.FilterEditors.Forms;
using Coevery.DeveloperTools.Projections.Models;
using Coevery.Localization;
using Coevery.Services;

namespace Coevery.DeveloperTools.Projections.FieldTypeEditors {
    public class DateTimeFieldTypeEditor : ConcreteFieldTypeEditorBase {
        private readonly IClock _clock;

        public Localizer T { get; set; }

        public DateTimeFieldTypeEditor(IClock clock) {
            _clock = clock;
            T = NullLocalizer.Instance;
        }

        public override bool CanHandle(string fieldTypeName, Type storageType) {
            return fieldTypeName == "DatetimeField";
        }

        public override string FormName {
            get { return DateTimeFilterForm.FormName; }
        }

        public override Action<IHqlExpressionFactory> GetFilterPredicate(dynamic formState) {
            return DateTimeFilterForm.GetFilterPredicate(formState, "Value", _clock.UtcNow, true);
        }

        public override LocalizedString DisplayFilter(string fieldName, string storageName, dynamic formState) {
            return DateTimeFilterForm.DisplayFilter(fieldName + " " + storageName, formState, T);
        }

        public override Action<IAliasFactory> GetFilterRelationship(string aliasName) {
            return x => x.ContentPartRecord<FieldIndexPartRecord>().Property("IntegerFieldIndexRecords", aliasName);
        }
    }
}