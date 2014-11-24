
using System;
using System.Collections.Generic;
using Coevery.ContentManagement;

namespace Coevery.PropertyManagement.Models {
    public sealed class BuildingPart : ContentPart<BuildingPartRecord> {

		public int Apartment{
			get{ return Record.Apartment; }
			set{ Record.Apartment = value; }
		}

		public string Name{
			get{ return Record.Name; }
			set{ Record.Name = value; }
		}

		public string Description{
			get{ return Record.Description; }
			set{ Record.Description = value; }
		}
    }
}
