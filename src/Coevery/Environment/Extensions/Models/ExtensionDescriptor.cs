﻿using System.Collections.Generic;

namespace Coevery.Environment.Extensions.Models {
    public class ExtensionDescriptor {
        /// <summary>
        /// Virtual path base, "~/Themes", "~/Modules", or "~/Core"
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Folder name under virtual path base
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The extension type.
        /// </summary>
        public string ExtensionType { get; set; }
        
        // extension metadata
        public string Name { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string CoeveryVersion { get; set; }
        public string Author { get; set; }
        public string WebSite { get; set; }
        public string Tags { get; set; }
        public string AntiForgery { get; set; }
        public string Zones { get; set; }
        public string BaseTheme { get; set; }
        public string SessionState { get; set; }
        public string TablePrefix { get; set; }
        public string Disabled { get; set; }
        public IEnumerable<FeatureDescriptor> Features { get; set; }
    }
}