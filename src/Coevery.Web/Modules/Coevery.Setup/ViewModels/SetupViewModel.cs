using System.Collections.Generic;
using System.ComponentModel;
using Coevery.Recipes.Models;
using Coevery.Setup.Annotations;
using Coevery.Setup.Controllers;

namespace Coevery.Setup.ViewModels {
    public class SetupViewModel  {
        public SetupViewModel() {
        }

        [SiteNameValid(maximumLength: 70)]
        public string SiteName { get; set; }
        [UserNameValid(minimumLength: 3, maximumLength: 25)]
        public string AdminUsername { get; set; }
        [PasswordValid(minimumLength: 7, maximumLength: 50)]
        public string AdminPassword { get; set; }
        [PasswordConfirmationRequired]
        public string ConfirmPassword { get; set; }
        public SetupDatabaseType DatabaseProvider { get; set; }
        
        public string DatabaseServerName { get; set; }
        public string DatabaseName { get; set; }
        public DatabaseAuthenticationMode ServerAuthentication { get; set; }
        public string DatabaseUserName { get; set; }
        public string DatabasePassword { get; set; }
        public string DatabaseTablePrefix { get; set; }
        public bool DatabaseIsPreconfigured { get; set; }
    }
}