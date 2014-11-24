using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevery.PropertyManagement.Models.Mappings
{
    public class HouseMeterReadingAutoMappingOverride : IAutoMappingOverride<HouseMeterReadingPartRecord>
    {
        public void Override(AutoMapping<HouseMeterReadingPartRecord> mapping)
        {
            mapping.References(x => x.HouseMeter).Column("HouseMeterId");
        }
    }
}