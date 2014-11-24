namespace Coevery.PropertyManagement.Models {
    public class HouseChargeItemRecord : ChargeItemSettingCommonRecord {
        public virtual HousePartRecord House { get; set; }
    }
}