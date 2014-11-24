using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace Coevery.PropertyManagement.Models.Mappings
{
    public class ServiceAutoMappingOverride : IAutoMappingOverride<ServicePartRecord>
    {
        public void Override(AutoMapping<ServicePartRecord> mapping)
        {
            mapping.References(x => x.House).Column("HouseId");
            mapping.References(x => x.Owner).Column("OwnerId");
            mapping.References(x => x.ServicePerson).Column("ServicePersonId");
        }
    }
}