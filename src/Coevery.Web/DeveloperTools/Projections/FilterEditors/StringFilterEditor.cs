﻿using System;
using System.Linq;
using Coevery.ContentManagement;
using Coevery.DeveloperTools.Projections.FilterEditors.Forms;
using Coevery.Localization;

namespace Coevery.DeveloperTools.Projections.FilterEditors {
    public class StringFilterEditor : IFilterEditor {
        public StringFilterEditor() {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public bool CanHandle(Type type) {
            return new[] {
                typeof(char), 
                typeof(string),
            }.Contains(type);
        }

        public string FormName {
            get { return StringFilterForm.FormName; }
        }

        public Action<IHqlExpressionFactory> Filter(string property, dynamic formState) {
            return StringFilterForm.GetFilterPredicate(formState, property);
        }

        public LocalizedString Display(string property, dynamic formState) {
            return StringFilterForm.DisplayFilter(property, formState, T);
        }
    }
}