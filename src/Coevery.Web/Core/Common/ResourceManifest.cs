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

            RegisterAngular(manifest);
            RegisterJQuery(manifest);

            manifest.DefineScript("pnotify").SetUrl("jQuery/jquery.pnotify.min.js", "jQuery/jquery.pnotify.js").SetVersion("1.2.2");
            manifest.DefineStyle("pnotify_Icons").SetUrl("jQuery/jquery.pnotify.icons.css");
            manifest.DefineStyle("pnotify").SetUrl("jQuery/jquery.pnotify.css").SetDependencies("pnotify_Icons");

            manifest.DefineScript("require").SetUrl("require.min.js", "require.js").SetVersion("2.1.6");

            manifest.DefineScript("spin").SetUrl("spin.js", "spin.js").SetVersion("1.3.2");
        }

        private void RegisterAngular(Coevery.UI.Resources.ResourceManifest manifest) {
            manifest.DefineScript("angular").SetUrl("angular/angular.min.js", "angular/angular.js").SetVersion("1.2.0-rc.2")
                .SetCdn("//ajax.googleapis.com/ajax/libs/angularjs/1.2.0-rc.2/angular.min.js", "//ajax.googleapis.com/ajax/libs/angularjs/1.2.0-rc.2/angular.min.js", true);

            var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(c => c.Name).ToArray();
            manifest.DefineScript("angular_i18n").SetUrl("angular/i18n/angular-locale.js").SetCultures(cultures);

            manifest.DefineScript("angularResource").SetUrl("angular/angular-resource.min.js", "angular/angular-resource.js").SetVersion("1.2.0-rc.2")
                .SetCdn("//ajax.googleapis.com/ajax/libs/angularjs/1.2.0-rc.2/angular-resource.min.js", "//ajax.googleapis.com/ajax/libs/angularjs/1.2.0-rc.2/angular-resource.js", true);

            manifest.DefineScript("angularRoute").SetUrl("angular/angular-route.min.js", "angular/angular-route.js").SetVersion("1.2.0-rc.2")
                .SetCdn("//ajax.googleapis.com/ajax/libs/angularjs/1.2.0-rc.2/angular-route.min.js", "//ajax.googleapis.com/ajax/libs/angularjs/1.2.0-rc.2/angular-route.js", true);

            manifest.DefineScript("angular_UI_Router").SetUrl("angular/ui-router/angular-ui-router.min.js", "angular/ui-router/angular-ui-router.js").SetVersion("0.0.2").SetDependencies("angular");

            manifest.DefineScript("angular_couchPotato").SetUrl("angular/couchPotato/angular-couchPotato.min.js", "angular/couchPotato/angular-couchPotato.js").SetVersion("0.0.4").SetDependencies("angular_UI_Router");

            manifest.DefineScript("i18next").SetUrl("angular/i18next/i18next-1.6.3.min.js", "angular/i18next/i18next-1.6.3.js").SetVersion("1.6.3");

            manifest.DefineScript("ng_i18next").SetUrl("angular/i18next/ng-i18next.min.js", "angular/i18next/ng-i18next.js").SetVersion("0.2.8");

            manifest.DefineScript("ui_utils").SetUrl("angular/ui-utils/ui-utils.min.js", "angular/ui-utils/ui-utils.js").SetVersion("0.0.3").SetDependencies("angular");

            manifest.DefineScript("ui_bootstrap").SetUrl("angular/ui-bootstrap/ui-bootstrap-tpls-0.4.0.js", "angular/ui-bootstrap/ui-bootstrap-tpls-0.4.0.js").SetVersion("0.4.0").SetDependencies("angular");

            manifest.DefineScript("underscore").SetUrl("angular/underscore/underscore.min.js", "angular/underscore/underscore.js").SetVersion("1.5.1");

            manifest.DefineScript("angularunderscore").SetUrl("angular/underscore/angular-underscore.js", "angular/underscore/angular-underscore.js").SetVersion("1.0.0").SetDependencies("angular");

            manifest.DefineScript("promisetracker").SetUrl("angular/promise-tracker/promise-tracker.js", "angular/promise-tracker/promise-tracker.js").SetVersion("1.4.2").SetDependencies("angular");
        }

        private void RegisterJQuery(Coevery.UI.Resources.ResourceManifest manifest) {
            manifest.DefineScript("jQuery").SetUrl("jquery-1.9.1.min.js", "jquery-1.9.1.js").SetVersion("1.9.1")
                .SetCdn("//ajax.aspnetcdn.com/ajax/jQuery/jquery-1.9.1.min.js", "//ajax.aspnetcdn.com/ajax/jQuery/jquery-1.9.1.js", true);

            manifest.DefineScript("jQueryMousewheel").SetUrl("jquery.mousewheel.js", "jquery.mousewheel.js").SetVersion("3.1.3");
            manifest.DefineScript("jQueryValidate").SetUrl("jquery.validate.min.js", "jquery.validate.js");

            // Full jQuery UI bundle
            manifest.DefineScript("jQueryUI").SetUrl("jquery-ui.min.js", "jquery-ui.js").SetVersion("1.9.2").SetDependencies("jQuery")
                .SetCdn("//ajax.aspnetcdn.com/ajax/jquery.ui/1.9.2/jquery-ui.min.js", "//ajax.aspnetcdn.com/ajax/jquery.ui/1.9.2/jquery-ui.js", true);

            // UI Core
            manifest.DefineScript("jQueryUI_Core").SetUrl("jquery.ui.core.min.js", "jquery.ui.core.js").SetVersion("1.9.2").SetDependencies("jQuery");
            manifest.DefineScript("jQueryUI_Widget").SetUrl("jquery.ui.widget.min.js", "jquery.ui.widget.js").SetVersion("1.9.2").SetDependencies("jQuery");
            manifest.DefineScript("jQueryUI_Mouse").SetUrl("jquery.ui.mouse.min.js", "jquery.ui.mouse.js").SetVersion("1.9.2").SetDependencies("jQuery", "jQueryUI_Widget");
            manifest.DefineScript("jQueryUI_Position").SetUrl("jquery.ui.position.min.js", "jquery.ui.position.js").SetVersion("1.9.2").SetDependencies("jQuery");

            // Interactions
            manifest.DefineScript("jQueryUI_Sortable").SetUrl("jquery.ui.sortable.min.js", "jquery.ui.sortable.js").SetVersion("1.9.2").SetDependencies("jQueryUI_Core", "jQueryUI_Widget", "jQueryUI_Mouse");

            // Widgets
            manifest.DefineScript("jQueryUI_Autocomplete").SetUrl("jquery.ui.autocomplete.min.js", "jquery.ui.autocomplete.js").SetVersion("1.9.2").SetDependencies("jQueryUI_Core", "jQueryUI_Widget", "jQueryUI_Position", "jQueryUI_Menu");
            manifest.DefineScript("jQueryUI_Slider").SetUrl("jquery.ui.slider.min.js", "jquery.ui.slider.js").SetVersion("1.9.2").SetDependencies("jQueryUI_Core", "jQueryUI_Widget", "jQueryUI_Mouse");
            manifest.DefineScript("jQueryUI_Tabs").SetUrl("jquery.ui.tabs.min.js", "jquery.ui.tabs.js").SetVersion("1.9.2").SetDependencies("jQueryUI_Core", "jQueryUI_Widget");
            manifest.DefineScript("jQueryUI_DatePicker").SetUrl("jquery.ui.datepicker.min.js", "jquery.ui.datepicker.js").SetVersion("1.9.2").SetDependencies("jQueryUI_Core");
            manifest.DefineScript("jQueryUI_SliderAccess").SetUrl("jquery-ui-sliderAccess.js").SetVersion("0.2").SetDependencies("jQueryUI_Core");
            manifest.DefineScript("jQueryUI_TimePicker").SetUrl("jquery-ui-timepicker-addon.js").SetVersion("1.0.5").SetDependencies("jQueryUI_Core", "jQueryUI_Slider", "jQueryUI_SliderAccess");

            // Effects
            manifest.DefineScript("jQueryEffects_Core").SetUrl("jquery.ui.effect.min.js", "jquery.ui.effect.js").SetVersion("1.9.2").SetDependencies("jQuery");
            manifest.DefineScript("jQueryEffects_Clip").SetUrl("jquery.ui.effect-clip.min.js", "jquery.ui.effect-clip.js").SetVersion("1.9.2").SetDependencies("jQueryEffects_Core");
            manifest.DefineScript("jQueryEffects_Drop").SetUrl("jquery.ui.effect-drop.min.js", "jquery.ui.effect-drop.js").SetVersion("1.9.2").SetDependencies("jQueryEffects_Core");

            // Menu
            manifest.DefineScript("jQueryUI_Menu").SetUrl("jquery.ui.menu.min.js", "jquery.ui.menu.js").SetVersion("1.9.2").SetDependencies("jQueryUI_Core", "jQueryUI_Widget", "jQueryUI_Position");

            manifest.DefineStyle("jQueryUI_Coevery").SetUrl("jquery-ui-1.9.2.custom.css").SetVersion("1.9.2");
            manifest.DefineStyle("jQueryUI_DatePicker").SetUrl("ui.datepicker.css").SetDependencies("jQueryUI_Coevery").SetVersion("1.7.2");
            manifest.DefineStyle("jQueryUI_TimePicker").SetUrl("jquery-ui-timepicker-addon.css").SetDependencies("jQueryUI_Coevery").SetVersion("1.0.5");

            // jqGrid
            manifest.DefineScript("jqGrid").SetUrl("jqGrid/jquery.jqGrid.min.js", "jqGrid/jquery.jqGrid.src.js").SetVersion("4.5.4").SetDependencies("jQuery");
            var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(c => c.TwoLetterISOLanguageName).ToList();
            cultures.Add("zh-CN");
            cultures.Add("zh-TW");

            manifest.DefineScript("jqGrid_i18n").SetUrl("jqGrid/i18n/grid.locale.js").SetVersion("4.5.4").SetDependencies("jqGrid").SetCultures(cultures.ToArray());

            manifest.DefineScript("simplePagination").SetUrl("jquery.simplePagination.js").SetVersion("1.6").SetDependencies("jQuery");

            manifest.DefineStyle("jqGrid").SetUrl("ui.jqgrid.css");
            manifest.DefineStyle("jqGridCustom").SetUrl("grid.css");
        }
    }
}