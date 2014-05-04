﻿using Coevery.ContentManagement.MetaData;
using Coevery.Data.Migration;

namespace Coevery.Core.Settings {
    public class Migrations : DataMigrationImpl {
        public int Create() {
            SchemaBuilder.CreateTable("ContentFieldDefinitionRecord",
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<string>("Name")
                );

            SchemaBuilder.CreateTable("ContentPartDefinitionRecord",
                table => table
                    .ContentPartVersionRecord()
                    .Column<string>("Name")
                    .Column<string>("Settings", column => column.Unlimited())
                );

            SchemaBuilder.CreateTable("ContentPartFieldDefinitionRecord",
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<string>("Name")
                    .Column<string>("Settings", column => column.Unlimited())
                    .Column<int>("ContentFieldDefinitionRecord_id")
                    .Column<int>("ContentPartDefinitionRecord_Id")
                );

            SchemaBuilder.CreateTable("ContentTypeDefinitionRecord",
                table => table
                    .ContentPartVersionRecord()
                    .Column<string>("Name")
                    .Column<string>("DisplayName")
                    .Column<bool>("Customized")
                    .Column<string>("Settings", column => column.Unlimited())
                );

            SchemaBuilder.CreateTable("ContentTypePartDefinitionRecord",
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<string>("Settings", column => column.Unlimited())
                    .Column<int>("ContentPartDefinitionRecord_id")
                    .Column<int>("ContentTypeDefinitionRecord_Id")
                );

            SchemaBuilder.CreateTable("ShellDescriptorRecord",
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<int>("SerialNumber")
                );

            SchemaBuilder.CreateTable("ShellFeatureRecord",
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<string>("Name")
                    .Column<int>("ShellDescriptorRecord_id"));

            SchemaBuilder.CreateTable("ShellFeatureStateRecord",
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<string>("Name")
                    .Column<string>("InstallState")
                    .Column<string>("EnableState")
                    .Column<int>("ShellStateRecord_Id")
                );

            SchemaBuilder.CreateTable("ShellParameterRecord",
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<string>("Component")
                    .Column<string>("Name")
                    .Column<string>("Value")
                    .Column<int>("ShellDescriptorRecord_id")
                );

            SchemaBuilder.CreateTable("ShellStateRecord",
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<string>("Unused")
                );

            // declare the Site content type to let users alter it
            ContentDefinitionManager.AlterTypeDefinition("Site", cfg => { });

            return 1;
        }
    }
}