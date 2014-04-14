﻿using System.Data;
using Coevery.ContentManagement.MetaData;
using Coevery.Core.Forms.Services;
using Coevery.Core.Projections.Models;
using Coevery.Core.Projections.Services;
using Coevery.Data;
using Coevery.Data.Migration;
using Coevery.Environment.Configuration;
using Coevery.Localization;
using NHibernate.Dialect;

namespace Coevery.Core.Projections {
    public class Migrations : DataMigrationImpl {
        private readonly ShellSettings _shellSettings;
        private readonly Dialect _dialect;
        private readonly IRepository<MemberBindingRecord> _memberBindingRepository;
        private readonly IRepository<LayoutRecord> _layoutRepository;

        public Migrations(ISessionFactoryHolder sessionFactoryHolder,
            ShellSettings shellSettings,
            IRepository<MemberBindingRecord> memberBindingRepository,
            IRepository<LayoutRecord> layoutRepository) {
            _shellSettings = shellSettings;
            var configuration = sessionFactoryHolder.GetConfiguration();
            _dialect = Dialect.GetDialect(configuration.Properties);
            _memberBindingRepository = memberBindingRepository;
            _layoutRepository = layoutRepository;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public int Create() {
            // Properties index

            SchemaBuilder.CreateTable("StringFieldIndexRecord",
                table => table
                    .Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<string>("PropertyName")
                    .Column<string>("Value", c => c.WithLength(4000))
                    .Column<int>("FieldIndexPartRecord_Id")
                );

            SchemaBuilder.CreateTable("IntegerFieldIndexRecord",
                table => table
                    .Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<string>("PropertyName")
                    .Column<long>("Value")
                    .Column<int>("FieldIndexPartRecord_Id")
                );

            SchemaBuilder.CreateTable("DoubleFieldIndexRecord",
                table => table
                    .Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<string>("PropertyName")
                    .Column<double>("Value")
                    .Column<int>("FieldIndexPartRecord_Id")
                );

            SchemaBuilder.CreateTable("DecimalFieldIndexRecord",
                table => table
                    .Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<string>("PropertyName")
                    .Column<decimal>("Value")
                    .Column<int>("FieldIndexPartRecord_Id")
                );

            SchemaBuilder.CreateTable("FieldIndexPartRecord", table => table.ContentPartRecord());

            // Query

            ContentDefinitionManager.AlterTypeDefinition("Query",
                cfg => cfg
                    .WithPart("QueryPart")
                    .WithPart("TitlePart")
                    .WithPart("IdentityPart")
                );

            SchemaBuilder.CreateTable("QueryPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<string>("Name", column => column.WithLength(1024))
                );

            SchemaBuilder.CreateTable("FilterGroupRecord",
                table => table
                    .Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<int>("QueryPartRecord_id")
                );

            SchemaBuilder.CreateTable("FilterRecord",
                table => table
                    .Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<string>("Category", c => c.WithLength(64))
                    .Column<string>("Type", c => c.WithLength(64))
                    .Column<string>("Description", c => c.WithLength(255))
                    .Column<string>("State", c => c.Unlimited())
                    .Column<int>("Position")
                    .Column<int>("FilterGroupRecord_id")
                );

            SchemaBuilder.CreateTable("SortCriterionRecord",
                table => table
                    .Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<string>("Category", c => c.WithLength(64))
                    .Column<string>("Type", c => c.WithLength(64))
                    .Column<string>("Description", c => c.WithLength(255))
                    .Column<string>("State", c => c.Unlimited())
                    .Column<int>("Position")
                    .Column<int>("QueryPartRecord_id")
                );

            SchemaBuilder.CreateTable("LayoutRecord",
                table => table
                    .Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<string>("Category", c => c.WithLength(64))
                    .Column<string>("Type", c => c.WithLength(64))
                    .Column<string>("Description", c => c.WithLength(255))
                    .Column<string>("State", c => c.Unlimited())
                    .Column<string>("DisplayType", c => c.WithLength(64))
                    .Column<int>("Display")
                    .Column<int>("QueryPartRecord_id")
                    .Column<int>("GroupProperty_id")
                );

            SchemaBuilder.CreateTable("PropertyRecord",
                table => table
                    .Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<string>("Category", c => c.WithLength(64))
                    .Column<string>("Type", c => c.WithLength(64))
                    .Column<string>("Description", c => c.WithLength(255))
                    .Column<string>("State", c => c.Unlimited())
                    .Column<int>("Position")
                    .Column<int>("LayoutRecord_id")
                    .Column<bool>("ExcludeFromDisplay")
                    .Column<bool>("CreateLabel")
                    .Column<string>("Label", c => c.WithLength(255))
                    .Column<bool>("LinkToContent")
                    .Column<bool>("CustomizePropertyHtml")
                    .Column<string>("CustomPropertyTag", c => c.WithLength(64))
                    .Column<string>("CustomPropertyCss", c => c.WithLength(64))
                    .Column<bool>("CustomizeLabelHtml")
                    .Column<string>("CustomLabelTag", c => c.WithLength(64))
                    .Column<string>("CustomLabelCss", c => c.WithLength(64))
                    .Column<bool>("CustomizeWrapperHtml")
                    .Column<string>("CustomWrapperTag", c => c.WithLength(64))
                    .Column<string>("CustomWrapperCss", c => c.WithLength(64))
                    .Column<string>("NoResultText", c => c.Unlimited())
                    .Column<bool>("ZeroIsEmpty")
                    .Column<bool>("HideEmpty")
                    .Column<bool>("RewriteOutput")
                    .Column<string>("RewriteText", c => c.Unlimited())
                    .Column<bool>("StripHtmlTags")
                    .Column<bool>("TrimLength")
                    .Column<bool>("AddEllipsis")
                    .Column<int>("MaxLength")
                    .Column<bool>("TrimOnWordBoundary")
                    .Column<bool>("PreserveLines")
                    .Column<bool>("TrimWhiteSpace")
                );

            SchemaBuilder.CreateTable("ProjectionPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<int>("Items")
                    .Column<int>("ItemsPerPage")
                    .Column<int>("Skip")
                    .Column<string>("PagerSuffix", c => c.WithLength(255))
                    .Column<int>("MaxItems")
                    .Column<bool>("DisplayPager")
                    .Column<int>("QueryPartRecord_id")
                    .Column<int>("LayoutRecord_Id")
                );

            SchemaBuilder.CreateTable("MemberBindingRecord",
                table => table
                    .Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<string>("Type", c => c.WithLength(255))
                    .Column<string>("Member", c => c.WithLength(64))
                    .Column<string>("Description", c => c.WithLength(500))
                    .Column<string>("DisplayName", c => c.WithLength(64))
                );

            ContentDefinitionManager.AlterTypeDefinition("ProjectionWidget",
                cfg => cfg
                    .WithPart("WidgetPart")
                    .WithPart("CommonPart")
                    .WithPart("IdentityPart")
                    .WithPart("ProjectionPart")
                    .WithSetting("Stereotype", "Widget")
                );

            ContentDefinitionManager.AlterTypeDefinition("ProjectionPage",
                cfg => cfg
                    .WithPart("CommonPart")
                    .WithPart("TitlePart")
                    .WithPart("AutoroutePart", builder => builder
                        .WithSetting("AutorouteSettings.AllowCustomPattern", "true")
                        .WithSetting("AutorouteSettings.AutomaticAdjustmentOnEdit", "false")
                        .WithSetting("AutorouteSettings.PatternDefinitions", "[{Name:'Title', Pattern: '{Content.Slug}', Description: 'my-projections'}]")
                        .WithSetting("AutorouteSettings.DefaultPatternIndex", "0"))
                    .WithPart("MenuPart")
                    .WithPart("ProjectionPart")
                    .WithPart("AdminMenuPart", p => p.WithSetting("AdminMenuPartTypeSettings.DefaultPosition", "5"))
                    .DisplayedAs("Projection")
                );

            //// Default Model Bindings - CommonPartRecord

            //_memberBindingRepository.Create(new MemberBindingRecord {
            //    Type = typeof(CommonPartRecord).FullName,
            //    Member = "CreatedUtc",
            //    DisplayName = T("Creation date").Text,
            //    Description = T("When the content item was created").Text
            //});

            //_memberBindingRepository.Create(new MemberBindingRecord {
            //    Type = typeof(CommonPartRecord).FullName,
            //    Member = "ModifiedUtc",
            //    DisplayName = T("Modification date").Text,
            //    Description = T("When the content item was modified").Text
            //});

            //_memberBindingRepository.Create(new MemberBindingRecord {
            //    Type = typeof(CommonPartRecord).FullName,
            //    Member = "PublishedUtc",
            //    DisplayName = T("Publication date").Text,
            //    Description = T("When the content item was published").Text
            //});

            //// Default Model Bindings - TitlePartRecord

            //_memberBindingRepository.Create(new MemberBindingRecord {
            //    Type = typeof(TitlePartRecord).FullName,
            //    Member = "Title",
            //    DisplayName = T("Title Part Title").Text,
            //    Description = T("The title assigned using the Title part").Text
            //});

            //// Default Model Bindings - BodyPartRecord

            //_memberBindingRepository.Create(new MemberBindingRecord {
            //    Type = typeof(BodyPartRecord).FullName,
            //    Member = "Text",
            //    DisplayName = T("Body Part Text").Text,
            //    Description = T("The text from the Body part").Text
            //});

            return 1;
        }

        public int UpdateFrom1() {
            SchemaBuilder.CreateTable("NavigationQueryPartRecord",
                table => table.ContentPartRecord()
                    .Column<int>("Items")
                    .Column<int>("Skip")
                    .Column<int>("QueryPartRecord_id")
                );

            ContentDefinitionManager.AlterTypeDefinition("NavigationQueryMenuItem",
                cfg => cfg
                    .WithPart("NavigationQueryPart")
                    .WithPart("MenuPart")
                    .WithPart("CommonPart")
                    .DisplayedAs("Query Link")
                    .WithSetting("Description", "Injects menu items from a Query")
                    .WithSetting("Stereotype", "MenuItem")
                );

            SchemaBuilder.CreateTable("ListViewPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<string>("Name", column => column.WithLength(1024))
                    .Column<string>("ItemContentType")
                );

            ContentDefinitionManager.AlterTypeDefinition("ListViewPage",
                cfg => cfg
                    .WithPart("ListViewPart")
                    .WithPart("ProjectionPart")
                    .DisplayedAs("List View"));

            return 2;
        }

        private string DataTablePrefix() {
            if (string.IsNullOrEmpty(_shellSettings.DataTablePrefix)) {
                return string.Empty;
            }
            return _shellSettings.DataTablePrefix + "_";
        }

        public int UpdateFrom2() {
            SchemaBuilder.AlterTable("ProjectionPartRecord",
                table => table
                    .AlterColumn("PagerSuffix", c => c.WithType(DbType.String).WithLength(255))
                );

            SchemaBuilder.AlterTable("ListViewPartRecord",
                table => table
                    .AddColumn<bool>("IsDefault", column => column.WithDefault(false)));

            ContentDefinitionManager.AlterTypeDefinition("ListViewPage",
                cfg => cfg
                    .WithPart("TitlePart"));

            return 3;
        }

        public int UpdateFrom3() {
            SchemaBuilder.CreateTable("EntityFilterRecord",
                table => table
                    .Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<string>("EntityName")
                    .Column<string>("Title")
                    .Column<int>("FilterGroupRecord_id")
                );
            return 4;
        }

        public int UpdateFrom4() {
            var layouts = _layoutRepository.Fetch(record => record.Type == "ngGrid");
            foreach (var layoutRecord in layouts) {
                var state = FormParametersHelper.FromString(layoutRecord.State);
                if (!state.ContainsKey("PageRowCount")) {
                    state["PageRowCount"] = "50";
                }
                if (!state.ContainsKey("SortedBy")) {
                    state["SortedBy"] = string.Empty;
                }
                if (!state.ContainsKey("SortMode")) {
                    state["SortMode"] = string.Empty;
                }

                layoutRecord.Category = ProjectionService.DefaultLayoutCategory;
                layoutRecord.Type = ProjectionService.DefaultLayoutType;
                layoutRecord.State = FormParametersHelper.ToString(state);
                _layoutRepository.Update(layoutRecord);
            }
            return 5;
        }

        public int UpdateFrom5() {
            SchemaBuilder.CreateTable("LayoutGroupRecord",
                table => table
                    .Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<int>("Position")
                    .Column<string>("Sort")
                    .Column<int>("LayoutRecord_id")
                    .Column<int>("GroupProperty_id")
                );
            return 6;
        }

        public int UpdateFrom6() {
            SchemaBuilder.AlterTable("PropertyRecord",
                table => table.AlterColumn("Type", col => col.WithType(DbType.String).WithLength(1023)));
            return 7;
        }
    }
}