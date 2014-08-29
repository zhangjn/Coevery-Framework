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

        public static ContentField GetField(this ContentPart contentPart, string fieldName) {
            return contentPart.Get(typeof (object), fieldName);
        }

        public static void SetFieldParameter(this ContentPart contentPart, string fieldName, dynamic state) {
            var field = contentPart.GetField(fieldName);
            SetFieldParameter(field, state);
        }

        public static void SetFieldParameter(this ContentField contentField, dynamic state) {
            string parameter;
            contentField.PartFieldDefinition.Settings.TryGetValue(_parameterKey, out parameter);
            var existing = FormParametersHelper.FromString(parameter);
            IDictionary<string,string> stateDictionary = FormParametersHelper.FromString(FormParametersHelper.ToString(state));
            foreach (var kv in stateDictionary) {
                existing[kv.Key] = kv.Value;
            }
            contentField.PartFieldDefinition.Settings[_parameterKey] = FormParametersHelper.ToString(existing);
        }

        public static dynamic RetrieveParameters(this ContentField contentField)
        {
            var parameter = contentField.PartFieldDefinition.Settings[_parameterKey];
            var state = FormParametersHelper.ToDynamic(parameter);
            return state;
        }
    }
}
