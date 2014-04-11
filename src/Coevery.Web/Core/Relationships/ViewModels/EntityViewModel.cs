using System.Collections.Generic;

namespace Coevery.Core.Relationships.ViewModels {
    public class EntityViewModel {
        public EntityViewModel() {
            Fields = new List<string>();
        }

        public string Name { get; set; }
        public bool Selected { get; set; }
        public IList<string> Fields { get; set; }
    }
}