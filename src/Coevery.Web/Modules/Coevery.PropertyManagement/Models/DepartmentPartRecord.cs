using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.ContentManagement.Records;
using Coevery.Data.Conventions;

namespace Coevery.PropertyManagement.Models
{
    [Table("Department")]
    public class DepartmentPartRecord : ContentPartRecord
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }
}