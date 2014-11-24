using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.ContentManagement.Records;
using Coevery.Data.Conventions;
using Coevery.Users.Models;

namespace Coevery.PropertyManagement.Models
{
    [Table("Service")]
    public class ServicePartRecord : ContentPartRecord
    {
        //public virtual int HouseId { get; set; }
        //public virtual int OwnerId { get; set; }
        public virtual HousePartRecord House { get; set; }
        public virtual CustomerPartRecord Owner { get; set; }
        public virtual string Mobile { get; set; }
        public virtual string FaultDescription { get; set; }
        public virtual DateTime ReceivedDate { get; set; }
        //public virtual int ServicePersonId { get; set; }
        public virtual UserPartRecord ServicePerson { get; set; }
        public virtual decimal ServiceCharge { get; set; }
        public virtual string ServiceVoucherNo { get; set; }
        public virtual string StockOutVoucher { get; set; }
        public virtual string StockReturnVoucher { get; set; }
    }
}