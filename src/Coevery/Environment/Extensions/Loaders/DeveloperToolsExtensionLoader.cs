using System;
using System.Linq;
using Coevery.Environment.Extensions.Models;
using Coevery.FileSystems.Dependencies;
using Coevery.Logging;

namespace Coevery.Environment.Extensions.Loaders {
    /// <summary>
    /// Load an extension by looking into specific namespaces of the "Coevery.DeveloperTools" assembly
    /// </summary>
    public class DeveloperToolsExtensionLoader : ExtensionLoaderBase {
        private const string DeveloperToolsAssemblyName = "Coevery.DeveloperTools";
        private readonly IAssemblyLoader _assemblyLoader;

        public DeveloperToolsExtensionLoader(IDependenciesFolder dependenciesFolder, IAssemblyLoader assemblyLoader)
            : base(dependenciesFolder) {
            _assemblyLoader = assemblyLoader;

            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }
        public bool Disabled { get; set; }

        public override int Order { get { return 20; } }

        public override ExtensionProbeEntry Probe(ExtensionDescriptor descriptor) {
            if (Disabled)
                return null;

            if (descriptor.Location == "~/DeveloperTools") {
                return new ExtensionProbeEntry {
                    Descriptor = descriptor,
                    Loader = this,
                    Priority = 100, // Higher priority because assemblies in ~/bin always take precedence
                    VirtualPath = "~/DeveloperTools/" + descriptor.Id,
                    VirtualPathDependencies = Enumerable.Empty<string>(),
                };
            }
            return null;
        }

        protected override ExtensionEntry LoadWorker(ExtensionDescriptor descriptor) {
            if (Disabled)
                return null;

            var assembly = _assemblyLoader.Load(DeveloperToolsAssemblyName);
            if (assembly == null) {
                Logger.Error("DeveloperTools modules cannot be activated because assembly '{0}' could not be loaded", DeveloperToolsAssemblyName);
                return null;
            }

            Logger.Information("Loaded DeveloperTools module \"{0}\": assembly name=\"{1}\"", descriptor.Name, assembly.FullName);

            return new ExtensionEntry {
                Descriptor = descriptor,
                Assembly = assembly,
                ExportedTypes = assembly.GetExportedTypes().Where(x => IsTypeFromModule(x, descriptor))
            };
        }

        private static bool IsTypeFromModule(Type type, ExtensionDescriptor descriptor) {
            return (type.Namespace + ".").StartsWith(DeveloperToolsAssemblyName + "." + descriptor.Id + ".");
        }
    }
}