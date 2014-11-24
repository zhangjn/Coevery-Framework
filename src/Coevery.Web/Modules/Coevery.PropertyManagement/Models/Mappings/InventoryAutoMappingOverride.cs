using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace Coevery.PropertyManagement.Models.Mappings
{
    public class InventoryAutoMappingOverride: IAutoMappingOverride<VoucherPartRecord>
    {
        public void Override(AutoMapping<VoucherPartRecord> mapping)
        {
       //     mapping.References(x => x.Supplier).Column("SupplierId");
        }
    }
}