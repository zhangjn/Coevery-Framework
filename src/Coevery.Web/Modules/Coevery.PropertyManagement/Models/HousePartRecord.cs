using System.Collections;
using System.Collections.Generic;
using Coevery.ContentManagement.Records;
using Coevery.Data.Conventions;
using Coevery.Users.Models;

namespace Coevery.PropertyManagement.Models
{
    [Table("House")]
    public class HousePartRecord : ContentPartRecord
    {
        public HousePartRecord()
        {
            ChargeItems=new List<HouseChargeItemRecord>();
            MeterTypeItems=new List<HouseMeterRecord>();
        }

        public virtual BuildingPartRecord Building { get; set; }
        public virtual ApartmentPartRecord Apartment { get; set; }
        public virtual CustomerPartRecord Owner { get; set; }
        public virtual string HouseNumber { get; set; }
        //public virtual string Officer { get; set; }
        public virtual double? BuildingArea { get; set; }
        public virtual double? InsideArea { get; set; }
        public virtual double? PoolArea { get; set; }
        public virtual HouseStatusOption? HouseStatus { get; set; }
        public virtual UserPartRecord Officer { get; set; }
        //收费项目items与房间仪表items 的Ilist列表项，在House中加入 （需要在mapping中为其设置外键列对应）
        [CascadeAllDeleteOrphan,Aggregate]
        public virtual IList<HouseChargeItemRecord> ChargeItems { get; set; }

        [CascadeAllDeleteOrphan,Aggregate]
        public virtual IList<HouseMeterRecord> MeterTypeItems { get; set; }
    }

    public enum HouseStatusOption
    {
        空置=1,
        自营=2,
        出租=3
    }
}