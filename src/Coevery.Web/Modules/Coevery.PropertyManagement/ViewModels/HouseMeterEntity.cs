using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevery.PropertyManagement.ViewModels
{
    public class HouseMeterEntity
    {
        public int Id { get; set; }//仪表类型编号
        public int HouseId { get; set; }//可能用到房间编号
        public int MeterTypeItemId { get; set; }//仪表种类Id
        public string MeterName { get; set; }//仪表名称
        public string MeterNumber { get; set; }//仪表编号
        public double Ratio { get; set; }//倍率
    }
}