using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Automapping.Alterations;

namespace Coevery.PropertyManagement.Models.Mappings {
    public class ChargeItemSettingCommonAutoMappingOverride : IAutoMappingOverride<ChargeItemSettingCommonRecord> {
        public void Override(FluentNHibernate.Automapping.AutoMapping<ChargeItemSettingCommonRecord> mapping) {
            mapping.References(x => x.ChargeItemSetting).Column("ChargeItemSettingId");
            mapping.DiscriminateSubClassesOnColumn("Type").AlwaysSelectWithValue();
            mapping.SubClass<ContractChargeItemRecord>("Contract");
            mapping.SubClass<HouseChargeItemRecord>("House");
        }
    }
}