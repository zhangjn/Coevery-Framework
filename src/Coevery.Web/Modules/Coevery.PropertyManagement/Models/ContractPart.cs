
using System;
using System.Collections.Generic;
using Coevery.ContentManagement;
using Coevery.ContentManagement.Utilities;

namespace Coevery.PropertyManagement.Models {
    public sealed class ContractPart : ContentPart<ContractPartRecord> {
        internal LazyField<int?> _RenterField=new LazyField<int?>();

		public string Number{
			get{ return Record.Number; }
			set{ Record.Number = value; }
		}

		public string Name{
			get{ return Record.Name; }
			set{ Record.Name = value; }
		}

        public int? RenterId {
            get { return _RenterField.Value; }
            set { _RenterField.Value = value; }
        }

		public CustomerPartRecord Renter{
            get { return Record.Renter; }
            set { Record.Renter = value; }
		}


		public ContractStatusOption? ContractStatus{
			get{ return Record.ContractStatus; }
			set{ Record.ContractStatus = value; }
		}

		public DateTime BeginDate{
			get{ return Record.BeginDate; }
			set{ Record.BeginDate = value; }
		}

		public DateTime EndDate{
			get{ return Record.EndDate; }
			set{ Record.EndDate = value; }
		}

		public string Description{
			get{ return Record.Description; }
			set{ Record.Description = value; }
		}
        public HouseStatusOption? HouseStatus {
            get { return Record.HouseStatus; }
            set { Record.HouseStatus = value; }
        }
    }
}
