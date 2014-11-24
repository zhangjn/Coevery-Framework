using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Automapping.Alterations;

namespace Coevery.PropertyManagement.Models.Mappings
{
    public class AdvancePaymentAutoMappingOverride : IAutoMappingOverride<AdvancePaymentPartRecord>
    {
        public void Override(FluentNHibernate.Automapping.AutoMapping<AdvancePaymentPartRecord> mapping)
        {
            mapping.References(x => x.Operator).Column("Operator");
            mapping.References(x => x.Customer).Column("CustomerId");
            mapping.HasMany(x => x.AdvancePaymentItems).KeyColumn("AdvancePaymentId").Inverse();

        }
    }
}