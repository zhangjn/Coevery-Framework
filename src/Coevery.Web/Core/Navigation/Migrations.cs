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
                    .Column<string>("IconClass")
                );

            ContentDefinitionManager.AlterTypeDefinition("ModuleMenuItem", cfg => cfg
                .WithPart("MenuPart")
                .WithPart("ModuleMenuItemPart")
                .DisplayedAs("Entity Navigation")
                .WithSetting("Description", "Adds a Module Menu Item to navigation")
                .WithSetting("Stereotype", "MenuItem")
                );

            ContentDefinitionManager.AlterTypeDefinition("MenuItem", cfg => cfg
                .WithPart("MenuPart")
                .DisplayedAs("Custom Link")
                .WithSetting("Description", "Represents a simple custom link with a text and an url.")
                .WithSetting("Stereotype", "MenuItem") // because we declare a new stereotype, the Shape MenuItem_Edit is needed
                );

            ContentDefinitionManager.AlterTypeDefinition("HtmlMenuItem", cfg => cfg
                .WithPart("MenuPart")
                .DisplayedAs("Html Menu Item")
                .WithSetting("Description", "Renders some custom HTML in the menu.")
                .WithSetting("BodyPartSettings.FlavorDefault", "html")
                .WithSetting("Stereotype", "MenuItem")
                );

            ContentDefinitionManager.AlterPartDefinition("MenuPart", builder => { });

            ContentDefinitionManager.AlterTypeDefinition("Menu", cfg => cfg
                .WithPart("PerspectivePart")
                );

            return 1;
        }
    }
}