using System;
using System.Linq;
using Coevery.ContentManagement;
using Coevery.DeveloperTools.Projections.FilterEditors.Forms;
using Coevery.Localization;

namespace Coevery.DeveloperTools.Projections.FilterEditors {
    public class BooleanFilterEditor : IFilterEditor {
        public BooleanFilterEditor() {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public bool CanHandle(Type type) {
            return new[] {
                typeof(Boolean),
                typeof(Boolean?)
            }.Contains(type);
        }

        public string FormName {
            get { return BooleanFilterForm.FormName; }
        }

        public Action<IHqlExpressionFactory> Filter(string property, dynamic formState) {
            return BooleanFilterForm.GetFilterPredicate(formState, property);
        }

        public LocalizedString Display(string property, dynamic formState) {
            return BooleanFilterForm.DisplayFilter(property, formState, T);
        }
    }
}