

using FluentNHibernate.Automapping.Alterations;

namespace Coevery.PropertyManagement.Models.Mappings {
    public class ContractChargeItemAutoMappingOverride : IAutoMappingOverride<ContractChargeItemRecord> {
        public void Override(FluentNHibernate.Automapping.AutoMapping<ContractChargeItemRecord> mapping) {
            mapping.References(x => x.Contract).Column("ContractId");
        }
    }
}