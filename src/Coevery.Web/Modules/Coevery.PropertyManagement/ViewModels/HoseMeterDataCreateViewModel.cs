using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevery.PropertyManagement.ViewModels
{
    public class HoseMeterDataCreateViewModel
    {
        public HoseMeterDataCreateViewModel()
        {
            HoseMeterReadingItems = new List<HoseMeterReadingItemsEntity>();
        }

        public List<HoseMeterReadingItemsEntity> HoseMeterReadingItems { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public int HouseMeterTypeItemId { get; set; }
    }
}