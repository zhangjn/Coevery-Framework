using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Automapping.Alterations;

namespace Coevery.PropertyManagement.Models.Mappings
{
    public class PaymentAutoMappingOverride : IAutoMappingOverride<PaymentPartRecord>
    {
        public void Override(FluentNHibernate.Automapping.AutoMapping<PaymentPartRecord> mapping)
        {
            mapping.HasMany(x => x.LineItems).KeyColumn("PaymentId").Inverse();
            mapping.References(x => x.Operator).Column("Operator");
            mapping.References(x => x.CustomerId).Column("CustomerId");

            mapping.HasMany(x => x.PaymentMethodItems).KeyColumn("PaymentId").Inverse();

        }
    }
}