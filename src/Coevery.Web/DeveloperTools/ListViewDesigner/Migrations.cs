﻿using Coevery.ContentManagement.MetaData;
using Coevery.Data.Migration;

namespace Coevery.DeveloperTools.ListViewDesigner {
    public class Migrations : DataMigrationImpl {
        public int Create() {
            SchemaBuilder.CreateTable("GridInfoPartRecord",
                table => table
                    .ContentPartVersionRecord()
                    .Column<string>("ItemContentType")
                    .Column<string>("DisplayName")
                );

            ContentDefinitionManager.AlterPartDefinition("GridInfoPart", cfg => { });
            ContentDefinitionManager.AlterTypeDefinition("GridInfo", cfg => cfg.WithPart("GridInfoPart"));
            return 1;
        }
    }
}