
using System;
using System.Collections.Generic;
using Coevery.ContentManagement;

namespace Coevery.PropertyManagement.Models
{
    public sealed class MeterTypePart : ContentPart<MeterTypePartRecord> {

		public string Name{
			get{ return Record.Name; }
			set{ Record.Name = value; }
		}

        public string Unit
        {
            get { return Record.Unit; }
            set { Record.Unit = value; }
        }

		public string Description{
			get{ return Record.Description; }
			set{ Record.Description = value; }
		}
    }
}
