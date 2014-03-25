using System.Collections.Generic;
using System.Web.Mvc;

namespace Coevery.Core.Common.ViewModels {
    public class ModuleMenuItemEditViewModel {
        public string EntityName { get; set; }
        public string IconClass { get; set; }
        public IEnumerable<SelectListItem> Entities { get; set; }
    }
}