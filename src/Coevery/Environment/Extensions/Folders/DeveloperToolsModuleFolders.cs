﻿using System.Collections.Generic;
using Coevery.Environment.Extensions.Models;

namespace Coevery.Environment.Extensions.Folders {
    public class DeveloperToolsModuleFolders : IExtensionFolders {
        private readonly IEnumerable<string> _paths;
        private readonly IExtensionHarvester _extensionHarvester;

        public DeveloperToolsModuleFolders(IEnumerable<string> paths, IExtensionHarvester extensionHarvester) {
            _paths = paths;
            _extensionHarvester = extensionHarvester;
        }

        public IEnumerable<ExtensionDescriptor> AvailableExtensions() {
            return _extensionHarvester.HarvestExtensions(_paths, DefaultExtensionTypes.Module, "Module.txt", false/*isManifestOptional*/);
        }
    }
}