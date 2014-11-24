using Coevery.ContentManagement.Records;
using Coevery.Data.Conventions;

namespace Coevery.PropertyManagement.Models {
	[Table("HouseMeterReading")]
    public class HouseMeterReadingPartRecord : ContentPartRecord {
		public virtual int Year { get; set; }
		public virtual int Month { get; set; }
        public virtual HouseMeterRecord HouseMeter { get; set; }
	    public virtual double? MeterData { get; set; }
		public virtual double? Amount { get; set; }
		public virtual StatusOption? Status { get; set; }
		public virtual string Remarks { get; set; }
    }

    public enum StatusOption {
		已录入,
		未录入,
	}
}

