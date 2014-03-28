using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using Coevery.ContentManagement.Drivers;
using Coevery.ContentManagement.Handlers;
using Coevery.ContentManagement.MetaData;
using Coevery.Core.Common.Extensions;
using Coevery.Core.Projections.Services;
using Coevery.DeveloperTools.FormDesigner.Models;
using Coevery.Localization;
using FubuCore;
using FubuCsProjFile;
using Microsoft.VisualStudio.TextTemplating;
using Newtonsoft.Json;

namespace Coevery.DeveloperTools.CodeGeneration.Services {
    public class DynamicAssemblyBuilder : IDynamicAssemblyBuilder {
        private readonly IEnumerable<IContentFieldDriver> _contentFieldDrivers;
        private readonly IContentDefinitionExtension _contentDefinitionExtension;
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IProjectionManager _projectionManager;

        public DynamicAssemblyBuilder(
            IEnumerable<IContentFieldDriver> contentFieldDrivers,
            IContentDefinitionExtension contentDefinitionExtension,
            IContentDefinitionManager contentDefinitionManager,
            ICoeveryServices coeveryServices, 
            IProjectionManager projectionManager) {
            Services = coeveryServices;
            _projectionManager = projectionManager;
            _contentFieldDrivers = contentFieldDrivers;
            _contentDefinitionManager = contentDefinitionManager;
            _contentDefinitionExtension = contentDefinitionExtension;
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
                AddHandlerFile(csProjFile, definition);
                AddViewFile(csProjFile, definition);
            }

            AddRoute(csProjFile, typeDefinitions);

            AddFrontMenuFile(csProjFile, typeDefinitions);
            csProjFile.Save();
        }

        private void AddFrontMenuFile(CsProjFile csProjFile, IEnumerable<DynamicDefinition> typeDefinitions) {

            var frontMenuSession = new TextTemplatingSession();
            frontMenuSession["Namespace"] = csProjFile.RootNamespace;
            frontMenuSession["TypeDefinitions"] = typeDefinitions;

            string frontMenuClassFilePath = Path.Combine(csProjFile.ProjectDirectory, "FrontMenu.cs");
            string frontMenuTemplate = TemplateHelper.ProcessTemplate("FrontMenu.tt", frontMenuSession);
            AddFile<CodeFile>(csProjFile, frontMenuClassFilePath, frontMenuTemplate);
        }

        private void AddRoute(CsProjFile csProjFile, IEnumerable<DynamicDefinition> modelDefinition)
        {
            string routePath = csProjFile.ProjectDirectory;

            // Route
            var routeSession = new TextTemplatingSession();
            routeSession["Namespace"] = csProjFile.RootNamespace;
            routeSession["AreaName"] = csProjFile.AssemblyName;
            routeSession["ModelDefinition"] = modelDefinition;

            string routeFilePath = Path.Combine(routePath, "Route.cs");
            string routeTemplate = TemplateHelper.ProcessTemplate("Route.tt", routeSession);
            AddFile<CodeFile>(csProjFile, routeFilePath, routeTemplate);
        }

        private void AddControllerFile(CsProjFile csProjFile, DynamicDefinition modelDefinition) {

            string moduleControllersPath = Path.Combine(csProjFile.ProjectDirectory, "Controllers");
            CheckDirectories(moduleControllersPath);

            // Controller
            var controllerSession = new TextTemplatingSession();
            controllerSession["ModelDefinition"] = modelDefinition;
            controllerSession["Namespace"] = csProjFile.RootNamespace;
            controllerSession["ContentManager"] = Services.ContentManager;
            controllerSession["ProjectionManager"] = _projectionManager;
            controllerSession["T"] = T;

            string controllerClassFilePath = Path.Combine(moduleControllersPath, string.Format("{0}Controller.cs", modelDefinition.Name));
            string controllerTemplate = TemplateHelper.ProcessTemplate("Controller.tt", controllerSession);
            AddFile<CodeFile>(csProjFile, controllerClassFilePath, controllerTemplate);
        }

        private void AddDriverFile(CsProjFile csProjFile, DynamicDefinition modelDefinition) {
            string moduleDriversPath = Path.Combine(csProjFile.ProjectDirectory, "Drivers");
            CheckDirectories(moduleDriversPath);

            var driverSession = new TextTemplatingSession();
            driverSession["Namespace"] = csProjFile.RootNamespace;
            driverSession["EntityName"] = modelDefinition.Name;

            string partClassFilePath = Path.Combine(moduleDriversPath, modelDefinition.Name + "PartDriver.cs");
            string driverTemplate = TemplateHelper.ProcessTemplate("Driver.tt", driverSession);
            AddFile<CodeFile>(csProjFile, partClassFilePath, driverTemplate);
        }

        private void AddModelClassFile(CsProjFile csProjFile, DynamicDefinition modelDefinition) {
            string moduleModelsPath = Path.Combine(csProjFile.ProjectDirectory, "Models");
            CheckDirectories(moduleModelsPath);

            var modelClassSession = new TextTemplatingSession();
            modelClassSession["Namespace"] = csProjFile.RootNamespace;
            modelClassSession["ModelDefinition"] = modelDefinition;

            string partClassFilePath = Path.Combine(moduleModelsPath, modelDefinition.Name + "Part.cs");
            string contentPart = TemplateHelper.ProcessTemplate("ContentPart.tt", modelClassSession);
            AddFile<CodeFile>(csProjFile, partClassFilePath, contentPart);

            string contentPartRecord = TemplateHelper.ProcessTemplate("ContentPartRecord.tt", modelClassSession);
            string recordClassFilePath = Path.Combine(moduleModelsPath, modelDefinition.Name + "PartRecord.cs");
            AddFile<CodeFile>(csProjFile, recordClassFilePath, contentPartRecord);
        }

        private void AddHandlerFile(CsProjFile csProjFile, DynamicDefinition modelDefinition) {
            string handlersPath = Path.Combine(csProjFile.ProjectDirectory, "Handlers");
            CheckDirectories(handlersPath);

            var handlerSession = new TextTemplatingSession();
            handlerSession["EntityName"] = modelDefinition.Name;
            handlerSession["Namespace"] = csProjFile.RootNamespace;

            string handlerFilePath = Path.Combine(handlersPath, string.Format("{0}PartHandler.cs", modelDefinition.Name));
            string handlerPart = TemplateHelper.ProcessTemplate("Handler.tt", handlerSession);
            AddFile<CodeFile>(csProjFile, handlerFilePath, handlerPart);
        }

        private void AddViewFile(CsProjFile csProjFile, DynamicDefinition modelDefinition) {
            string viewsPath = Path.Combine(csProjFile.ProjectDirectory, "Views");

            string controllerViewPath = Path.Combine(viewsPath, modelDefinition.Name);
            string partsViewPath = Path.Combine(viewsPath, "Parts");
            string viewModelsPath = Path.Combine(csProjFile.ProjectDirectory, "ViewModels");
            CheckDirectories(viewsPath, controllerViewPath, partsViewPath, viewModelsPath);

            // {{EntityName}}/Create.cshtml
            string createViewFilePath = Path.Combine(controllerViewPath, "Create.cshtml");
            string createViewTemplate = TemplateHelper.ProcessTemplate("CreateView.tt");
            AddFile<Content>(csProjFile, createViewFilePath, createViewTemplate);

            // {{EntityName}}/Edit.cshtml
            string editViewFilePath = Path.Combine(controllerViewPath, "Edit.cshtml");
            string editViewTemplate = TemplateHelper.ProcessTemplate("EditView.tt");
            AddFile<Content>(csProjFile, editViewFilePath, editViewTemplate);

            // {{EntityName}}/Index.cshtml
            string indexViewFilePath = Path.Combine(controllerViewPath, "Index.cshtml");
            string indexViewTemplate = TemplateHelper.ProcessTemplate("IndexView.tt");
            AddFile<Content>(csProjFile, indexViewFilePath, indexViewTemplate);

            // {{EntityName}}/Detail.cshtml
            string detailViewFilePath = Path.Combine(controllerViewPath, "Detail.cshtml");
            string detailViewTemplate = TemplateHelper.ProcessTemplate("DetailView.tt");
            AddFile<Content>(csProjFile, detailViewFilePath, detailViewTemplate);


            // Parts/CreateView-{{EntityName}}.cshtml
            var contentTypeDefinition = _contentDefinitionManager.GetTypeDefinition(modelDefinition.Name);
            var sectionList = contentTypeDefinition.Settings.ContainsKey("Layout")
                ? JsonConvert.DeserializeObject<IList<Section>>(contentTypeDefinition.Settings["Layout"])
                : Enumerable.Empty<Section>();

            var partsCreateViewSession = new TextTemplatingSession();
            partsCreateViewSession["EntityName"] = modelDefinition.Name;
            partsCreateViewSession["SectionList"] = sectionList;

            string partsCreateViewFilePath = Path.Combine(partsViewPath, string.Format("CreateView-{0}.cshtml", modelDefinition.Name));
            string partsCreateViewTemplate = TemplateHelper.ProcessTemplate("PartsCreateView.tt", partsCreateViewSession);
            AddFile<Content>(csProjFile, partsCreateViewFilePath, partsCreateViewTemplate);

            // Parts/ListView-{{EntityName}}.cshtml
            var partsListViewSession = new TextTemplatingSession();
            partsListViewSession["EntityTypeName"] = modelDefinition.Name;
            partsListViewSession["Namespace"] = csProjFile.RootNamespace;
            partsListViewSession["ViewName"] = modelDefinition.Name;
            partsListViewSession["ContentManager"] = Services.ContentManager;
            partsListViewSession["ProjectionManager"] = _projectionManager;
            partsListViewSession["T"] = T;

            string partsListViewFilePath = Path.Combine(partsViewPath, string.Format("ListView-{0}.cshtml", modelDefinition.Name));
            string partsListViewTemplate = TemplateHelper.ProcessTemplate("ListView.tt", partsListViewSession);
            AddFile<Content>(csProjFile, partsListViewFilePath, partsListViewTemplate);

            //ViewModels/{EntityName}ListViewModel.cs
            var listViewModelSession = new TextTemplatingSession();
            listViewModelSession["ModelDefinition"] = modelDefinition;
            listViewModelSession["Namespace"] = csProjFile.RootNamespace;
            listViewModelSession["ContentManager"] = Services.ContentManager;
            listViewModelSession["ProjectionManager"] = _projectionManager;
            listViewModelSession["T"] = T;

            string listViewModelFilePath = Path.Combine(viewModelsPath, string.Format("{0}ListViewModel.cs", modelDefinition.Name));
            string listViewModelTemplate = TemplateHelper.ProcessTemplate("ListViewModel.tt", listViewModelSession);
            AddFile<CodeFile>(csProjFile, listViewModelFilePath, listViewModelTemplate);

            // Parts/EditView-{{EntityName}}.cshtml
            var partsEditViewSession = new TextTemplatingSession();
            partsEditViewSession["EntityName"] = modelDefinition.Name;
            partsEditViewSession["SectionList"] = sectionList;

            string partsEditViewFilePath = Path.Combine(partsViewPath, string.Format("EditView-{0}.cshtml", modelDefinition.Name));
            string partsEditViewTemplate = TemplateHelper.ProcessTemplate("PartsEditView.tt", partsEditViewSession);
            AddFile<Content>(csProjFile, partsEditViewFilePath, partsEditViewTemplate);

            // Parts/DetailView-{{EntityName}}.cshtml
            var partsDetailViewSession = new TextTemplatingSession();
            partsDetailViewSession["EntityName"] = modelDefinition.Name;
            partsDetailViewSession["SectionList"] = sectionList;

            string partsDetailViewFilePath = Path.Combine(partsViewPath, string.Format("DetailView-{0}.cshtml", modelDefinition.Name));
            string partsDetailViewTemplate = TemplateHelper.ProcessTemplate("PartsDetailView.tt", partsDetailViewSession);
            AddFile<Content>(csProjFile, partsDetailViewFilePath, partsDetailViewTemplate);
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