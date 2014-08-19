using Coevery.ContentManagement.MetaData;
using Coevery.Core.Settings.Metadata;
using Coevery.Data.Migration;

namespace Coevery.Users {
    public class UsersDataMigration : DataMigrationImpl {
        public int Create() {
            SchemaBuilder.CreateTable("User",
                table => table
                    .ContentPartRecord()
                    .Column<string>("UserName")
                    .Column<string>("Email")
                    .Column<string>("NormalizedUserName")
                    .Column<string>("Password")
                    .Column<string>("PasswordFormat")
                    .Column<string>("HashAlgorithm")
                    .Column<string>("PasswordSalt")
                    .Column<string>("RegistrationStatus", c => c.WithDefault("Approved"))
                    .Column<string>("EmailStatus", c => c.WithDefault("Approved"))
                    .Column<string>("EmailChallengeToken")
                );

            return 1;
        }

        public int UpdateFrom1() {
            ContentDefinitionManager.AlterTypeDefinition("User",
                cfg => cfg
                    .WithPart("UserPart")
                    .CollectionDisplayNameAs("Users")
                    .Creatable());

            return 2;
        }
    }
}