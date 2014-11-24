using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevery.PropertyManagement.ViewModels
{
    public class HoseMeterReadingItemsEntity
    {
        public int Id { get; set; }
        public double MeterData { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }
}