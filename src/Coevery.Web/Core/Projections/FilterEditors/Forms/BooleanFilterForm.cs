﻿using System;
using Coevery.ContentManagement;
using Coevery.Core.Forms.Services;
using Coevery.DisplayManagement;
using Coevery.Localization;

namespace Coevery.Core.Projections.FilterEditors.Forms {
    public class BooleanFilterForm : IFormProvider {
        public const string FormName = "BooleanFilter";

        protected dynamic Shape { get; set; }
        public Localizer T { get; set; }

        public BooleanFilterForm(IShapeFactory shapeFactory) {
            Shape = shapeFactory;
            T = NullLocalizer.Instance;
        }

        public void Describe(DescribeContext context) {
            Func<IShapeFactory, object> form =
                shape => Shape.FilterEditors_BooleanFilter(Id: FormName);

            context.Form(FormName, form);
        }

        public static LocalizedString DisplayFilter(string fieldName, dynamic formState, Localizer T) {
            bool value = Convert.ToBoolean((bool)formState.Value);
            fieldName = fieldName.Split('.')[1];
            return value
                ? T("{0} is true", fieldName)
                : T("{0} is false", fieldName);
        }

        public static Action<IHqlExpressionFactory> GetFilterPredicate(dynamic formState, string property) {
            bool value = Convert.ToBoolean((bool)formState.Value);

            if (value) {
                return x => x.Gt(property, (long)0);
            }

            return x => x.Or(l => l.Eq(property, (long)0), r => r.IsNull(property));
        }
    }
}