using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.ContentManagement.Records;
using Coevery.Data.Conventions;

namespace Coevery.PropertyManagement.Models
{
    [Table("Material")]
    public class MaterialPartRecord : ContentPartRecord
    {
        public virtual string SerialNo { get; set; }
        public virtual string Name { get; set; }
        public virtual string Brand { get; set; }
        public virtual string Model { get; set; }
        public virtual double? CostPrice { get; set; }
        public virtual string Remark { get; set; }
        public virtual string Unit { get; set; }
    }
}