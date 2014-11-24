using Coevery.ContentManagement;
using Coevery.ContentManagement.Drivers;
using Coevery.PropertyManagement.Models;

namespace Coevery.PropertyManagement.Drivers {
    public class ContractPartDriver : ContentPartDriver<ContractPart> {
        protected override DriverResult Display(ContractPart part, string displayType, dynamic shapeHelper) {
			return Combined(
                ContentShape("Parts_ListView",
                    () => shapeHelper.Parts_ListView()),
                ContentShape("Parts_DetailView",
                    () => shapeHelper.Parts_DetailView())
                );
        }

        protected override DriverResult Editor(ContractPart part, string displayType, dynamic shapeHelper) {
            return Combined(
                ContentShape("Parts_CreateView", () => shapeHelper.Parts_CreateView()),
                ContentShape("Parts_EditView", () => shapeHelper.Parts_EditView())
                );
        }

        protected override DriverResult Editor(ContractPart part, IUpdateModel updater, string displayType, dynamic shapeHelper) {
            return Editor(part, displayType, shapeHelper);
        }
    }
}