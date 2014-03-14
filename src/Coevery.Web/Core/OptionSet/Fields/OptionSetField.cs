using System.Collections.Generic;
using Coevery.ContentManagement;
using Coevery.ContentManagement.FieldStorage;
using Coevery.ContentManagement.Utilities;
using Coevery.Core.OptionSet.Models;

namespace Coevery.Core.OptionSet.Fields {
    /// <summary>
    /// This field has not state, as all terms are saved using <see cref="TermContentItem"/>
    /// </summary>
    public class OptionSetField : ContentField {
        internal LazyField<IEnumerable<OptionItemPart>> OptionItemsField { get; set; }

        public OptionSetField() {
            OptionItemsField = new LazyField<IEnumerable<OptionItemPart>>();
        }

        /// <summary>
        /// Gets the Terms associated with this field
        /// </summary>
        public IEnumerable<OptionItemPart> OptionItems {
            get { return OptionItemsField.Value; }
        }

        public string Value {
            get { return Storage.Get<string>(Name); }
            set { Storage.Set(value ?? string.Empty); }
        }
    }
}