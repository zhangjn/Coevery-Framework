using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Automapping.Alterations;

namespace Coevery.PropertyManagement.Models.Mappings
{
    public class AdvancePaymentItemAutoMappingOverride : IAutoMappingOverride<AdvancePaymentItemRecord>
    {
        public void Override(FluentNHibernate.Automapping.AutoMapping<AdvancePaymentItemRecord> mapping)
        {
            mapping.References(x => x.Payment).Column("AdvancePaymentId");

        }
    }
}