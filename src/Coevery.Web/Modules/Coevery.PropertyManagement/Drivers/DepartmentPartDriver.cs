﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.ContentManagement;
using Coevery.ContentManagement.Drivers;
using Coevery.PropertyManagement.Models;

namespace Coevery.PropertyManagement.Drivers
{
    public class DepartmentPartDriver : ContentPartDriver<DepartmentPart>
    {
        protected override DriverResult Display(DepartmentPart part, string displayType, dynamic shapeHelper)
        {
            return Combined(
                ContentShape("Parts_ListView",
                    () => shapeHelper.Parts_ListView()),
                ContentShape("Parts_DetailView",
                    () => shapeHelper.Parts_DetailView())
                );
        }

        protected override DriverResult Editor(DepartmentPart part, string displayType, dynamic shapeHelper)
        {
            return Combined(
                ContentShape("Parts_CreateView", () => shapeHelper.Parts_CreateView()),
                ContentShape("Parts_EditView", () => shapeHelper.Parts_EditView())
                );
        }

        protected override DriverResult Editor(DepartmentPart part, IUpdateModel updater, string displayType, dynamic shapeHelper)
        {
            return Editor(part, displayType, shapeHelper);
        }
    }
}