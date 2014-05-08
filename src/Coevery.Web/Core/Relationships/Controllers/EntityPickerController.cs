using System.Web.Mvc;
using Coevery.Mvc;
using Coevery.Themes;

namespace Coevery.Core.Relationships.Controllers {
    [Themed]
    public class EntityPickerController : Controller {

        public EntityPickerController(ICoeveryServices services) {
            Services = services;
        }

        public ICoeveryServices Services { get; set; }

        public ActionResult Index() {
            var model = Services.New.EntityPicker();
            return new ShapeResult(this, model);
        }
    }
}