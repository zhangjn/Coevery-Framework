
using FluentNHibernate.Automapping.Alterations;

namespace Coevery.PropertyManagement.Models.Mappings
{
    public class HouseMeterTypeItemAutoMappingOverride : IAutoMappingOverride<HouseMeterRecord>
    {
        public void Override(FluentNHibernate.Automapping.AutoMapping<HouseMeterRecord> mapping) {
            mapping.References(x => x.MeterType).Column("MeterTypeId");
            mapping.References(x => x.House).Column("HouseId");

            mapping.HasMany(x => x.MeterReadings).KeyColumn("HouseMeterId").Inverse();
        }
    }
}