using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coevery.Core.Forms.Services;

namespace Coevery.ContentManagement
{
    public static class ContentPartExtensions {
        private static readonly string _parameterKey= "__Paramter__";

        public static void StoreParameter<TProperty>(this ContentPart contentPart, string fieldName, string name,
            TProperty value) {
            ContentField field = contentPart.Get(typeof (object), fieldName);
            string parameter;
            field.PartFieldDefinition.Settings.TryGetValue(_parameterKey, out parameter);
            var state = FormParametersHelper.ToDynamic(parameter);
            state[name] = value;
            field.PartFieldDefinition.Settings[_parameterKey] = FormParametersHelper.ToString(state);
        }

        public static TProperty RetrieveParameter<TProperty>(this ContentPart contentPart, string fieldName, string name) {
            ContentField field = contentPart.Get(typeof(object), fieldName);
            var parameter = field.PartFieldDefinition.Settings[_parameterKey];
            var state = FormParametersHelper.ToDynamic(parameter);
            return (TProperty)state[name];
        }
    }
}
