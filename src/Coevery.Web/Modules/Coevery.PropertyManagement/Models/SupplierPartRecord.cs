using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.ContentManagement.Records;
using Coevery.Data.Conventions;

namespace Coevery.PropertyManagement.Models
{
    [Table("Supplier")]
    public class SupplierPartRecord : ContentPartRecord
    {
        public virtual string Name { get; set; }
        public virtual string Address { get; set; }
        public virtual string Contactor { get; set; }
        public virtual string Tel { get; set; }
        public virtual string MobilePhone { get; set; }
        public virtual string Email { get; set; }
        public virtual string QQ { get; set; }
        public virtual string Fax { get; set; }
        public virtual string Description { get; set; }
    }
}