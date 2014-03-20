using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using Coevery.ContentManagement;
using Coevery.ContentManagement.Drivers;
using Coevery.ContentManagement.Handlers;
using Coevery.ContentManagement.MetaData;
using Coevery.Core.Common.Extensions;
using Coevery.Core.Projections.Models;
using Coevery.DeveloperTools.CodeGeneration.CodeGenerationTemplates;
using Coevery.DeveloperTools.FormDesigner.Models;
using FubuCore;
using FubuCsProjFile;
using Newtonsoft.Json;

namespace Coevery.DeveloperTools.CodeGeneration.Services {
    public class DynamicAssemblyBuilder : IDynamicAssemblyBuilder {
        private readonly IEnumerable<IContentFieldDriver> _contentFieldDrivers;
        private readonly IContentDefinitionExtension _contentDefinitionExtension;
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IGridColumn _gridColumn;

        public DynamicAssemblyBuilder(
            IEnumerable<IContentFieldDriver> contentFieldDrivers,
            IContentDefinitionExtension contentDefinitionExtension,
            IContentDefinitionManager contentDefinitionManager,
            ICoeveryServices coeveryServices,
            IGridColumn gridColumn) {
            Services = coeveryServices;
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
                AddViewFile(csProjFile, definition);
            }
            csProjFile.Save();
        }

        private void AddControllerFile(CsProjFile csProjFile, DynamicDefinition controllerDefinition) {

            string moduleControllersPath = Path.Combine(csProjFile.ProjectDirectory, "Controllers");
            CheckDirectories(moduleControllersPath);

            // Controller
            string controllerClassFilePath = Path.Combine(moduleControllersPath, string.Format("{0}Controller.cs", controllerDefinition.Name));
            var partTemplate = new ControllerTemplate() { Session = new Dictionary<string, object>() };
            partTemplate.Session["Namespace"] = csProjFile.RootNamespace;
            partTemplate.Session["ControllerName"] = controllerDefinition.Name;
            partTemplate.Initialize();
            AddFile<CodeFile>(csProjFile, controllerClassFilePath, partTemplate.TransformText());

            // Api controller
            string apiControllerClassFilePath = Path.Combine(moduleControllersPath, string.Format("{0}ApiController.cs", controllerDefinition.Name));
            var apiControllerTemplate = new ApiControllerTemplate() {Session = new Dictionary<string, object>()};
            apiControllerTemplate.Session["Namespace"] = csProjFile.RootNamespace;
            apiControllerTemplate.Session["ControllerName"] = controllerDefinition.Name;
            apiControllerTemplate.Initialize();
            AddFile<CodeFile>(csProjFile, apiControllerClassFilePath, apiControllerTemplate.TransformText());

        }

        private void AddDriverFile(CsProjFile csProjFile, DynamicDefinition driverDefinition) {
            string moduleDriversPath = Path.Combine(csProjFile.ProjectDirectory, "Drivers");

            CheckDirectories(moduleDriversPath);

            string partClassFilePath = Path.Combine(moduleDriversPath, driverDefinition.Name + "PartDriver.cs");
            var partTemplate = new DriverTemplate() {Session = new Dictionary<string, object>()};
            partTemplate.Session["Namespace"] = csProjFile.RootNamespace;
            partTemplate.Session["DriverName"] = driverDefinition.Name;
            partTemplate.Initialize();

            AddFile<CodeFile>(csProjFile, partClassFilePath, partTemplate.TransformText());
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

        private void AddViewFile(CsProjFile csProjFile, DynamicDefinition modelDefinition) {
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

            // {{EntityName}}/Create.cshtml
            string indexViewFilePath = Path.Combine(controllerViewPath, "Index.cshtml");
            var indexViewTemplate = new CreateViewTemplate();
            AddFile<Content>(csProjFile, indexViewFilePath, indexViewTemplate.TransformText());

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

            // Parts/ListView-{{EntityName}}.cshtml
            var query = Services.ContentManager.Query<ListViewPart, ListViewPartRecord>("ListViewPage")
                .Where(v => v.IsDefault).List().ToList().FirstOrDefault();

            var gridDefinition = (Object[])_gridColumn.Get(modelDefinition.Name, query.Id);

            string partsListViewFilePath = Path.Combine(partsViewPath, string.Format("ListView-{0}.cshtml", modelDefinition.Name));
            var partsListViewTemplate = new ListViewTemplate() { Session = new Dictionary<string, object>() };
            partsListViewTemplate.Session["GridDefinition"] = gridDefinition;
            partsListViewTemplate.Session["Namespace"] = csProjFile.RootNamespace;
            partsListViewTemplate.Session["ModuleName"] = modelDefinition.Name;
            partsListViewTemplate.Initialize();
            AddFile<Content>(csProjFile, partsListViewFilePath, partsListViewTemplate.TransformText());

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