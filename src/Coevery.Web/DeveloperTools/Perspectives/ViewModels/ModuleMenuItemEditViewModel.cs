using System.Collections.Generic;
using System.Web.Mvc;

namespace Coevery.DeveloperTools.Perspectives.ViewModels {
    public class ModuleMenuItemEditViewModel {
        public string EntityName { get; set; }
        public string IconClass { get; set; }
        public IEnumerable<SelectListItem> Entities { get; set; }
    }
}