using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Automapping.Alterations;

namespace Coevery.PropertyManagement.Models.Mappings {
    public class BillAutoMappingOverride : IAutoMappingOverride<BillRecord> {
        public void Override(FluentNHibernate.Automapping.AutoMapping<BillRecord> mapping) {
            mapping.References(x => x.House).Column("HouseId");
            mapping.References(x => x.Contract).Column("ContractId");
            mapping.References(x => x.ChargeItem).Column("ChargeItemSettingCommonId");
        }
    }
}