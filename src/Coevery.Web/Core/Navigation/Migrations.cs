using Coevery.ContentManagement.MetaData;
using Coevery.Data.Migration;

namespace Coevery.Core.Navigation {
    public class Migrations : DataMigrationImpl {

        public int Create() {
            ContentDefinitionManager.AlterPartDefinition("MenuPart", builder => { });

            SchemaBuilder.CreateTable("MenuItemPartRecord", 
                table => table
                    .ContentPartRecord()
                    .Column<string>("Url", column => column.WithLength(1024))
                    .Column<string>("IconClass")
                );

            SchemaBuilder.CreateTable("MenuPartRecord", 
                table => table
                    .ContentPartRecord()
                    .Column<string>("Description", column => column.Unlimited())
                    .Column<string>("Name")
                    .Column<string>("MenuText")
                    .Column<string>("MenuPosition")
                    .Column<int>("MenuId")
                );

            SchemaBuilder.CreateTable("ModuleMenuItemPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<int>("ContentTypeDefinitionRecord_id")
                );

            ContentDefinitionManager.AlterTypeDefinition("ModuleMenuItem", cfg => cfg
                .WithPart("MenuPart")
                .WithPart("CommonPart")
                .WithPart("IdentityPart")
                .WithPart("ModuleMenuItemPart")
                .DisplayedAs("Module Menu Item")
                .WithSetting("Description", "Adds a Module Menu Item to navigation")
                .WithSetting("Stereotype", "MenuItem")
                );

            ContentDefinitionManager.AlterTypeDefinition("MenuItem", cfg => cfg
                .WithPart("MenuPart")
                .WithPart("IdentityPart")
                .WithPart("CommonPart")
                .DisplayedAs("Custom Link")
                .WithSetting("Description", "Represents a simple custom link with a text and an url.")
                .WithSetting("Stereotype", "MenuItem") // because we declare a new stereotype, the Shape MenuItem_Edit is needed
                );

            ContentDefinitionManager.AlterTypeDefinition("Menu", cfg => cfg
                .WithPart("CommonPart", p => p.WithSetting("OwnerEditorSettings.ShowOwnerEditor", "false"))
                .WithPart("TitlePart")
                );

            SchemaBuilder.CreateTable("MenuWidgetPartRecord", table => table
                .ContentPartRecord()
                .Column<int>("StartLevel")
                .Column<int>("Levels")
                .Column<bool>("Breadcrumb")
                .Column<bool>("AddHomePage")
                .Column<bool>("AddCurrentPage")
                .Column<int>("Menu_id")
                );

            ContentDefinitionManager.AlterTypeDefinition("MenuWidget", cfg => cfg
                .WithPart("CommonPart")
                .WithPart("IdentityPart")
                .WithPart("WidgetPart")
                .WithPart("MenuWidgetPart")
                .WithSetting("Stereotype", "Widget")
                );

            SchemaBuilder.CreateTable("AdminMenuPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<string>("AdminMenuText")
                    .Column<string>("AdminMenuPosition")
                    .Column<bool>("OnAdminMenu")
                );

            ContentDefinitionManager.AlterTypeDefinition("HtmlMenuItem", cfg => cfg
                .WithPart("MenuPart")
                .WithPart("BodyPart")
                .WithPart("CommonPart")
                .WithPart("IdentityPart")
                .DisplayedAs("Html Menu Item")
                .WithSetting("Description", "Renders some custom HTML in the menu.")
                .WithSetting("BodyPartSettings.FlavorDefault", "html")
                .WithSetting("Stereotype", "MenuItem")
                );
            
            ContentDefinitionManager.AlterPartDefinition("AdminMenuPart", builder => { });

            SchemaBuilder.CreateTable("ShapeMenuItemPartRecord",
                table => table.ContentPartRecord()
                    .Column<string>("ShapeType")
                );

            ContentDefinitionManager.AlterTypeDefinition("ShapeMenuItem",
                cfg => cfg
                    .WithPart("ShapeMenuItemPart")
                    .WithPart("MenuPart")
                    .WithPart("CommonPart")
                    .DisplayedAs("Shape Link")
                    .WithSetting("Description", "Injects menu items from a Shape")
                    .WithSetting("Stereotype", "MenuItem")
                );

            return 4;
        }

        public int UpdateFrom1() {
            SchemaBuilder.CreateTable("AdminMenuPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<string>("AdminMenuText")
                    .Column<string>("AdminMenuPosition")
                    .Column<bool>("OnAdminMenu")
                );
            ContentDefinitionManager.AlterPartDefinition("AdminMenuPart", builder => { });
            return 2;
        }

        public int UpdateFrom2() {
            ContentDefinitionManager.AlterTypeDefinition("MenuItem", cfg => cfg
                .WithPart("MenuPart")
                .WithPart("CommonPart")
                .WithPart("IdentityPart")
                .DisplayedAs("Custom Link")
                .WithSetting("Description", "Represents a simple custom link with a text and an url.")
                .WithSetting("Stereotype", "MenuItem") // because we declare a new stereotype, the Shape MenuItem_Edit is needed
                );

            ContentDefinitionManager.AlterTypeDefinition("Menu", cfg => cfg
                .WithPart("CommonPart")
                .WithPart("TitlePart")
                );

            SchemaBuilder.CreateTable("MenuWidgetPartRecord",table => table
                .ContentPartRecord()
                .Column<int>("StartLevel")
                .Column<int>("Levels")
                .Column<bool>("Breadcrumb")
                .Column<bool>("AddHomePage")
                .Column<bool>("AddCurrentPage")
                .Column<int>("Menu_id")
                );

            ContentDefinitionManager.AlterTypeDefinition("MenuWidget", cfg => cfg
                .WithPart("CommonPart")
                .WithPart("IdentityPart")
                .WithPart("WidgetPart")
                .WithPart("MenuWidgetPart")
                .WithSetting("Stereotype", "Widget")
                );

            SchemaBuilder
                .AlterTable("MenuPartRecord", table => table.DropColumn("OnMainMenu"))
                .AlterTable("MenuPartRecord", table => table.AddColumn<int>("MenuId"))
                ;

            ContentDefinitionManager.AlterTypeDefinition("HtmlMenuItem", cfg => cfg
                .WithPart("MenuPart")
                .WithPart("BodyPart")
                .WithPart("CommonPart")
                .WithPart("IdentityPart")
                .DisplayedAs("Html Menu Item")
                .WithSetting("Description", "Renders some custom HTML in the menu.")
                .WithSetting("BodyPartSettings.FlavorDefault", "html")
                .WithSetting("Stereotype", "MenuItem")
               );

            return 3;
        }

        public int UpdateFrom3() {
            SchemaBuilder.CreateTable("ShapeMenuItemPartRecord",
                table => table.ContentPartRecord()
                    .Column<string>("ShapeType")
                );

            ContentDefinitionManager.AlterTypeDefinition("ShapeMenuItem",
                cfg => cfg
                    .WithPart("ShapeMenuItemPart")
                    .WithPart("MenuPart")
                    .WithPart("CommonPart")
                    .DisplayedAs("Shape Link")
                    .WithSetting("Description", "Injects menu items from a Shape")
                    .WithSetting("Stereotype", "MenuItem")
                );

            return 4;
        }

        public int UpdateFrom4() {
            ContentDefinitionManager.AlterPartDefinition("MenuPart", builder => { });

            ContentDefinitionManager.AlterPartDefinition("AdminMenuPart", builder => { });

            return 5;
        }

        public int UpdateFrom5() {
            ContentDefinitionManager.AlterTypeDefinition("Menu", cfg => cfg
                .WithPart("IdentityPart")
            );

            return 6;
        }

        public int UpdateFrom6()
        {
            ContentDefinitionManager.AlterTypeDefinition("Menu", cfg => cfg
                .WithPart("PerspectivePart")
            );

            ContentDefinitionManager.AlterTypeDefinition("ModuleMenuItem", cfg => cfg
                .DisplayedAs("Entity Navigation"));
            return 7;
        }
    }
}