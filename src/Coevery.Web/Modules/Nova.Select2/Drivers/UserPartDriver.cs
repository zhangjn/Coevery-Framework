using Coevery.ContentManagement;
using Coevery.ContentManagement.Drivers;
using Nova.Select2.Models;

namespace Nova.Select2.Drivers {
    public class UserPartDriver : ContentPartDriver<UserPart> {
        protected override DriverResult Display(UserPart part, string displayType, dynamic shapeHelper) {
			return Combined(
                ContentShape("Parts_ListView",
                    () => shapeHelper.Parts_ListView()),
                ContentShape("Parts_DetailView",
                    () => shapeHelper.Parts_DetailView())
                );
        }

        protected override DriverResult Editor(UserPart part, string displayType, dynamic shapeHelper) {
            return Combined(
                ContentShape("Parts_CreateView", () => shapeHelper.Parts_CreateView()),
                ContentShape("Parts_EditView", () => shapeHelper.Parts_EditView())
                );
        }

        protected override DriverResult Editor(UserPart part, IUpdateModel updater, string displayType, dynamic shapeHelper) {
            return Editor(part, displayType, shapeHelper);
        }
    }
}