using System.Collections.Generic;
using System.Collections.ObjectModel;
using Coevery.ContentManagement;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.ContentManagement.MetaData.Services;
using Coevery.ContentManagement.Utilities;
using Coevery.Core.Settings.Metadata.Records;
using Coevery.Utility.Extensions;

namespace Coevery.Core.Settings.Metadata.Parts {
    public class ContentTypeDefinitionPart : ContentPart<ContentTypeDefinitionRecord> {
        public readonly LazyField<SettingsDictionary> _definitionSettings = new LazyField<SettingsDictionary>();

        public string Name {
            get { return Record.Name; }
            set { Record.Name = value; }
        }

        public string DisplayName {
            get { return Record.DisplayName; }
            set { Record.DisplayName = value; }
        }

        public ReadOnlyDictionary<string, string> DefinitionSettings {
            get {
                return new ReadOnlyDictionary<string, string>(_definitionSettings.Value);
            }
        }

        public void WithSetting(SettingsDictionary settings) {
            _definitionSettings.Value = settings;
        }

        public void WithSetting(string name, string value) {
            var settings = new Dictionary<string, string>();
            settings[name] = value;
            WithSetting(new SettingsDictionary(settings));
        }
    }
}