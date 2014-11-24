using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.ContentManagement;

namespace Coevery.PropertyManagement.Models
{
    public sealed class DepartmentPart : ContentPart<DepartmentPartRecord>
    {

        public string Name
        {
            get { return Record.Name; }
            set { Record.Name = value; }
        }

      

        public string Description
        {
            get { return Record.Description; }
            set { Record.Description = value; }
        }
    }
}