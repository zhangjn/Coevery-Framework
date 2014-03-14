using Coevery.ContentManagement.MetaData;
using Coevery.Data.Migration;

namespace Coevery.Core.OptionSet {
    public class Migrations : DataMigrationImpl {
        public int Create() {
            SchemaBuilder.CreateTable("OptionSetPartRecord", table => table
                .ContentPartRecord()
                .Column<string>("TermTypeName", column => column.WithLength(255))
                .Column<string>("Name", column => column.WithLength(1024))
                .Column<bool>("IsInternal")
                );

            SchemaBuilder.CreateTable("OptionItemPartRecord", table => table
                .ContentPartRecord()
                .Column<int>("OptionSetId")
                .Column<string>("Name", column => column.WithLength(1024))
                .Column<int>("Weight")
                .Column<bool>("Selectable")
                );

            SchemaBuilder.CreateTable("OptionItemContentItem", table => table
                .Column<int>("Id", column => column.PrimaryKey().Identity())
                .Column<string>("Field", column => column.WithLength(250))
                .Column<int>("OptionItemRecord_id")
                .Column<int>("OptionItemContainerPartRecord_id")
                );

            SchemaBuilder.CreateTable("OptionItemContainerPartRecord", table => table
                .ContentPartRecord()
                .Column<int>("ContentItemRecord_id")
                );

            ContentDefinitionManager.AlterTypeDefinition("OptionSet", cfg => cfg
                .WithPart("OptionSetPart")
                );

            ContentDefinitionManager.AlterTypeDefinition("OptionItem", cfg => cfg
                .WithPart("OptionItemPart")
                );

            return 1;
        }
    }
}