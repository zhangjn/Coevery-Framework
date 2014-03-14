using System.Collections.Generic;
using System.Web.Mvc;
using Coevery.Core.Fields.Settings;

namespace Coevery.Core.Reference.Settings {
    public class ReferenceFieldSettings : FieldSettings {
        public bool DisplayAsLink { get; set; }

        /// <summary>
        /// The ID of the Query object used to generate the list of selectable content items.
        /// </summary>
        public string ContentTypeName { get; set; }

        public int QueryId { get; set; }
        public int RelationshipId { get; set; }
        public string RelationshipName { get; set; }
        public bool IsDisplayField { get; set; }
        public bool IsUnique { get; set; }

        public IList<SelectListItem> ContentTypeList { get; set; }
    }
}