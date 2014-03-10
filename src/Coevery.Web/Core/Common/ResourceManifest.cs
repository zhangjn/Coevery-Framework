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

            manifest.DefineScript("require").SetUrl("require.min.js", "require.js").SetVersion("2.1.6");
            manifest.DefineScript("spin").SetUrl("spin.js", "spin.js").SetVersion("1.3.2");

            RegisterAngular(manifest);
            RegisterJQuery(manifest);
        }

        private void RegisterAngular(UI.Resources.ResourceManifest manifest) {
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

        private void RegisterJQuery(UI.Resources.ResourceManifest manifest) {
            manifest.DefineScript("jQuery").SetUrl("jQuery/jquery-1.9.1.min.js", "jQuery/jquery-1.9.1.js").SetVersion("1.9.1")
                .SetCdn("//ajax.aspnetcdn.com/ajax/jQuery/jquery-1.9.1.min.js", "//ajax.aspnetcdn.com/ajax/jQuery/jquery-1.9.1.js", true);

            manifest.DefineScript("jQueryMousewheel").SetUrl("jQuery/jquery.mousewheel.js", "jQuery/jquery.mousewheel.js").SetVersion("3.1.3");
            manifest.DefineScript("jQueryValidate").SetUrl("jQuery/jquery.validate.min.js", "jQuery/jquery.validate.js");

            // Full jQuery UI bundle
            manifest.DefineScript("jQueryUI").SetUrl("jQuery/jquery-ui.min.js", "jQuery/jquery-ui.js").SetVersion("1.9.2").SetDependencies("jQuery")
                .SetCdn("//ajax.aspnetcdn.com/ajax/jquery.ui/1.9.2/jquery-ui.min.js", "//ajax.aspnetcdn.com/ajax/jquery.ui/1.9.2/jquery-ui.js", true);

            // UI Core
            manifest.DefineScript("jQueryUI_Core").SetUrl("jQuery/jquery.ui.core.min.js", "jQuery/jquery.ui.core.js").SetVersion("1.9.2").SetDependencies("jQuery");
            manifest.DefineScript("jQueryUI_Widget").SetUrl("jQuery/jquery.ui.widget.min.js", "jQuery/jquery.ui.widget.js").SetVersion("1.9.2").SetDependencies("jQuery");
            manifest.DefineScript("jQueryUI_Mouse").SetUrl("jQuery/jquery.ui.mouse.min.js", "jQuery/jquery.ui.mouse.js").SetVersion("1.9.2").SetDependencies("jQuery", "jQueryUI_Widget");
            manifest.DefineScript("jQueryUI_Position").SetUrl("jQuery/jquery.ui.position.min.js", "jQuery/jquery.ui.position.js").SetVersion("1.9.2").SetDependencies("jQuery");

            // Interactions
            manifest.DefineScript("jQueryUI_Sortable").SetUrl("jQuery/jquery.ui.sortable.min.js", "jQuery/jquery.ui.sortable.js").SetVersion("1.9.2").SetDependencies("jQueryUI_Core", "jQueryUI_Widget", "jQueryUI_Mouse");

            // Widgets
            manifest.DefineScript("jQueryUI_Autocomplete").SetUrl("jQuery/jquery.ui.autocomplete.min.js", "jQuery/jquery.ui.autocomplete.js").SetVersion("1.9.2").SetDependencies("jQueryUI_Core", "jQueryUI_Widget", "jQueryUI_Position", "jQueryUI_Menu");
            manifest.DefineScript("jQueryUI_Slider").SetUrl("jQuery/jquery.ui.slider.min.js", "jQuery/jquery.ui.slider.js").SetVersion("1.9.2").SetDependencies("jQueryUI_Core", "jQueryUI_Widget", "jQueryUI_Mouse");
            manifest.DefineScript("jQueryUI_Tabs").SetUrl("jQuery/jquery.ui.tabs.min.js", "jQuery/jquery.ui.tabs.js").SetVersion("1.9.2").SetDependencies("jQueryUI_Core", "jQueryUI_Widget");
            manifest.DefineScript("jQueryUI_DatePicker").SetUrl("jQuery/jquery.ui.datepicker.min.js", "jQuery/jquery.ui.datepicker.js").SetVersion("1.9.2").SetDependencies("jQueryUI_Core");
            manifest.DefineScript("jQueryUI_SliderAccess").SetUrl("jQuery/jquery-ui-sliderAccess.js").SetVersion("0.2").SetDependencies("jQueryUI_Core");
            manifest.DefineScript("jQueryUI_TimePicker").SetUrl("jQuery/jquery-ui-timepicker-addon.js").SetVersion("1.0.5").SetDependencies("jQueryUI_Core", "jQueryUI_Slider", "jQueryUI_SliderAccess");

            // Effects
            manifest.DefineScript("jQueryEffects_Core").SetUrl("jQuery/jquery.ui.effect.min.js", "jQuery/jquery.ui.effect.js").SetVersion("1.9.2").SetDependencies("jQuery");
            manifest.DefineScript("jQueryEffects_Clip").SetUrl("jQuery/jquery.ui.effect-clip.min.js", "jQuery/jquery.ui.effect-clip.js").SetVersion("1.9.2").SetDependencies("jQueryEffects_Core");
            manifest.DefineScript("jQueryEffects_Drop").SetUrl("jQuery/jquery.ui.effect-drop.min.js", "jQuery/jquery.ui.effect-drop.js").SetVersion("1.9.2").SetDependencies("jQueryEffects_Core");

            // Menu
            manifest.DefineScript("jQueryUI_Menu").SetUrl("jQuery/jquery.ui.menu.min.js", "jQuery/jquery.ui.menu.js").SetVersion("1.9.2").SetDependencies("jQueryUI_Core", "jQueryUI_Widget", "jQueryUI_Position");

            manifest.DefineStyle("jQueryUI_Coevery").SetUrl("jQuery/jquery-ui-1.9.2.custom.css").SetVersion("1.9.2");
            manifest.DefineStyle("jQueryUI_DatePicker").SetUrl("jQuery/ui.datepicker.css").SetDependencies("jQueryUI_Coevery").SetVersion("1.7.2");
            manifest.DefineStyle("jQueryUI_TimePicker").SetUrl("jQuery/jquery-ui-timepicker-addon.css").SetDependencies("jQueryUI_Coevery").SetVersion("1.0.5");

            // jqGrid
            manifest.DefineScript("jqGrid").SetUrl("jQuery/jqGrid/jquery.jqGrid.min.js", "jQuery/jqGrid/jquery.jqGrid.src.js").SetVersion("4.5.4").SetDependencies("jQuery");
            var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(c => c.TwoLetterISOLanguageName).ToList();
            cultures.Add("zh-CN");
            cultures.Add("zh-TW");

            manifest.DefineScript("jqGrid_i18n").SetUrl("jQuery/jqGrid/i18n/grid.locale.js").SetVersion("4.5.4").SetDependencies("jqGrid").SetCultures(cultures.ToArray());

            manifest.DefineScript("simplePagination").SetUrl("jQuery/jquery.simplePagination.js").SetVersion("1.6").SetDependencies("jQuery");

            manifest.DefineStyle("jqGrid").SetUrl("jQuery/ui.jqgrid.css");
            manifest.DefineStyle("jqGridCustom").SetUrl("jQuery/grid.css");

            manifest.DefineScript("pnotify").SetUrl("jQuery/jquery.pnotify.min.js", "jQuery/jquery.pnotify.js").SetVersion("1.2.2");
            manifest.DefineStyle("pnotify_Icons").SetUrl("jQuery/jquery.pnotify.icons.css");
            manifest.DefineStyle("pnotify").SetUrl("jQuery/jquery.pnotify.css").SetDependencies("pnotify_Icons");
        }
    }
}