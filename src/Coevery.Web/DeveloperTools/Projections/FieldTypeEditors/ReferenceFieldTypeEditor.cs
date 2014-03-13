using System;
using Coevery.ContentManagement;
using Coevery.DeveloperTools.Projections.FilterEditors.Forms;
using Coevery.DeveloperTools.Projections.Models;
using Coevery.Localization;

namespace Coevery.DeveloperTools.Projections.FieldTypeEditors {
    public class ReferenceFieldTypeEditor : ConcreteFieldTypeEditorBase {
        public Localizer T { get; set; }

        public ReferenceFieldTypeEditor() {
            T = NullLocalizer.Instance;
        }

        public override bool CanHandle(string fieldTypeName, Type storageType) {
            return fieldTypeName == "ReferenceField";
        }

        public override string FormName {
            get { return ReferenceFilterForm.FormName; }
        }

        public override Action<IHqlExpressionFactory> GetFilterPredicate(dynamic formState) {
            return ReferenceFilterForm.GetFilterPredicate(formState, "Value");
        }

        public override LocalizedString DisplayFilter(string fieldName, string storageName, dynamic formState) {
            return ReferenceFilterForm.DisplayFilter(fieldName + " " + storageName, formState, T);
        }

        public override Action<IAliasFactory> GetFilterRelationship(string aliasName) {
            return x => x.ContentPartRecord<FieldIndexPartRecord>().Property("IntegerFieldIndexRecords", aliasName);
        }

        //public override void ApplySortCriterion(Coevery.Projections.Descriptors.SortCriterion.SortCriterionContext context, string storageName, Type storageType, Coevery.ContentManagement.MetaData.Models.ContentPartDefinition part, Coevery.ContentManagement.MetaData.Models.ContentPartFieldDefinition field) {
        //}
    }
}