using Coevery.ContentManagement.MetaData;
using Coevery.Data.Migration;

namespace Sample.Lead {
    public class Migrations : DataMigrationImpl {
        public int Create() {
            ContentDefinitionManager.AlterPartDefinition(
                "LeadPart", cfg => cfg.WithField("Subject", f => f.OfType("TextField"))
                    .WithField("Description", f => f.OfType("TextField"))
                    .WithField("CompanyName", f => f.OfType("TextField")));

            ContentDefinitionManager.AlterTypeDefinition(
                "Lead", cfg => cfg.WithPart("LeadPart"));

            return 1;
        }
    }
}