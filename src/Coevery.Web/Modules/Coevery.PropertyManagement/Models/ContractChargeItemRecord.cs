namespace Coevery.PropertyManagement.Models
{
    public class ContractChargeItemRecord : ChargeItemSettingCommonRecord {
        public virtual ContractPartRecord Contract { get; set; }
    }
}