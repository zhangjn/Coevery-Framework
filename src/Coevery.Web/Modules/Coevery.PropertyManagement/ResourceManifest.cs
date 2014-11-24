using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.UI.Resources;

namespace Coevery.PropertyManagement
{
    public class ResourceManifest: IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();
            manifest.DefineScript("DatepickerLanguagePack").SetUrl("jquery.ui.datepicker-zh-TW.js").SetDependencies("jQuery");
            manifest.DefineScript("handlebars").SetUrl("handlebars.js").SetDependencies("jQuery");
            manifest.DefineScript("chineseAmountConvert").SetUrl("chineseAmountConvert.js").SetDependencies("jQuery");
        }
    }
    
}