
using System.Collections.Generic;
using Coevery.Data.Conventions;

namespace Coevery.PropertyManagement.Models
{
    [Table("HouseMeter")]
    public class HouseMeterRecord
    {
        public HouseMeterRecord() {
            MeterReadings = new List<HouseMeterReadingPartRecord>();
        }

        public virtual int Id { get; set; }

        public virtual string MeterNumber { get; set; }
        public virtual double Ratio { get; set; }

        //仪表类型 对象属性 ，对应表中MeterTypeId（需要加进表，mapping对应）
        public virtual MeterTypePartRecord MeterType { get; set; }


        //表示属于哪个房间,对应表中HouseId字段（需要mapping对应，HousePartRecord中的lineitems对象要设置HouseId为外键列）
        public virtual HousePartRecord House { get; set; }

        [Aggregate]
        public virtual IList<HouseMeterReadingPartRecord> MeterReadings { get; set; }
    }
}