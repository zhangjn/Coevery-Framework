using Coevery.ContentManagement;
using Coevery.ContentManagement.Drivers;
using Coevery.PropertyManagement.Models;

namespace Coevery.PropertyManagement.Drivers {
    public class BuildingPartDriver : ContentPartDriver<BuildingPart> {
        protected override DriverResult Display(BuildingPart part, string displayType, dynamic shapeHelper) {
			return Combined(
                ContentShape("Parts_ListView",
                    () => shapeHelper.Parts_ListView()),
                ContentShape("Parts_DetailView",
                    () => shapeHelper.Parts_DetailView())
                );
        }

        protected override DriverResult Editor(BuildingPart part, string displayType, dynamic shapeHelper) {
            return Combined(
                ContentShape("Parts_CreateView", () => shapeHelper.Parts_CreateView()),
                ContentShape("Parts_EditView", () => shapeHelper.Parts_EditView())
                );
        }

        protected override DriverResult Editor(BuildingPart part, IUpdateModel updater, string displayType, dynamic shapeHelper) {
            return Editor(part, displayType, shapeHelper);
        }
    }
}