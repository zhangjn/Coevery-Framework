using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Coevery.Environment;
using Coevery.Environment.Configuration;
using Coevery.Logging;
using Coevery.Recipes.Models;
using Coevery.Setup.Services;
using Coevery.Setup.ViewModels;
using Coevery.Localization;
using Coevery.Themes;
using Coevery.UI.Notify;

namespace Coevery.Setup.Controllers {
    [ValidateInput(false), Themed]
    public class SetupController : Controller {
        private readonly IViewsBackgroundCompilation _viewsBackgroundCompilation;
        private readonly ShellSettings _shellSettings;
        private readonly INotifier _notifier;
        private readonly ISetupService _setupService;
        private const string DefaultRecipe = "Default";

        public SetupController(
            INotifier notifier, 
            ISetupService setupService, 
            IViewsBackgroundCompilation viewsBackgroundCompilation,
            ShellSettings shellSettings) {
            _viewsBackgroundCompilation = viewsBackgroundCompilation;
            _shellSettings = shellSettings;
            _notifier = notifier;
            _setupService = setupService;

            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        public Localizer T { get; set; }
        public ILogger Logger { get; set; }

        private ActionResult IndexViewResult(SetupViewModel model) {
            return View(model);
        }

        public ActionResult Index() {
            var initialSettings = _setupService.Prime();
            
            // On the first time installation of Coevery, the user gets to the setup screen, which
            // will take a while to finish (user inputting data and the setup process itself).
            // We use this opportunity to start a background task to "pre-compile" all the known
            // views in the app folder, so that the application is more reponsive when the user
            // hits the homepage and admin screens for the first time.))
            if (StringComparer.OrdinalIgnoreCase.Equals(initialSettings.Name, ShellSettings.DefaultName)) {
                _viewsBackgroundCompilation.Start();
            }

            //

            return IndexViewResult(new SetupViewModel {
                AdminUsername = "admin",
                DatabaseIsPreconfigured = !string.IsNullOrEmpty(initialSettings.DataProvider)
            });
        }

        [HttpPost, ActionName("Index")]
        public ActionResult IndexPOST(SetupViewModel model) {
            // sets the setup request timeout to 10 minutes to give enough time to execute custom recipes.  
            HttpContext.Server.ScriptTimeout = 600;

            // if no builtin provider, a connection string is mandatory
            if (model.DatabaseProvider != SetupDatabaseType.Builtin) {
                if (string.IsNullOrEmpty(model.DatabaseServerName)) {
                    ModelState.AddModelError("DatabaseServerName", T("A database server name is required").Text);
                }
                if (string.IsNullOrEmpty(model.DatabaseName)) {
                    ModelState.AddModelError("DatabaseName", T("A database name is required").Text);
                }
                if ((model.DatabaseProvider == SetupDatabaseType.MySql || model.ServerAuthentication == DatabaseAuthenticationMode.UserNamePassword)) {
                    if (string.IsNullOrEmpty(model.DatabaseUserName)) {
                        ModelState.AddModelError("DatabaseUserName", T("A database user name is required").Text);
                    }
                    if (string.IsNullOrEmpty(model.DatabasePassword)) {
                        ModelState.AddModelError("DatabasePassword", T("A database user password name is required").Text);
                    }
                }
            }


            if (!String.IsNullOrWhiteSpace(model.ConfirmPassword) && model.AdminPassword != model.ConfirmPassword ) {
                ModelState.AddModelError("ConfirmPassword", T("Password confirmation must match").Text);
            }

            if (model.DatabaseProvider != SetupDatabaseType.Builtin && !String.IsNullOrWhiteSpace(model.DatabaseTablePrefix)) {
                model.DatabaseTablePrefix = model.DatabaseTablePrefix.Trim();
                if(!Char.IsLetter(model.DatabaseTablePrefix[0])) {
                    ModelState.AddModelError("DatabaseTablePrefix", T("The table prefix must begin with a letter").Text);
                }

                if(model.DatabaseTablePrefix.Any(x => !Char.IsLetterOrDigit(x))) {
                    ModelState.AddModelError("DatabaseTablePrefix", T("The table prefix must contain letters or digits").Text);
                }
            }
            if (!ModelState.IsValid) {
                model.DatabaseIsPreconfigured = !string.IsNullOrEmpty(_setupService.Prime().DataProvider);
                
                return IndexViewResult(model);
            }

            try {
                string providerName = null;
                string databaseConnectionString = null;

                switch (model.DatabaseProvider)
                {
                    case SetupDatabaseType.Builtin:
                        providerName = "SqlCe";
                        break;

                    case SetupDatabaseType.SqlServer:
                        providerName = "SqlServer";
                        databaseConnectionString = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;", model.DatabaseServerName, model.DatabaseName);
                        if (model.ServerAuthentication == DatabaseAuthenticationMode.Windows) 
                            databaseConnectionString += "Integrated Security=true;";
                        else 
                            databaseConnectionString += string.Format("User ID={0};Password={1}", model.DatabaseUserName, model.DatabasePassword);
                        break;

                    case SetupDatabaseType.MySql:
                        providerName = "MySql";
                        databaseConnectionString = string.Format("Data Source={0};Database={1};User Id={2};Password={3}", model.DatabaseServerName, model.DatabaseName, model.DatabaseUserName, model.DatabasePassword);
                        break;

                    default:
                        throw new ApplicationException("Unknown database type: " + model.DatabaseProvider);
                }

                var setupContext = new SetupContext {
                    SiteName = model.SiteName,
                    AdminUsername = model.AdminUsername,
                    AdminPassword = model.AdminPassword,
                    DatabaseProvider = providerName,
                    DatabaseConnectionString = databaseConnectionString,
                    DatabaseTablePrefix = model.DatabaseTablePrefix,
                    EnabledFeatures = null // default list
                };

                _setupService.Setup(setupContext);

                // First time installation if finally done. Tell the background views compilation
                // process to stop, so that it doesn't interfere with the user (asp.net compilation
                // uses a "single lock" mechanism for compiling views).
                _viewsBackgroundCompilation.Stop();

                // redirect to the welcome page.
                return Redirect("~/" + _shellSettings.RequestUrlPrefix);
            } catch (Exception ex) {
                Logger.Error(ex, "Setup failed");
                _notifier.Error(T("Setup failed: {0}", ex.Message));

                model.DatabaseIsPreconfigured = !string.IsNullOrEmpty(_setupService.Prime().DataProvider);

                return IndexViewResult(model);
            }
        }
    }
}