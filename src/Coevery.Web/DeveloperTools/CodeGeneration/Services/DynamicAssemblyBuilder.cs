using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using Coevery.ContentManagement.Drivers;
using Coevery.ContentManagement.Handlers;
using Coevery.ContentManagement.MetaData;
using Coevery.Core.Common.Extensions;
using Coevery.Core.Projections.Models;
using Coevery.Core.Projections.Services;
using Coevery.DeveloperTools.CodeGeneration.CodeGenerationTemplates;
using Coevery.DeveloperTools.FormDesigner.Models;
using Coevery.Localization;
using FubuCore;
using FubuCsProjFile;
using Newtonsoft.Json.Linq;

namespace Coevery.DeveloperTools.CodeGeneration.Services {
    public class DynamicAssemblyBuilder : IDynamicAssemblyBuilder {

        public IProjectionManager _projectionManager;
        internal const string AssemblyName = "Coevery.DynamicTypes";
        private readonly IEnumerable<IContentFieldDriver> _contentFieldDrivers;
        private readonly IContentDefinitionExtension _contentDefinitionExtension;
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IGridColumn _gridColumn;

        public DynamicAssemblyBuilder(IEnumerable<IContentFieldDriver> contentFieldDrivers,
            IContentDefinitionExtension contentDefinitionExtension,
            IContentDefinitionManager contentDefinitionManager) {
            ICoeveryServices coeveryServices,
            IGridColumn gridColumn)
        {
            Services = coeveryServices;
            _projectionManager = projectionManager;
            _contentFieldDrivers = contentFieldDrivers;
            _contentDefinitionManager = contentDefinitionManager;
            _contentDefinitionExtension = contentDefinitionExtension;
            _gridColumn = gridColumn;
        }

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
            // user-defined parts
            // except for those parts with the same name as a type (implicit type's part or a mistake)
            var userDefinedParts = _contentDefinitionExtension
                .ListUserDefinedPartDefinitions()
                .Select(cpd => new DynamicDefinition {
                    Name = cpd.Name.RemovePartSuffix(),
                    Fields = cpd.Fields.Select(f => new DynamicFieldDefinition {
                        Name = f.Name,
                        Type = GetFieldType(f.FieldDefinition.Name)
                    })
                }).ToList();

            if (userDefinedParts.Any()) {
                Build(userDefinedParts, moduleId);
                return true;
            }
            return false;
        }

        private void Build(IEnumerable<DynamicDefinition> typeDefinitions, string moduleId) {
            string moduleCsProjPath = HostingEnvironment.MapPath(string.Format("~/Modules/{0}/{0}.csproj", moduleId));
            var csProjFile = CsProjFile.LoadFrom(moduleCsProjPath);
            foreach (var definition in typeDefinitions) {
                AddModelClassFile(csProjFile, definition);
                AddControllerFile(csProjFile, definition);
                AddDriverFile(csProjFile, definition);
                AddViewFile(csProjFile,definition);
            }
            csProjFile.Save();
        }

        private void AddControllerFile(CsProjFile csProjFile, DynamicDefinition controllerDefinition)
        {
            string moduleControllersPath = Path.Combine(csProjFile.ProjectDirectory, "Controllers");
            if (!moduleControllersPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                moduleControllersPath += Path.DirectorySeparatorChar;
            }
            if (!Directory.Exists(moduleControllersPath))
            {
                Directory.CreateDirectory(moduleControllersPath);
            }

            string controllerClassFilePath = moduleControllersPath + controllerDefinition.Name + "Controller.cs";
            if (File.Exists(controllerClassFilePath))
            {
                //Context.Output.WriteLine(T("Controller {0} already exists in target Module {1}.", controllerName, moduleName));
                return;
            }
            var partTemplate = new ControllerTemplate() { Session = new Dictionary<string, object>() };
            partTemplate.Session["Namespace"] = csProjFile.RootNamespace;
            partTemplate.Session["ControllerName"] = controllerDefinition.Name;
            partTemplate.Initialize();
            string controllerText = partTemplate.TransformText();
            File.WriteAllText(controllerClassFilePath, controllerText);

            var partRelativePath = controllerClassFilePath.PathRelativeTo(csProjFile.ProjectDirectory);
            csProjFile.Add<CodeFile>(partRelativePath);

            string apiControllerClassFilePath = moduleControllersPath + controllerDefinition.Name + "ApiController.cs";
            if (File.Exists(apiControllerClassFilePath))
            {
                //Context.Output.WriteLine(T("Controller {0} already exists in target Module {1}.", controllerName, moduleName));
                return;
            }
            var apiControllerTemplate = new ApiControllerTemplate() { Session = new Dictionary<string, object>() };
            apiControllerTemplate.Session["Namespace"] = csProjFile.RootNamespace;
            apiControllerTemplate.Session["ControllerName"] = controllerDefinition.Name;
            apiControllerTemplate.Initialize();
            string apiControllerText = apiControllerTemplate.TransformText();
            File.WriteAllText(apiControllerClassFilePath, apiControllerText);

            var recordRelativePath = apiControllerClassFilePath.PathRelativeTo(csProjFile.ProjectDirectory);
            csProjFile.Add<CodeFile>(recordRelativePath);
        }

        private void AddViewFile(CsProjFile csProjFile, DynamicDefinition viewDefinition) {
            string viewsNamePath = Path.Combine(csProjFile.ProjectDirectory, "Views",viewDefinition.Name);
            string viewsPartPath = Path.Combine(csProjFile.ProjectDirectory, "Views","Parts");

            if (!viewsNamePath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                viewsNamePath += Path.DirectorySeparatorChar;
            }
            if (!Directory.Exists(viewsNamePath))
            {
                Directory.CreateDirectory(viewsNamePath);
            }

            if (!viewsPartPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                viewsPartPath += Path.DirectorySeparatorChar;
            }
            if (!Directory.Exists(viewsPartPath))
            {
                Directory.CreateDirectory(viewsPartPath);
            }

            var id = _contentDefinitionExtension.GetEntityNameFromCollectionName(viewDefinition.Name);

            var query = Services.ContentManager.Query<ListViewPart, ListViewPartRecord>("ListViewPage")
                 .Where(v => v.IsDefault).List().ToList().FirstOrDefault();

            var gridDefinition = (Object[])_gridColumn.Get(viewDefinition.Name, query.Id);
            
            string viewNameFilePath = viewsNamePath + "ListView-" + viewDefinition.Name + ".cshtml";
            if (File.Exists(viewNameFilePath))
            {
                //Context.Output.WriteLine(T("Controller {0} already exists in target Module {1}.", controllerName, moduleName));
                return;
            }
            var viewNameTemplate = new ListViewTemplate() { Session = new Dictionary<string, object>() };

            viewNameTemplate.Session["ModelDefinition"] = gridDefinition;
            viewNameTemplate.Session["Namespace"] = csProjFile.RootNamespace;
            viewNameTemplate.Session["ViewName"] = viewDefinition.Name;
            viewNameTemplate.Initialize();
            string partText = viewNameTemplate.TransformText();
            File.WriteAllText(viewNameFilePath, partText);

            var viewNameRelativePath = viewNameFilePath.PathRelativeTo(csProjFile.ProjectDirectory);
            csProjFile.Add<Content>(viewNameRelativePath);

            string viewPartFilePath = viewsPartPath + "Index.cshtml";
            if (File.Exists(viewPartFilePath))
            {
                //Context.Output.WriteLine(T("Controller {0} already exists in target Module {1}.", controllerName, moduleName));
                return;
            }
            var indexViewTemplate = new IndexViewTemplate() { Session = new Dictionary<string, object>() };
            string indexViewText = indexViewTemplate.TransformText();
            File.WriteAllText(viewPartFilePath, indexViewText);

            var indexViewRelativePath = viewPartFilePath.PathRelativeTo(csProjFile.ProjectDirectory);
            csProjFile.Add<Content>(indexViewRelativePath);
        }

        private void AddDriverFile(CsProjFile csProjFile, DynamicDefinition driverDefinition)
        {
            string moduleDriversPath = Path.Combine(csProjFile.ProjectDirectory, "Drivers");
            if (!moduleDriversPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                moduleDriversPath += Path.DirectorySeparatorChar;
            }
            if (!Directory.Exists(moduleDriversPath))
            {
                Directory.CreateDirectory(moduleDriversPath);
            }

            string partClassFilePath = moduleDriversPath + driverDefinition.Name + "PartDriver.cs";
            if (File.Exists(partClassFilePath))
            {
                //Context.Output.WriteLine(T("Controller {0} already exists in target Module {1}.", controllerName, moduleName));
                return;
            }
            var partTemplate = new DriverTemplate() { Session = new Dictionary<string, object>() };
            partTemplate.Session["Namespace"] = csProjFile.RootNamespace;
            partTemplate.Session["DriverName"] = driverDefinition.Name;
            partTemplate.Initialize();
            string partText = partTemplate.TransformText();
            File.WriteAllText(partClassFilePath, partText);

            var partRelativePath = partClassFilePath.PathRelativeTo(csProjFile.ProjectDirectory);
            csProjFile.Add<CodeFile>(partRelativePath);
        }

        private void AddModelClassFile(CsProjFile csProjFile, DynamicDefinition modelDefinition) {
            string moduleModelsPath = Path.Combine(csProjFile.ProjectDirectory, "Models");
            CheckDirectories(moduleModelsPath);

            string partClassFilePath = Path.Combine(moduleModelsPath, modelDefinition.Name + "Part.cs");
            var partTemplate = new ContentPartTemplate() {Session = new Dictionary<string, object>()};
            partTemplate.Session["Namespace"] = csProjFile.RootNamespace;
            partTemplate.Session["ModelDefinition"] = modelDefinition;
            partTemplate.Initialize();
            AddFile<CodeFile>(csProjFile, partClassFilePath, partTemplate.TransformText());

            string recordClassFilePath = Path.Combine(moduleModelsPath, modelDefinition.Name + "PartRecord.cs");
            var recordTemplate = new ContentPartRecordTemplate() {Session = new Dictionary<string, object>()};
            recordTemplate.Session["Namespace"] = csProjFile.RootNamespace;
            recordTemplate.Session["ModelDefinition"] = modelDefinition;
            recordTemplate.Initialize();
            AddFile<CodeFile>(csProjFile, recordClassFilePath, recordTemplate.TransformText());
        }

        private void AddViewFiles(CsProjFile csProjFile, DynamicDefinition modelDefinition) {
            string viewsPath = Path.Combine(csProjFile.ProjectDirectory, "Views");
            string controllerViewPath = Path.Combine(viewsPath, modelDefinition.Name);
            string partsViewPath = Path.Combine(viewsPath, "Parts");
            CheckDirectories(viewsPath, controllerViewPath, partsViewPath);

            // {{EntityName}}/Create.cshtml
            string createViewFilePath = Path.Combine(controllerViewPath, "Create.cshtml");
            var createViewTemplate = new CreateViewTemplate();
            AddFile<Content>(csProjFile, createViewFilePath, createViewTemplate.TransformText());

            // {{EntityName}}/Edit.cshtml
            string editViewFilePath = Path.Combine(controllerViewPath, "Edit.cshtml");
            var editViewTemplate = new EditViewTemplate();
            AddFile<Content>(csProjFile, editViewFilePath, editViewTemplate.TransformText());

            // {{EntityName}}/Detail.cshtml
            string detailViewFilePath = Path.Combine(controllerViewPath, "Detail.cshtml");
            var detailViewTemplate = new DetailViewTemplate();
            AddFile<Content>(csProjFile, detailViewFilePath, detailViewTemplate.TransformText());

            // Parts/CreateView-{{EntityName}}.cshtml
            var contentTypeDefinition = _contentDefinitionManager.GetTypeDefinition(modelDefinition.Name);
            var sectionList = contentTypeDefinition.Settings.ContainsKey("Layout")
                ? JsonConvert.DeserializeObject<IList<Section>>(contentTypeDefinition.Settings["Layout"])
                : Enumerable.Empty<Section>();

            string partsCreateViewFilePath = Path.Combine(partsViewPath, string.Format("CreateView-{0}.cshtml", modelDefinition.Name));
            var partsCreateViewTemplate = new PartsCreateViewTemplate() {Session = new Dictionary<string, object>()};
            partsCreateViewTemplate.Session["ModuleName"] = modelDefinition.Name;
            partsCreateViewTemplate.Session["SectionList"] = sectionList;
            partsCreateViewTemplate.Initialize();
            AddFile<Content>(csProjFile, partsCreateViewFilePath, partsCreateViewTemplate.TransformText());

            // Parts/EditView-{{EntityName}}.cshtml
            string partsEditViewFilePath = Path.Combine(partsViewPath, string.Format("EditView-{0}.cshtml", modelDefinition.Name));
            var partsEditViewTemplate = new PartsEditViewTemplate() {Session = new Dictionary<string, object>()};
            partsEditViewTemplate.Session["ModuleName"] = modelDefinition.Name;
            partsEditViewTemplate.Session["SectionList"] = sectionList;
            partsEditViewTemplate.Initialize();
            AddFile<Content>(csProjFile, partsEditViewFilePath, partsEditViewTemplate.TransformText());

            // Parts/DetailView-{{EntityName}}.cshtml
            string partsDetailViewFilePath = Path.Combine(partsViewPath, string.Format("DetailView-{0}.cshtml", modelDefinition.Name));
            var partsDetailViewTemplate = new PartsDetailViewTemplate() {Session = new Dictionary<string, object>()};
            partsDetailViewTemplate.Session["SectionList"] = sectionList;
            partsDetailViewTemplate.Initialize();
            AddFile<Content>(csProjFile, partsDetailViewFilePath, partsDetailViewTemplate.TransformText());
        }

        private void AddFile<T>(CsProjFile csProjFile, string path, string text) where T : ProjectItem, new() {
            File.WriteAllText(path, text);
            var relativePath = path.PathRelativeTo(csProjFile.ProjectDirectory);
            csProjFile.Add<T>(relativePath);
        }

        private void CheckDirectories(params string[] paths) {
            foreach (var path in paths) {
                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }
            }
        }
    }
}