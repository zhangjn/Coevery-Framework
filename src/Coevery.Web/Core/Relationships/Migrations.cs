using Coevery.ContentManagement;
using Coevery.ContentManagement.MetaData;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.Core.Relationships.Settings;
using Coevery.Data.Migration;

namespace Coevery.Core.Relationships {
    public class Migrations : DataMigrationImpl {
        private readonly IContentDefinitionQuery _contentDefinitionQuery;
        public Migrations(IContentDefinitionQuery contentDefinitionQuery) {
            _contentDefinitionQuery = contentDefinitionQuery;
        }

        public int Create() {
            SchemaBuilder.CreateTable("RelationshipRecord",
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<string>("Name")
                    .Column<byte>("Type", column => column.NotNull())
                    .Column<int>("PrimaryEntity_Id", column => column.NotNull())
                    .Column<int>("RelatedEntity_Id", column => column.NotNull())
                );

            SchemaBuilder.CreateTable("OneToManyRelationshipRecord",
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<int>("Relationship_Id", column => column.Unique())
                    .Column<int>("LookupField_Id", column => column.Nullable())
                    .Column<int>("RelatedListProjection_Id", column => column.Nullable())
                    .Column<string>("RelatedListLabel", column => column.Nullable())
                    .Column<bool>("ShowRelatedList", column => column.NotNull())
                    .Column<byte>("DeleteOption", column => column.Nullable())
                );

            SchemaBuilder.CreateTable("ManyToManyRelationshipRecord",
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<int>("Relationship_Id", column => column.Unique())
                    .Column<int>("RelatedListProjection_Id", column => column.Nullable())
                    .Column<string>("RelatedListLabel", column => column.Nullable())
                    .Column<bool>("ShowRelatedList", column => column.NotNull())
                    .Column<int>("PrimaryListProjection_Id", column => column.Nullable())
                    .Column<string>("PrimaryListLabel", column => column.Nullable())
                    .Column<bool>("ShowPrimaryList", column => column.NotNull())
                );



            return 1;
        }

        public int UpdateFrom1() {
            var parts = _contentDefinitionQuery.ListPartDefinitions();
            foreach (var part in parts) {
                ContentPartDefinition partDefinition = part;
                ContentDefinitionManager.AlterPartDefinition(part.Name, VersionOptions.Published, cfg => {
                    foreach (var field in partDefinition.Fields) {
                        var settings = field.Settings.GetModel<ReferenceFieldSettings>();
                        if (settings != null) {
                            field.Settings["ReferenceFieldSettings.DeleteAction"] = DeleteAction.NotAllowed.ToString();
                        }
                    }
                });
            }
            return 2;
        }
    }
}