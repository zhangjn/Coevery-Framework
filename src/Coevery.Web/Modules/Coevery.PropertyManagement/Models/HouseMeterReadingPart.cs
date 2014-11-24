
using System;
using System.Collections.Generic;
using Coevery.ContentManagement;
using Coevery.ContentManagement.Utilities;

namespace Coevery.PropertyManagement.Models {
    public sealed class HouseMeterReadingPart : ContentPart<HouseMeterReadingPartRecord> {

        internal LazyField<int?> _houseMeterIdField = new LazyField<int?>();

        public int Year {
            get { return Record.Year; }
            set { Record.Year = value; }
        }

        public int Month {
            get { return Record.Month; }
            set { Record.Month = value; }
        }

        public int? HouseMeterId {
            get { return _houseMeterIdField.Value; }
            set { _houseMeterIdField.Value = value; }
        }

        public HouseMeterRecord HouseMeter {
            get { return Record.HouseMeter; }
            set { Record.HouseMeter = value; }
        }

        public double? MeterData {
            get { return Record.MeterData; }
            set { Record.MeterData = value; }
        }

        public double? Amount {
            get { return Record.Amount; }
            set { Record.Amount = value; }
        }

        public StatusOption? Status {
            get { return Record.Status; }
            set { Record.Status = value; }
        }

        public string Remarks {
            get { return Record.Remarks; }
            set { Record.Remarks = value; }
        }
    }
}
