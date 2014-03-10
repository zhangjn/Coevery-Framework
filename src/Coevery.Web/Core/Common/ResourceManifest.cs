using System.Globalization;
using System.Linq;
using Coevery.UI.Resources;

namespace Coevery.Core.Common {
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();
            manifest.DefineScript("ShapesBase").SetUrl("base.js").SetDependencies("jQuery");
            manifest.DefineStyle("Shapes").SetUrl("site.css"); // todo: missing
            manifest.DefineStyle("ShapesSpecial").SetUrl("special.css");

            manifest.DefineScript("Switchable").SetUrl("jquery.switchable.js")
                .SetDependencies("jQuery")
                .SetDependencies("ShapesBase");
            manifest.DefineStyle("Switchable").SetUrl("jquery.switchable.css");

            manifest.DefineStyle("Module").SetUrl("module.css");

            manifest.DefineScript("angular").SetUrl("angular.min.js", "angular.js").SetVersion("1.2.0-rc.2")
                .SetCdn("//ajax.googleapis.com/ajax/libs/angularjs/1.2.0-rc.2/angular.min.js", "//ajax.googleapis.com/ajax/libs/angularjs/1.2.0-rc.2/angular.min.js", true);

            var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(c => c.Name).ToArray();
            manifest.DefineScript("angular_i18n").SetUrl("i18n/angular-locale.js").SetCultures(cultures);

            manifest.DefineScript("angularResource").SetUrl("angular-resource.min.js", "angular-resource.js").SetVersion("1.2.0-rc.2")
                .SetCdn("//ajax.googleapis.com/ajax/libs/angularjs/1.2.0-rc.2/angular-resource.min.js", "//ajax.googleapis.com/ajax/libs/angularjs/1.2.0-rc.2/angular-resource.js", true);

            manifest.DefineScript("angularRoute").SetUrl("angular-route.min.js", "angular-route.js").SetVersion("1.2.0-rc.2")
                .SetCdn("//ajax.googleapis.com/ajax/libs/angularjs/1.2.0-rc.2/angular-route.min.js", "//ajax.googleapis.com/ajax/libs/angularjs/1.2.0-rc.2/angular-route.js", true);

            manifest.DefineScript("ng_grid_row_selection").SetUrl("ng-grid-row-selection.js", "ng-grid-row-selection.js");

            manifest.DefineScript("ng_grid_flexible_height").SetUrl("ng-grid-flexible-height.js", "ng-grid-flexible-height.js").SetDependencies("ng_grid_row_selection");

            manifest.DefineScript("ngGrid").SetUrl("ng-grid-2.0.6.min.js", "ng-grid-2.0.6.debug.js").SetVersion("2.0.6").SetDependencies("ng_grid_flexible_height");
            manifest.DefineStyle("ngGrid").SetUrl("ng-grid.css");

            manifest.DefineScript("pnotify").SetUrl("jquery.pnotify.min.js", "jquery.pnotify.js").SetVersion("1.2.2");
            manifest.DefineStyle("pnotify_Icons").SetUrl("jquery.pnotify.icons.css");
            manifest.DefineStyle("pnotify").SetUrl("jquery.pnotify.css").SetDependencies("pnotify_Icons");

            manifest.DefineScript("angular_UI_Router").SetUrl("angular-ui-router.min.js", "angular-ui-router.js").SetVersion("0.0.2").SetDependencies("angular");

            manifest.DefineScript("angular_couchPotato").SetUrl("angular-couchPotato.min.js", "angular-couchPotato.js").SetVersion("0.0.4").SetDependencies("angular_UI_Router");

            manifest.DefineScript("require").SetUrl("require.min.js", "require.js").SetVersion("2.1.6");

            manifest.DefineScript("i18next").SetUrl("i18next-1.6.3.min.js", "i18next-1.6.3.js").SetVersion("1.6.3");

            manifest.DefineScript("ng_i18next").SetUrl("ng-i18next.min.js", "ng-i18next.js").SetVersion("0.2.8");

            manifest.DefineScript("ui_utils").SetUrl("ui-utils.min.js", "ui-utils.js").SetVersion("0.0.3").SetDependencies("angular");

            manifest.DefineScript("ui_bootstrap").SetUrl("ui-bootstrap-tpls-0.4.0.js", "ui-bootstrap-tpls-0.4.0.js").SetVersion("0.4.0").SetDependencies("angular");
            manifest.DefineScript("underscore").SetUrl("underscore.min.js", "underscore.js").SetVersion("1.5.1");
            manifest.DefineScript("angularunderscore").SetUrl("angular-underscore.js", "angular-underscore.js").SetVersion("1.0.0").SetDependencies("angular");
            manifest.DefineScript("promisetracker").SetUrl("promise-tracker.js", "promise-tracker.js").SetVersion("1.4.2").SetDependencies("angular");
            manifest.DefineScript("spin").SetUrl("spin.js", "spin.js").SetVersion("1.3.2");
        }
    }
}