using FluentNHibernate.Automapping.Alterations;

namespace Coevery.PropertyManagement.Models.Mappings
{
    public class HouseAutoMappingOverride : IAutoMappingOverride<HousePartRecord>
    {
        public void Override(FluentNHibernate.Automapping.AutoMapping<HousePartRecord> mapping)
        {
            mapping.HasMany(x => x.ChargeItems).KeyColumn("HouseId").Inverse();
            mapping.HasMany(x => x.MeterTypeItems).KeyColumn("HouseId").Inverse();
            mapping.References(x => x.Owner).Column("OwnerId");
            mapping.References(x => x.Officer).Column("OfficerId");
            mapping.References(x => x.Apartment).Column("ApartmentId");
            mapping.References(x => x.Building).Column("BuildingId");
        }
    }
}