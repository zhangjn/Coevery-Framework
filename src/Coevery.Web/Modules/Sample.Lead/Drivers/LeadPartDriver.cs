﻿using Coevery.ContentManagement.Drivers;
using Sample.Lead.Models;

namespace Sample.Lead.Drivers
{
    public class LeadPartDriver: ContentPartDriver<LeadPart> {
        protected override DriverResult Display(LeadPart part, string displayType, dynamic shapeHelper) {
            if (displayType == "List") {
                return Combined(
                    ContentShape("Parts_ListView",
                        () => shapeHelper.Parts_ListView())
                    );
            }
            return null;
        }
    }
}