
using System;
using System.Collections.Generic;
using Coevery.ContentManagement;
using Coevery.ContentManagement.Utilities;
using Coevery.Users.Models;

namespace Coevery.PropertyManagement.Models {
    public sealed class HousePart : ContentPart<HousePartRecord> {
        internal LazyField<int?> _ownerIdField = new LazyField<int?>();
        internal LazyField<int?> _officerIdField = new LazyField<int?>();
        internal LazyField<int?> _apartmentIdField = new LazyField<int?>();
        internal LazyField<int?> _buildingIdField = new LazyField<int?>();


        public int? BuildingId {
            get { return _buildingIdField.Value; }
            set { _buildingIdField.Value = value; }
        }

        public BuildingPartRecord Building{
			get{ return Record.Building; }
			set{ Record.Building = value; }
		}

        public int? ApartmentId
        {
            get { return _apartmentIdField.Value; }
            set { _apartmentIdField.Value = value; }
        }

		public ApartmentPartRecord Apartment {
			get{ return Record.Apartment; }
			set{ Record.Apartment = value; }
		}

        public int? OwnerId {
            get { return _ownerIdField.Value; }
            set { _ownerIdField.Value = value; }
        }

        public CustomerPartRecord Owner {
            get { return Record.Owner; }
            set { Record.Owner = value; }
        }

        public string HouseNumber{
			get{ return Record.HouseNumber; }
			set{ Record.HouseNumber = value; }
		}

		public int? OfficerId{
            get { return _officerIdField.Value; }
            set { _officerIdField.Value = value; }
		}

        public UserPartRecord Officer
        {
            get { return Record.Officer; }
            set { Record.Officer = value; }
        }

		public double? BuildingArea{
			get{ return Record.BuildingArea; }
			set{ Record.BuildingArea = value; }
		}

		public double? InsideArea{
			get{ return Record.InsideArea; }
			set{ Record.InsideArea = value; }
		}

		public double? PoolArea{
			get{ return Record.PoolArea; }
			set{ Record.PoolArea = value; }
		}

		public HouseStatusOption? HouseStatus{
			get{ return Record.HouseStatus; }
			set{ Record.HouseStatus = value; }
		}
    }
}
