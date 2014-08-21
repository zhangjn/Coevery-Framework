using System.Collections.Generic;
using Coevery.Core.Fields.Settings;
using Coevery.Core.Relationships.ViewModels;

namespace Coevery.Core.Relationships.Settings {
    public class ReferenceFieldSettings : FieldSettings {
        public bool DisplayAsLink { get; set; }

        /// <summary>
        /// The ID of the Query object used to generate the list of selectable content items.
        /// </summary>
        public string ContentTypeName { get; set; }

        public int QueryId { get; set; }
        public int RelationshipId { get; set; }
        public string RelationshipName { get; set; }
        public bool IsUnique { get; set; }
        public string DisplayFieldName { get; set; }
        public DeleteAction DeleteAction { get; set; }

        public IList<EntityViewModel> Entities { get; set; }
    }

    public enum DeleteAction {
        NotAllowed,
        Clear,
        Cascade
    }
}