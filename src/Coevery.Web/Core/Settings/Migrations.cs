﻿using Coevery.ContentManagement.MetaData;
using Coevery.ContentManagement.Records;
using Coevery.Core.Settings.Metadata.Records;
using Coevery.Data;
using Coevery.Data.Migration;

namespace Coevery.Core.Settings {
    public class Migrations : DataMigrationImpl {
        private readonly IRepository<ContentTypeDefinitionRecord> _typeDefinitionRepository;
        private readonly IRepository<ContentPartDefinitionRecord> _partDefinitionRepository;
        private readonly IRepository<ContentTypeRecord> _contentTypeRepository;
        private readonly IRepository<ContentItemRecord> _contentItemRepository;
        private readonly IRepository<ContentItemVersionRecord> _contentItemVersionRepository;

        public Migrations(
            IRepository<ContentTypeDefinitionRecord> typeDefinitionRepository,
            IRepository<ContentPartDefinitionRecord> partDefinitionRepository,
            IRepository<ContentTypeRecord> contentTypeRepository,
            IRepository<ContentItemRecord> contentItemRepository,
            IRepository<ContentItemVersionRecord> contentItemVersionRepository) {
            _typeDefinitionRepository = typeDefinitionRepository;
            _partDefinitionRepository = partDefinitionRepository;
            _contentTypeRepository = contentTypeRepository;
            _contentItemRepository = contentItemRepository;
            _contentItemVersionRepository = contentItemVersionRepository;
        }

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
                    .Column<bool>("Hidden")
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
                    .Column<bool>("Hidden")
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

            return 4;
        }

        public int UpdateFrom1() {
            SchemaBuilder.CreateTable("SiteSettings2PartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<string>("BaseUrl", c => c.Unlimited())
                );

            return 2;
        }

        public int UpdateFrom2() {
            SchemaBuilder.AlterTable("SiteSettingsPartRecord",
                table => table
                    .AddColumn<string>("SiteTimeZone")
                );

            return 3;
        }

        public int UpdateFrom3() {
            ContentDefinitionManager.AlterTypeDefinition("Site", cfg => { });

            return 4;            
        }
    }
}