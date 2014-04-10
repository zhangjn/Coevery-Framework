using Coevery.Data.Migration;

namespace Coevery.ContentManagement.DataMigrations {
    public class FrameworkDataMigration : DataMigrationImpl {

        public int Create() {
            SchemaBuilder.CreateTable("Framework_ContentItemRecord", 
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<string>("Data", c => c.Unlimited())
                    .Column<int>("ContentType_id")
                );

            SchemaBuilder.CreateTable("Framework_ContentItemVersionRecord", 
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<int>("Number")
                    .Column<bool>("Published")
                    .Column<bool>("Latest")
                    .Column<string>("Data", c => c.Unlimited())
                    .Column<int>("ContentItemRecord_id", c => c.NotNull())
                );

            SchemaBuilder.CreateTable("Framework_ContentTypeRecord", 
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<string>("Name")
                );

            SchemaBuilder.CreateTable("Framework_CultureRecord", 
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<string>("Culture")
                );

            return 1;
        }

        public int UpdateFrom1() {
            SchemaBuilder.AlterTable("Framework_ContentItemRecord",
               table => table
                   .CreateIndex("IDX_ContentType_id", "ContentType_id")
               );

            SchemaBuilder.AlterTable("Framework_ContentItemVersionRecord",
                table => table
                    .CreateIndex("IDX_ContentItemRecord_id", "ContentItemRecord_id")
                );

            return 2;
        }

    }
}