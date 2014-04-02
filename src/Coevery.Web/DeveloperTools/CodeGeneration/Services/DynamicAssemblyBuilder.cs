using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using Coevery.ContentManagement.Drivers;
using Coevery.ContentManagement.Handlers;
using Coevery.Localization;
using FubuCsProjFile;
using Microsoft.VisualStudio.TextTemplating;

namespace Coevery.DeveloperTools.CodeGeneration.Services {
    public class DynamicAssemblyBuilder : IDynamicAssemblyBuilder {
        private readonly IEnumerable<IContentFieldDriver> _contentFieldDrivers;
        private readonly ITemplateGenerator _templateGenerator;

        public DynamicAssemblyBuilder(
            IEnumerable<IContentFieldDriver> contentFieldDrivers,
            ICoeveryServices coeveryServices, 
            ITemplateGenerator templateGenerator) {
            Services = coeveryServices;
            _templateGenerator = templateGenerator;
            _contentFieldDrivers = contentFieldDrivers;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }
        public ICoeveryServices Services { get; private set; }

        public Type GetFieldType(string fieldNameType) {
            var drivers = _contentFieldDrivers.Where(x => x.GetFieldInfo().Any(fi => fi.FieldTypeName == fieldNameType)).ToList();
            Type defaultType = typeof (string);
            var membersContext = new DescribeMembersContext(
                (storageName, storageType, displayName, description) => { defaultType = storageType; });
            foreach (var driver in drivers) {
                driver.Describe(membersContext);
            }
            return defaultType;
        }

        public bool Build(string moduleId) {
            string moduleCsProjPath = HostingEnvironment.MapPath(string.Format("~/Modules/{0}/{0}.csproj", moduleId));
            var csProjFile = CsProjFile.LoadFrom(moduleCsProjPath);
            ProcessTemplate(csProjFile);
            csProjFile.Save();
            return true;
        }

        private void ProcessTemplate(CsProjFile csProjFile) {
            var modelClassSession = new TextTemplatingSession();
            modelClassSession["ProjFile"] = csProjFile;
            modelClassSession["Services"] = Services;

            _templateGenerator.ProcessTemplate("ProcessTemplate.tt", modelClassSession);
        }

        
    }
}