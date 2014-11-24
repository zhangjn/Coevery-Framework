using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Automapping.Alterations;

namespace Coevery.PropertyManagement.Models.Mappings
{
    public class PaymentLineItemAutoMappingOverride : IAutoMappingOverride<PaymentLineItemPartRecord>
    {
        public void Override(FluentNHibernate.Automapping.AutoMapping<PaymentLineItemPartRecord> mapping)
        {
            mapping.References(x => x.Bill).Column("BillId");
            mapping.References(x => x.Payment).Column("PaymentId");
           
        }
    }
}