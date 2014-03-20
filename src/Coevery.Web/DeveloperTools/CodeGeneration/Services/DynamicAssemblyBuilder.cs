﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using Coevery.ContentManagement;
using Coevery.ContentManagement.Drivers;
using Coevery.ContentManagement.Handlers;
using Coevery.ContentManagement.MetaData;
using Coevery.ContentManagement.Records;
using Coevery.Core.Common.Extensions;
using Coevery.Core.Entities.DynamicTypeGeneration;
using Coevery.Data;
using Coevery.DeveloperTools.CodeGeneration.CodeGenerationTemplates;
using Coevery.DeveloperTools.FormDesigner.Models;
using Coevery.FileSystems.VirtualPath;
using FubuCore;
using FubuCsProjFile;
using Newtonsoft.Json;

namespace Coevery.DeveloperTools.CodeGeneration.Services {
    public class DynamicAssemblyBuilder : IDynamicAssemblyBuilder {
        internal const string AssemblyName = "Coevery.DynamicTypes";
        private readonly IVirtualPathProvider _virtualPathProvider;
        private readonly IEnumerable<IContentFieldDriver> _contentFieldDrivers;
        private readonly IDynamicTypeGenerationEvents _dynamicTypeGenerationEvents;
        private readonly IContentDefinitionExtension _contentDefinitionExtension;
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public DynamicAssemblyBuilder(
            IVirtualPathProvider virtualPathProvider,
            IEnumerable<IContentFieldDriver> contentFieldDrivers,
            IContentDefinitionExtension contentDefinitionExtension,
            IDynamicTypeGenerationEvents dynamicTypeGenerationEvents,
            IContentDefinitionManager contentDefinitionManager) {
            _virtualPathProvider = virtualPathProvider;
            _contentFieldDrivers = contentFieldDrivers;
            _dynamicTypeGenerationEvents = dynamicTypeGenerationEvents;
            _contentDefinitionManager = contentDefinitionManager;
            _contentDefinitionExtension = contentDefinitionExtension;
        }

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
                AddViewFiles(csProjFile, definition);
            }
            csProjFile.Save();
        }

        private void AddModelClassFile(CsProjFile csProjFile, DynamicDefinition modelDefinition) {
            string moduleModelsPath = Path.Combine(csProjFile.ProjectDirectory, "Models");
            if (!moduleModelsPath.EndsWith(Path.DirectorySeparatorChar.ToString())) {
                moduleModelsPath += Path.DirectorySeparatorChar;
            }
            if (!Directory.Exists(moduleModelsPath)) {
                Directory.CreateDirectory(moduleModelsPath);
            }

            string partClassFilePath = moduleModelsPath + modelDefinition.Name + "Part.cs";
            if (File.Exists(partClassFilePath)) {
                //Context.Output.WriteLine(T("Controller {0} already exists in target Module {1}.", controllerName, moduleName));
                return;
            }
            var partTemplate = new ContentPartTemplate() {Session = new Dictionary<string, object>()};
            partTemplate.Session["Namespace"] = csProjFile.RootNamespace;
            partTemplate.Session["ModelDefinition"] = modelDefinition;
            partTemplate.Initialize();
            string partText = partTemplate.TransformText();
            File.WriteAllText(partClassFilePath, partText);

            var partRelativePath = partClassFilePath.PathRelativeTo(csProjFile.ProjectDirectory);
            csProjFile.Add<CodeFile>(partRelativePath);

            string recordClassFilePath = moduleModelsPath + modelDefinition.Name + "PartRecord.cs";
            if (File.Exists(recordClassFilePath)) {
                //Context.Output.WriteLine(T("Controller {0} already exists in target Module {1}.", controllerName, moduleName));
                return;
            }
            var recordTemplate = new ContentPartRecordTemplate() {Session = new Dictionary<string, object>()};
            recordTemplate.Session["Namespace"] = csProjFile.RootNamespace;
            recordTemplate.Session["ModelDefinition"] = modelDefinition;
            recordTemplate.Initialize();
            string recordText = recordTemplate.TransformText();
            File.WriteAllText(recordClassFilePath, recordText);

            var recordRelativePath = recordClassFilePath.PathRelativeTo(csProjFile.ProjectDirectory);
            csProjFile.Add<CodeFile>(recordRelativePath);
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