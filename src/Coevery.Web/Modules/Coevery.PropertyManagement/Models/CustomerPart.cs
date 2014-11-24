
using System;
using System.Collections.Generic;
using Coevery.ContentManagement;

namespace Coevery.PropertyManagement.Models {
    public sealed class CustomerPart : ContentPart<CustomerPartRecord> {

		public string Number{
			get{ return Record.Number; }
			set{ Record.Number = value; }
		}

		public string Name{
			get{ return Record.Name; }
			set{ Record.Name = value; }
		}

		public CustomerTypeOption? CustomerType{
			get{ return Record.CustomerType; }
			set{ Record.CustomerType = value; }
		}

		public string Phone{
			get{ return Record.Phone; }
			set{ Record.Phone = value; }
		}

		public string Description{
			get{ return Record.Description; }
			set{ Record.Description = value; }
		}
        public decimal CustomerBalance
        {
            get { return Record.CustomerBalance; }
            set { Record.CustomerBalance = value; }
		}
        
    }
}
