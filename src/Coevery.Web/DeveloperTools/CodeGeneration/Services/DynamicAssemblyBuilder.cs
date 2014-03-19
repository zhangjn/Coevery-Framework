using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
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

        public bool Build() {
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
                Build(userDefinedParts);
                return true;
            }
            return false;
        }

        private void Build(IEnumerable<DynamicDefinition> typeDefinitions) {
            //var assemblyBuidler = BuildAssembly();
            //var moduleBuidler = BuildModule(assemblyBuidler);
            var moduleName = "Sample.Lead";
            string moduleCsProjPath = HostingEnvironment.MapPath(string.Format("~/Modules/{0}/{0}.csproj", moduleName));
            var csProjFile = CsProjFile.LoadFrom(moduleCsProjPath);
            foreach (var definition in typeDefinitions) {
                AddModelClassFile(csProjFile, definition);
                AddViewFiles(csProjFile, definition);
                //if (!definition.Fields.Any()) {
                //    continue;
                //}
                //var typeBuidler = BuildType(definition, moduleBuidler);
                //var fieldBuilders = BuildFields(definition, typeBuidler).ToList();
                //BuildEmptyCtor(typeBuidler);
                //BuildCtor(typeBuidler, fieldBuilders);
                //BuildProperties(definition, typeBuidler, fieldBuilders, false);
                //Type type = typeBuidler.CreateType();

                //var partTypeBuidler = BuildPartType(definition, moduleBuidler, type);
                //var partFieldBuilders = BuildFields(definition, partTypeBuidler).ToList();
                //BuildPartEmptyCtor(partTypeBuidler, type);
                //BuildCtor(partTypeBuidler, partFieldBuilders);
                //BuildProperties(definition, partTypeBuidler, partFieldBuilders, true);
                //var contentPartType = partTypeBuidler.CreateType();

                //var driverTypeBuidler = BuildDriverType(definition, moduleBuidler, contentPartType);
                //driverTypeBuidler.CreateType();

                //var handlerTypeBuidler = BuildHandlerType(definition, moduleBuidler, type);
                //BuildHandlerCtor(handlerTypeBuidler, type);
                //handlerTypeBuidler.CreateType();
            }
            csProjFile.Save();
            //_dynamicTypeGenerationEvents.OnBuilded(moduleBuidler);
            //assemblyBuidler.Save(AssemblyName + ".dll");
        }

        private static void BuildHandlerCtor(TypeBuilder typBuilder, Type type) {
            Type paramType = typeof (IRepository<>);
            var paramGenericType = paramType.MakeGenericType(type);
            var ctorBuilder = typBuilder.DefineConstructor(
                MethodAttributes.Public |
                MethodAttributes.SpecialName |
                MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                new Type[1] {paramGenericType});

            var generator = ctorBuilder.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            Type contentType = typeof (DynamicContentsHandler<>);
            var genericContentType = contentType.MakeGenericType(type);
            var baseCtorInfo = genericContentType.GetConstructor(new Type[1] {paramGenericType});
            generator.Emit(OpCodes.Call, baseCtorInfo);
            generator.Emit(OpCodes.Ret);
        }

        private static void BuildPartEmptyCtor(TypeBuilder typBuilder, Type type) {
            var ctorBuilder = typBuilder.DefineConstructor(
                MethodAttributes.Public |
                MethodAttributes.SpecialName |
                MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                new Type[0]);

            var generator = ctorBuilder.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            Type contentType = typeof (ContentPart<>);
            var genericContentType = contentType.MakeGenericType(type);
            var baseCtorInfo = genericContentType.GetConstructor(new Type[0]);
            generator.Emit(OpCodes.Call, baseCtorInfo);
            generator.Emit(OpCodes.Ret);
        }


        private AssemblyBuilder BuildAssembly() {
            AppDomain aDomain = AppDomain.CurrentDomain;

            // Build the assembly
            var asmName = new AssemblyName {Name = AssemblyName};
            var directory = GetAssemblyDirectory();
            var asmBuilder = aDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave, directory);
            return asmBuilder;
        }

        private string GetAssemblyDirectory() {
            var virtualPath = _virtualPathProvider.Combine("~/Modules/" + AssemblyName, "bin");
            _virtualPathProvider.CreateDirectory(virtualPath);
            return _virtualPathProvider.MapPath(virtualPath);
        }

        private ModuleBuilder BuildModule(AssemblyBuilder asmBuilder) {
            // Build the module
            var fileName = AssemblyName + ".dll";
            ModuleBuilder modBuilder = asmBuilder.DefineDynamicModule(AssemblyName, fileName);
            return modBuilder;
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
            AddFile(csProjFile, createViewFilePath, createViewTemplate.TransformText());

            // {{EntityName}}/Edit.cshtml
            string editViewFilePath = Path.Combine(controllerViewPath, "Edit.cshtml");
            var editViewTemplate = new EditViewTemplate();
            AddFile(csProjFile, editViewFilePath, editViewTemplate.TransformText());

            // {{EntityName}}/Detail.cshtml
            string detailViewFilePath = Path.Combine(controllerViewPath, "Detail.cshtml");
            var detailViewTemplate = new DetailViewTemplate();
            AddFile(csProjFile, detailViewFilePath, detailViewTemplate.TransformText());

            // Parts/CreateView-{{EntityName}}.cshtml
            var contentTypeDefinition = _contentDefinitionManager.GetTypeDefinition(modelDefinition.Name);
            var sectionList = contentTypeDefinition.Settings.ContainsKey("Layout")
                ? JsonConvert.DeserializeObject<ICollection<Section>>(contentTypeDefinition.Settings["Layout"])
                : Enumerable.Empty<Section>();

            string partsCreateViewFilePath = Path.Combine(partsViewPath, string.Format("CreateView-{0}.cshtml", modelDefinition.Name));
            var partsCreateViewTemplate = new PartsCreateViewTemplate() {Session = new Dictionary<string, object>()};
            partsCreateViewTemplate.Session["ModuleName"] = modelDefinition.Name;
            partsCreateViewTemplate.Session["SectionList"] = sectionList;
            partsCreateViewTemplate.Initialize();
            AddFile(csProjFile, partsCreateViewFilePath, partsCreateViewTemplate.TransformText());

            // Parts/EditView-{{EntityName}}.cshtml
            string partsEditViewFilePath = Path.Combine(partsViewPath, string.Format("EditView-{0}.cshtml", modelDefinition.Name));
            var partsEditViewTemplate = new PartsEditViewTemplate() { Session = new Dictionary<string, object>() };
            partsEditViewTemplate.Session["ModuleName"] = modelDefinition.Name;
            partsEditViewTemplate.Session["SectionList"] = sectionList;
            partsEditViewTemplate.Initialize();
            AddFile(csProjFile, partsEditViewFilePath, partsEditViewTemplate.TransformText());

            // Parts/DetailView-{{EntityName}}.cshtml
            string partsDetailViewFilePath = Path.Combine(partsViewPath, string.Format("DetailView-{0}.cshtml", modelDefinition.Name));
            var partsDetailViewTemplate = new PartsDetailViewTemplate() { Session = new Dictionary<string, object>() };
            partsDetailViewTemplate.Session["SectionList"] = sectionList;
            partsDetailViewTemplate.Initialize();
            AddFile(csProjFile, partsDetailViewFilePath, partsDetailViewTemplate.TransformText());
        }

        private void AddFile(CsProjFile csProjFile, string path, string text) {
            File.WriteAllText(path, text);
            var relativePath = path.PathRelativeTo(csProjFile.ProjectDirectory);
            csProjFile.Add<CodeFile>(relativePath);
        }

        private void CheckDirectories(params string[] paths) {
            foreach (var path in paths) {
                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }
            }
        }

        private static TypeBuilder BuildType(DynamicDefinition definition, ModuleBuilder modBuilder) {
            // Build the type
            var typeName = string.Format("{0}.{1}.{2}PartRecord", AssemblyName, "Records", definition.Name);
            var typBuilder = modBuilder.DefineType(typeName,
                TypeAttributes.Public |
                TypeAttributes.Class |
                TypeAttributes.AutoClass |
                TypeAttributes.AnsiClass |
                TypeAttributes.BeforeFieldInit |
                TypeAttributes.AutoLayout, typeof (ContentPartVersionRecord));
            return typBuilder;
        }

        private static TypeBuilder BuildPartType(DynamicDefinition definition, ModuleBuilder modBuilder, Type type) {
            // Build the type
            var typeName = string.Format("{0}.{1}.{2}Part", AssemblyName, "Records", definition.Name);
            Type contentType = typeof (ContentPart<>);
            var genericContentType = contentType.MakeGenericType(type);
            var typBuilder = modBuilder.DefineType(typeName,
                TypeAttributes.Public |
                TypeAttributes.Class |
                TypeAttributes.AutoClass |
                TypeAttributes.AnsiClass |
                TypeAttributes.BeforeFieldInit |
                TypeAttributes.AutoLayout, genericContentType);
            return typBuilder;
        }

        private static TypeBuilder BuildDriverType(DynamicDefinition definition, ModuleBuilder modBuilder, Type type) {
            // Build the type
            var typeName = string.Format("{0}.{1}.{2}PartDriver", AssemblyName, "Records", definition.Name);
            Type driverType = typeof (DynamicContentsDriver<>);
            var genericContentType = driverType.MakeGenericType(type);
            var typBuilder = modBuilder.DefineType(typeName,
                TypeAttributes.Public |
                TypeAttributes.Class |
                TypeAttributes.AutoClass |
                TypeAttributes.AnsiClass |
                TypeAttributes.BeforeFieldInit |
                TypeAttributes.AutoLayout, genericContentType);
            return typBuilder;
        }

        private static TypeBuilder BuildHandlerType(DynamicDefinition definition, ModuleBuilder modBuilder, Type type) {
            // Build the type
            var typeName = string.Format("{0}.{1}.{2}PartHandler", AssemblyName, "Records", definition.Name);
            Type contentType = typeof (DynamicContentsHandler<>);
            var genericContentType = contentType.MakeGenericType(type);
            var typBuilder = modBuilder.DefineType(typeName,
                TypeAttributes.Public |
                TypeAttributes.Class |
                TypeAttributes.AutoClass |
                TypeAttributes.AnsiClass |
                TypeAttributes.BeforeFieldInit |
                TypeAttributes.AutoLayout, genericContentType);
            return typBuilder;
        }

        private static IEnumerable<FieldBuilder> BuildFields(DynamicDefinition definition, TypeBuilder typBuilder) {
            return definition.Fields.Select(field => typBuilder.DefineField("_" + field.Name, field.Type,
                FieldAttributes.Private | FieldAttributes.InitOnly));
        }

        private static void BuildEmptyCtor(TypeBuilder typBuilder) {
            var ctorBuilder = typBuilder.DefineConstructor(
                MethodAttributes.Public |
                MethodAttributes.SpecialName |
                MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                new Type[0]);

            var generator = ctorBuilder.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            var baseCtorInfo = typeof (Object).GetConstructor(new Type[0]);
            generator.Emit(OpCodes.Call, baseCtorInfo);
            generator.Emit(OpCodes.Ret);
        }

        private static void BuildCtor(TypeBuilder typBuilder,
            List<FieldBuilder> fieldBuilders) {
            Type[] ctorParams = fieldBuilders.Select(f => f.FieldType).ToArray();
            var ctorBuilder = typBuilder.DefineConstructor(
                MethodAttributes.Public |
                MethodAttributes.SpecialName |
                MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                ctorParams);

            ILGenerator ctorIL = ctorBuilder.GetILGenerator();
            ctorIL.Emit(OpCodes.Ldarg_0);

            //Get the base constructor
            var baseCtorInfo = typeof (Object).GetConstructor(new Type[0]);
            ctorIL.Emit(OpCodes.Call, baseCtorInfo);

            for (byte i = 0; i < fieldBuilders.Count; i++) {
                ctorIL.Emit(OpCodes.Ldarg_0);
                if (i == 0) {
                    ctorIL.Emit(OpCodes.Ldarg_1);
                }
                else if (i == 1) {
                    ctorIL.Emit(OpCodes.Ldarg_2);
                }
                else if (i == 2) {
                    ctorIL.Emit(OpCodes.Ldarg_3);
                }
                else {
                    ctorIL.Emit(OpCodes.Ldarg_S, i + 1);
                }
                ctorIL.Emit(OpCodes.Stfld, fieldBuilders[i]);
            }
            ctorIL.Emit(OpCodes.Ret);
        }

        private static void BuildProperties(DynamicDefinition definition,
            TypeBuilder typBuilder,
            List<FieldBuilder> fieldBuilders, bool userParanet) {
            var fields = definition.Fields.ToList();
            for (int i = 0; i < fields.Count; i++) {
                var propBuilder = typBuilder.DefineProperty(
                    fields[i].Name, PropertyAttributes.HasDefault, fields[i].Type, Type.EmptyTypes);

                // Build Get prop
                var getMethBuilder = typBuilder.DefineMethod(
                    "get_" + fields[i].Name, MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                    fields[i].Type, Type.EmptyTypes);
                var generator = getMethBuilder.GetILGenerator();
                if (userParanet) {
                    MethodInfo getRecord = typBuilder.BaseType.GetProperty("Record").GetGetMethod();
                    MethodInfo mi = typBuilder.BaseType.GetProperty("Record").PropertyType.GetProperty(fields[i].Name).GetGetMethod();
                    generator.DeclareLocal(fields[i].Type);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Call, getRecord);
                    generator.Emit(OpCodes.Callvirt, mi);
                    generator.Emit(OpCodes.Stloc_0);
                    Label targetInstruction = generator.DefineLabel();
                    generator.Emit(OpCodes.Br_S, targetInstruction);
                    generator.MarkLabel(targetInstruction);
                    generator.Emit(OpCodes.Ldloc_0);
                    generator.Emit(OpCodes.Ret);
                }
                else {
                    generator.Emit(OpCodes.Ldarg_0); // load 'this'
                    generator.Emit(OpCodes.Ldfld, fieldBuilders[i]); // load the field
                    generator.Emit(OpCodes.Ret);
                }

                propBuilder.SetGetMethod(getMethBuilder);

                // Build Set prop
                var setMethBuilder = typBuilder.DefineMethod(
                    "set_" + fields[i].Name, MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                    typeof (void), new[] {fieldBuilders[i].FieldType});
                generator = setMethBuilder.GetILGenerator();
                if (userParanet) {
                    MethodInfo rmi = typBuilder.BaseType.GetProperty("Record").GetGetMethod();
                    MethodInfo mi = typBuilder.BaseType.GetProperty("Record").PropertyType.GetProperty(fields[i].Name).GetSetMethod();
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Call, rmi);
                    generator.Emit(OpCodes.Ldarg_1);
                    generator.Emit(OpCodes.Call, mi);
                    generator.Emit(OpCodes.Ret);
                }
                else {
                    generator.Emit(OpCodes.Ldarg_0); // load 'this'
                    generator.Emit(OpCodes.Ldarg_1); // load value
                    generator.Emit(OpCodes.Stfld, fieldBuilders[i]);
                    generator.Emit(OpCodes.Ret);
                }

                propBuilder.SetSetMethod(setMethBuilder);
            }
        }
    }
}