
using System;
using System.Collections.Generic;
using Coevery.ContentManagement.Records;
using Coevery.Data.Conventions;

namespace Coevery.PropertyManagement.Models {
	[Table("Contract")]
    public class ContractPartRecord : ContentPartRecord {
	    public ContractPartRecord()
	    {
            Houses=new List<ContractHouseRecord>();
            ChargeItems=new List<ContractChargeItemRecord>();
	    }

	    public virtual string Number { get; set; }
		public virtual string Name { get; set; }
        //public virtual int? OwnerId { get; set; }//与Owner对象不能同时存在
        //public virtual int? RenterId { get; set; }//与Render不能同时存在
		public virtual ContractStatusOption? ContractStatus { get; set; }
		public virtual DateTime BeginDate { get; set; }
		public virtual DateTime EndDate { get; set; }
		public virtual string Description { get; set; }
        public virtual CustomerPartRecord Renter { get; set; }

        [CascadeAllDeleteOrphan, Aggregate]
        public virtual IList<ContractHouseRecord> Houses { get; set; }

        [CascadeAllDeleteOrphan, Aggregate]
        public virtual IList<ContractChargeItemRecord> ChargeItems { get; set; }

	    public virtual HouseStatusOption? HouseStatus { get; set; }

    }

    public enum ContractStatusOption {
		签订,
        正在执行,
        合同变更,
        终止,
	}
}

