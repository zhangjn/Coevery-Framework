using Coevery.ContentManagement;
using Coevery.ContentManagement.Drivers;
using Nova.Select2.Models;

namespace Nova.Select2.Drivers {
    public class OpportunityPartDriver : ContentPartDriver<OpportunityPart> {
        protected override DriverResult Display(OpportunityPart part, string displayType, dynamic shapeHelper) {
			return Combined(
                ContentShape("Parts_ListView",
                    () => shapeHelper.Parts_ListView()),
                ContentShape("Parts_DetailView",
                    () => shapeHelper.Parts_DetailView())
                );
        }

        protected override DriverResult Editor(OpportunityPart part, string displayType, dynamic shapeHelper) {
            return Combined(
                ContentShape("Parts_CreateView", () => shapeHelper.Parts_CreateView()),
                ContentShape("Parts_EditView", () => shapeHelper.Parts_EditView())
                );
        }

        protected override DriverResult Editor(OpportunityPart part, IUpdateModel updater, string displayType, dynamic shapeHelper) {
            return Editor(part, displayType, shapeHelper);
        }
    }
}