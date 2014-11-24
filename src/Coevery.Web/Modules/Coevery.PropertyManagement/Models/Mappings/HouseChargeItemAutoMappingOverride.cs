
using FluentNHibernate.Automapping.Alterations;


namespace Coevery.PropertyManagement.Models.Mappings {
    public class HouseChargeItemAutoMappingOverride : IAutoMappingOverride<HouseChargeItemRecord> {
        public void Override(FluentNHibernate.Automapping.AutoMapping<HouseChargeItemRecord> mapping) {
            mapping.References(x => x.House).Column("HouseId");
        }
    }
}