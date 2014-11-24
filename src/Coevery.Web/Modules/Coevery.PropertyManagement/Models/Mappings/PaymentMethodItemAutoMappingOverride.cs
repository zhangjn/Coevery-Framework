using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Automapping.Alterations;

namespace Coevery.PropertyManagement.Models.Mappings
{
    public class PaymentMethodItemAutoMappingOverride : IAutoMappingOverride<PaymentMethodItemRecord>
    {
        public void Override(FluentNHibernate.Automapping.AutoMapping<PaymentMethodItemRecord> mapping) {
            mapping.References(x => x.Payment).Column("PaymentId");

        }
    }
}