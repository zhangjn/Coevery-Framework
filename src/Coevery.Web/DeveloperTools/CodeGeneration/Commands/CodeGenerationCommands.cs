using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using Coevery.Commands;
using Coevery.Data.Migration.Generator;
using Coevery.Data.Migration.Schema;
using Coevery.DeveloperTools.CodeGeneration.CodeGenerationTemplates;
using Coevery.DeveloperTools.CodeGeneration.Services;
using Coevery.Environment.Extensions;
using Coevery.Environment.Extensions.Models;
using FubuCsProjFile;
using FubuCsProjFile.MSBuild;

namespace Coevery.DeveloperTools.CodeGeneration.Commands {

    public class CodeGenerationCommands : DefaultCoeveryCommandHandler {
        private readonly IExtensionManager _extensionManager;
        private readonly ISchemaCommandGenerator _schemaCommandGenerator;
        private const string SolutionDirectoryModules = "E9C9F120-07BA-4DFB-B9C3-3AFB9D44C9D5";
        private const string SolutionDirectoryTests = "74E681ED-FECC-4034-B9BD-01B0BB1BDECA";
        private const string SolutionDirectoryThemes = "74492CBC-7201-417E-BC29-28B4C25A58B0";

        private static readonly string[] _themeDirectories = new[] {
	        "", "Content", "Styles", "Scripts", "Views"
        };
        private static readonly string[] _moduleDirectories = new[] {
	        "", "Properties", "Controllers", "Views", "Models", "Scripts", "Styles"
        };
        private static readonly string[] _moduleTestsDirectories = new[] {
            "", "Properties"
        };

        private const string ModuleName = "CodeGeneration";
        private static readonly string _codeGenTemplatePath = HostingEnvironment.MapPath("~/DeveloperTools/CodeGeneration/CodeGenerationTemplates/");
        private static readonly string _WebRootProj = HostingEnvironment.MapPath("~/");
        private static readonly string _CoeveryThemesProj = HostingEnvironment.MapPath("~/Themes/Themes.csproj");

        public CodeGenerationCommands(
            IExtensionManager extensionManager,
            ISchemaCommandGenerator schemaCommandGenerator) {
            _extensionManager = extensionManager;
            _schemaCommandGenerator = schemaCommandGenerator;

            // Default is to include in the solution when generating modules / themes
            IncludeInSolution = true;
        }

        [CoeverySwitch]
        public bool IncludeInSolution { get; set; }

        [CoeverySwitch]
        public bool CreateProject { get; set; }

        [CoeverySwitch]
        public string BasedOn { get; set; }

        [CommandHelp("codegen datamigration <feature-name> \r\n\t" + "Create a new Data Migration class")]
        [CommandName("codegen datamigration")]
        public void CreateDataMigration(string featureName) {
            Context.Output.WriteLine(T("Creating Data Migration for {0}", featureName));
            ExtensionDescriptor extensionDescriptor = _extensionManager.AvailableExtensions().FirstOrDefault(extension => DefaultExtensionTypes.IsModule(extension.ExtensionType) &&
                                                                                                             extension.Features.Any(feature => String.Equals(feature.Id, featureName, StringComparison.OrdinalIgnoreCase)));

            if (extensionDescriptor == null) {
                Context.Output.WriteLine(T("Creating data migration failed: target Feature {0} could not be found.", featureName));
                return;
            }

            string dataMigrationFolderPath = HostingEnvironment.MapPath("~/Modules/" + extensionDescriptor.Id + "/");
            string dataMigrationFilePath = dataMigrationFolderPath + "Migrations.cs";
            string moduleCsProjPath = HostingEnvironment.MapPath(string.Format("~/Modules/{0}/{0}.csproj", extensionDescriptor.Id));

            if (!Directory.Exists(dataMigrationFolderPath)) {
                Directory.CreateDirectory(dataMigrationFolderPath);
            }

            if (File.Exists(dataMigrationFilePath)) {
                Context.Output.WriteLine(T("Data migration already exists in target Module {0}.", extensionDescriptor.Id));
                return;
            }

            List<SchemaCommand> commands = _schemaCommandGenerator.GetCreateFeatureCommands(featureName, false).ToList();
            string dataMigrationText;
            using (var stringWriter = new StringWriter()) {
                var interpreter = new CodeGenerationCommandInterpreter(stringWriter);

                foreach (var command in commands) {
                    interpreter.Visit(command);
                    stringWriter.WriteLine();
                }
                var migrationTemplate = new DataMigration {Session = new Dictionary<string, object>()};
                migrationTemplate.Session["FeatureName"] = featureName;
                migrationTemplate.Session["Commands"] = stringWriter.ToString();
                migrationTemplate.Initialize();
                dataMigrationText = migrationTemplate.TransformText();
            }
            File.WriteAllText(dataMigrationFilePath, dataMigrationText);

            var projectFile = CsProjFile.LoadFrom(moduleCsProjPath);
            projectFile.Add<CodeFile>("Migrations.cs");
            projectFile.Save();
            
            TouchSolution(Context.Output);
            Context.Output.WriteLine(T("Data migration created successfully in Module {0}", extensionDescriptor.Id));
        }

        [CommandHelp("codegen module <module-name> [/IncludeInSolution:true|false]\r\n\t" + "Create a new Coevery module")]
        [CommandName("codegen module")]
        [CoeverySwitches("IncludeInSolution")]
        public void CreateModule(string moduleName) {
            Context.Output.WriteLine(T("Creating Module {0}", moduleName));

            if (_extensionManager.AvailableExtensions().Any(extension => String.Equals(moduleName, extension.Name, StringComparison.OrdinalIgnoreCase))) {
                Context.Output.WriteLine(T("Creating Module {0} failed: a module of the same name already exists", moduleName));
                return;
            }

            IntegrateModule(moduleName);
            Context.Output.WriteLine(T("Module {0} created successfully", moduleName));
        }

        [CommandHelp("codegen moduletests <module-name> [/IncludeInSolution:true|false]\r\n\t" + "Creates a new test project for a module")]
        [CommandName("codegen moduletests")]
        [CoeverySwitches("IncludeInSolution")]
        public void CreateModuleTests(string moduleName) {
            var projectName = moduleName + ".Tests";

            Context.Output.WriteLine(T("Creating module tests project {0}", projectName));

            var testsPath = HostingEnvironment.MapPath("~/Modules/" + moduleName + "/" + projectName + "/");

            if (Directory.Exists(testsPath)) {
                Context.Output.WriteLine(T("Creating module tests project {0} failed: a project of the same name already exists", projectName));
                return;
            }

            var propertiesPath = testsPath + "Properties";
            var content = new HashSet<string>();
            var folders = new HashSet<string>();

            foreach (var folder in _moduleTestsDirectories) {
                Directory.CreateDirectory(testsPath + folder);
                if (!String.IsNullOrEmpty(folder)) {
                    folders.Add(testsPath + folder);
                }
            }

            var csprojTemplate = new ModuleTestsCsProj();
            var csprojText = csprojTemplate.TransformText();
            var project = MSBuildProject.Parse(projectName, csprojText);
            var csProject = new CsProjFile(testsPath + projectName + ".csproj", project);
            SetProjectGuid(csProject);

            var assemblyInfoTemplate = new ModuleAssemblyInfo();
            assemblyInfoTemplate.Session = new Dictionary<string, object>();
            assemblyInfoTemplate.Session["ModuleName"] = moduleName;
            assemblyInfoTemplate.Session["ModuleTypeLibGuid"] = csProject.ProjectGuid;
            assemblyInfoTemplate.Initialize();
            string templateText = assemblyInfoTemplate.TransformText();
            File.WriteAllText(propertiesPath + "\\AssemblyInfo.cs", templateText);
            content.Add(propertiesPath + "\\AssemblyInfo.cs");

            CreateProjectItemGroup(csProject, testsPath, content, folders);
            AddCoeveryReferences(csProject);
            csProject.Save();

            // The string searches in solution/project files can be made aware of comment lines.
            if (IncludeInSolution) {
                AddToSolution(Context.Output, csProject, SolutionDirectoryTests);
            }

            Context.Output.WriteLine(T("Module tests project {0} created successfully", projectName));
        }

        [CommandName("codegen theme")]
        [CommandHelp("codegen theme <theme-name> [/CreateProject:true|false][/IncludeInSolution:true|false][/BasedOn:<theme-name>]\r\n\tCreate a new Coevery theme")]
        [CoeverySwitches("IncludeInSolution,BasedOn,CreateProject")]
        public void CreateTheme(string themeName) {
            Context.Output.WriteLine(T("Creating Theme {0}", themeName));
            if (_extensionManager.AvailableExtensions().Any(extension => String.Equals(themeName, extension.Id, StringComparison.OrdinalIgnoreCase))) {
                Context.Output.WriteLine(T("Creating Theme {0} failed: an extention of the same name already exists", themeName));
                return;
            }

            if (!string.IsNullOrEmpty(BasedOn)) {
                if (!_extensionManager.AvailableExtensions().Any(extension =>
                    string.Equals(extension.ExtensionType, DefaultExtensionTypes.Theme, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(BasedOn, extension.Id, StringComparison.OrdinalIgnoreCase))) {
                    Context.Output.WriteLine(T("Creating Theme {0} failed: base theme named {1} was not found.", themeName, BasedOn));
                    return;
                }
            }
            IntegrateTheme(themeName, BasedOn);
            Context.Output.WriteLine(T("Theme {0} created successfully", themeName));
        }

        [CommandHelp("codegen controller <module-name> <controller-name>\r\n\t" + "Create a new Coevery controller in a module")]
        [CommandName("codegen controller")]
        public void CreateController(string moduleName, string controllerName) {
            Context.Output.WriteLine(T("Creating Controller {0} in Module {1}", controllerName, moduleName));

            ExtensionDescriptor extensionDescriptor = _extensionManager.AvailableExtensions().FirstOrDefault(extension => DefaultExtensionTypes.IsModule(extension.ExtensionType) &&
                                                                                                             string.Equals(moduleName, extension.Name, StringComparison.OrdinalIgnoreCase));

            if (extensionDescriptor == null) {
                Context.Output.WriteLine(T("Creating Controller {0} failed: target Module {1} could not be found.", controllerName, moduleName));
                return;
            }

            string moduleControllersPath = HostingEnvironment.MapPath("~/Modules/" + extensionDescriptor.Id + "/Controllers/");
            string controllerPath = moduleControllersPath + controllerName + ".cs";
            string moduleCsProjPath = HostingEnvironment.MapPath(string.Format("~/Modules/{0}/{0}.csproj", extensionDescriptor.Id));

            if (!Directory.Exists(moduleControllersPath)) {
                Directory.CreateDirectory(moduleControllersPath);
            }
            if (File.Exists(controllerPath)) {
                Context.Output.WriteLine(T("Controller {0} already exists in target Module {1}.", controllerName, moduleName));
                return;
            }

            var controllerTemplate = new ControllerTemplate() {Session = new Dictionary<string, object>()};
            controllerTemplate.Session["ModuleName"] = moduleName;
            controllerTemplate.Session["ControllerName"] = controllerName;
            controllerTemplate.Initialize();
            string controllerText = controllerTemplate.TransformText();
            File.WriteAllText(controllerPath, controllerText);

            var project = CsProjFile.LoadFrom(moduleCsProjPath);
            project.Add<CodeFile>("Controllers\\" + controllerName + ".cs");
            project.Save();
            Context.Output.WriteLine(T("Controller {0} created successfully in Module {1}", controllerName, moduleName));
            TouchSolution(Context.Output);
        }

        private void IntegrateModule(string moduleName) {

            var project = CreateFilesFromTemplates(moduleName);
            // The string searches in solution/project files can be made aware of comment lines.
            if (IncludeInSolution) {
                AddToSolution(Context.Output, project, SolutionDirectoryModules);
            }
        }

        private void IntegrateTheme(string themeName, string baseTheme) {
            CreateThemeFromTemplates(Context.Output,
                themeName,
                baseTheme,
                CreateProject,
                IncludeInSolution);
        }

        private CsProjFile CreateFilesFromTemplates(string moduleName) {
            string modulePath = HostingEnvironment.MapPath("~/Modules/" + moduleName + "/");
            var csProject = CreateCsProject(modulePath + moduleName + ".csproj",moduleName);

            string propertiesPath = modulePath + "Properties";
            var content = new HashSet<string>();
            var folders = new HashSet<string>();
            foreach (var folder in _moduleDirectories) {
                Directory.CreateDirectory(modulePath + folder);
                if (!String.IsNullOrEmpty(folder)) {
                    folders.Add(modulePath + folder);
                }
            }

            File.WriteAllText(modulePath + "Web.config", File.ReadAllText(_codeGenTemplatePath + "ModuleRootWebConfig.txt"));
            content.Add(modulePath + "Web.config");
            File.WriteAllText(modulePath + "Scripts\\Web.config", File.ReadAllText(_codeGenTemplatePath + "StaticFilesWebConfig.txt"));
            content.Add(modulePath + "Scripts\\Web.config");
            File.WriteAllText(modulePath + "Styles\\Web.config", File.ReadAllText(_codeGenTemplatePath + "StaticFilesWebConfig.txt"));
            content.Add(modulePath + "Styles\\Web.config");

            var assemblyInfoTemplate = new ModuleAssemblyInfo();
            assemblyInfoTemplate.Session = new Dictionary<string, object>();
            assemblyInfoTemplate.Session["ModuleName"] = moduleName;
            assemblyInfoTemplate.Session["ModuleTypeLibGuid"] = csProject.ProjectGuid;
            assemblyInfoTemplate.Initialize();
            string templateText = assemblyInfoTemplate.TransformText();
            File.WriteAllText(propertiesPath + "\\AssemblyInfo.cs", templateText);
            content.Add(propertiesPath + "\\AssemblyInfo.cs");

            var moduleMainfestTemplate = new ModuleManifest();
            moduleMainfestTemplate.Session = new Dictionary<string, object>();
            moduleMainfestTemplate.Session["ModuleName"] = moduleName;
            moduleMainfestTemplate.Initialize();
            templateText = moduleMainfestTemplate.TransformText();
            File.WriteAllText(modulePath + "Module.txt", templateText, System.Text.Encoding.UTF8);
            content.Add(modulePath + "Module.txt");

            CreateProjectItemGroup(csProject, modulePath, content, folders);
            csProject.Save();
            return csProject;
        }

        private static CsProjFile CreateCsProject(string fileName, string projectName) {
            var projectTemplate = new ModuleCsProj();
            var projectText = projectTemplate.TransformText();
            var project = MSBuildProject.Parse(projectName, projectText);
            var csProject = new CsProjFile(fileName, project);
            csProject.RootNamespace = projectName;
            csProject.AssemblyName = projectName;
            AddCoeveryReferences(csProject);
            SetProjectGuid(csProject);
            return csProject;
        }

        private static void AddCoeveryReferences(CsProjFile projectFile) {
            projectFile.Add(new AssemblyReference("Coevery.Core") {
                HintPath = @"..\..\bin\Coevery.Core.dll",
                SpecificVersion = false
            });
            projectFile.Add(new AssemblyReference("Coevery.Framework")
            {
                HintPath = @"..\..\bin\Coevery.Framework.dll",
                SpecificVersion = false
            });
        }

        private void CreateThemeFromTemplates(TextWriter output, string themeName, string baseTheme, bool createProject, bool includeInSolution) {
            var themePath = HostingEnvironment.MapPath("~/Themes/" + themeName + "/");
            var createdFiles = new HashSet<string>();
            var createdFolders = new HashSet<string>();

            // create directories
            foreach (var folderName in _themeDirectories) {
                var folder = themePath + folderName;
                Directory.CreateDirectory(folder);
                if (!String.IsNullOrEmpty(folderName)) {
                    createdFolders.Add(folder);
                }
            }

            File.WriteAllText(themePath + "Web.config", File.ReadAllText(_codeGenTemplatePath + "ModuleRootWebConfig.txt"));
            createdFiles.Add(themePath + "Web.config");
            File.WriteAllText(themePath + "Scripts\\Web.config", File.ReadAllText(_codeGenTemplatePath + "StaticFilesWebConfig.txt"));
            createdFiles.Add(themePath + "Scripts\\Web.config");
            File.WriteAllText(themePath + "Styles\\Web.config", File.ReadAllText(_codeGenTemplatePath + "StaticFilesWebConfig.txt"));
            createdFiles.Add(themePath + "Styles\\Web.config");
            File.WriteAllText(themePath + "Content\\Web.config", File.ReadAllText(_codeGenTemplatePath + "StaticFilesWebConfig.txt"));
            createdFiles.Add(themePath + "Content\\Web.config");

            var themeTemplate = new ThemeManifest() {Session = new Dictionary<string, object>()};
            themeTemplate.Session["ThemeName"] = themeName;
            themeTemplate.Session["BaseTheme"] = baseTheme;
            themeTemplate.Initialize();
            var templateText = themeTemplate.TransformText();

            File.WriteAllText(themePath + "Theme.txt", templateText);
            createdFiles.Add(themePath + "Theme.txt");

            File.WriteAllBytes(themePath + "Theme.png", File.ReadAllBytes(_codeGenTemplatePath + "Theme.png"));
            createdFiles.Add(themePath + "Theme.png");

            File.WriteAllText(themePath + "Placement.info", File.ReadAllText(_codeGenTemplatePath + "Placement.info"));
            createdFiles.Add(themePath + "Placement.info");

            // create new csproj for the theme
            if (createProject) {
                var csProject = CreateCsProject(themePath + "\\" + themeName + ".csproj", themeName);
                CreateProjectItemGroup(csProject, themePath, createdFiles, createdFolders);
                csProject.Save();
                if (includeInSolution) {
                    // create a project (already done) and add it to the solution
                    AddToSolution(output, csProject, SolutionDirectoryThemes);
                }
            }

            if (includeInSolution && !createProject) {
                // include in solution but dont create a project: just add the references to Coevery.Themes project
                if (!File.Exists(_CoeveryThemesProj)) {
                    output.WriteLine(T("Warning: Coevery.Themes project file could not be found at {0}", _CoeveryThemesProj));
                }
                else {
                    var project = CsProjFile.LoadFrom(_CoeveryThemesProj);
                    CreateProjectItemGroup(project, HostingEnvironment.MapPath("~/Themes/"), createdFiles, createdFolders);
                    project.Save();
                }
                TouchSolution(output);
            }
        }


        private void AddToSolution(TextWriter output, CsProjFile csProject, string solutionFolderGuid) {
            var parentDirectory = Directory.GetParent(_WebRootProj).Parent;
            if (parentDirectory == null) {
                return;
            }
            var solutions = parentDirectory.GetFiles("*.sln");
            if (solutions.Length == 0) return;
            var solutionPath = solutions.First().FullName;
            if (File.Exists(solutionPath)) {
                var solution = Solution.LoadFrom(solutionPath);
                var solutionProject = new SolutionProject(csProject, solution.ParentDirectory);
                var projectReference = string.Format("EndProject\r\nProject(\"{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}\") = \"{0}\", \"{2}\", \"{{{1}}}\"\r\n", solutionProject.ProjectName, solutionProject.ProjectGuid, solutionProject.RelativePath);
                var projectConfiguationPlatforms = string.Format("GlobalSection(ProjectConfigurationPlatforms) = postSolution\r\n\t\t{{{0}}}.Debug|Any CPU.ActiveCfg = Debug|Any CPU\r\n\t\t{{{0}}}.Debug|Any CPU.Build.0 = Debug|Any CPU\r\n\t\t{{{0}}}.Release|Any CPU.ActiveCfg = Release|Any CPU\r\n\t\t{{{0}}}.Release|Any CPU.Build.0 = Release|Any CPU\r\n", solutionProject.ProjectGuid);
                var solutionText = File.ReadAllText(solutionPath);
                solutionText = solutionText.Insert(solutionText.LastIndexOf("EndProject\r\n"), projectReference).Replace("GlobalSection(ProjectConfigurationPlatforms) = postSolution\r\n", projectConfiguationPlatforms);
                solutionText = solutionText.Insert(solutionText.LastIndexOf("EndGlobalSection"), "\t{" + solutionProject.ProjectGuid + "} = {" + solutionFolderGuid + "}\r\n\t");
                File.WriteAllText(solutionPath, solutionText);
                TouchSolution(output);
            }
        }

        private static void CreateProjectItemGroup(CsProjFile project, string relativeFromPath, HashSet<string> contents, HashSet<string> folders)
        {
            if (relativeFromPath != null && !relativeFromPath.EndsWith("\\", StringComparison.OrdinalIgnoreCase)) {
                relativeFromPath += "\\";
            }
            else if (relativeFromPath == null) {
                relativeFromPath = "";
            }

            if (contents != null && contents.Count > 0) {
                foreach (var file in contents) {
                    project.Add<Content>(file.Replace(relativeFromPath, ""));
                }
            }
            if (folders != null && folders.Count > 0) {
                foreach (var folder in folders) {
                    project.BuildProject.AddNewItem("Folder", folder.Replace(relativeFromPath, ""));
                }
            }
        }

        private void TouchSolution(TextWriter output) {
            var parentDirectory = Directory.GetParent(_WebRootProj).Parent;
            if (parentDirectory == null) {
                return;
            }
            var solutions = parentDirectory.GetFiles("*.sln");
            if (!solutions.Any()) {
                output.WriteLine(T("Warning: Solution file could not be found at {0}", parentDirectory.FullName));
                return;
            }

            string solutionPath = solutions.First().FullName;
            try {
                File.SetLastWriteTime(solutionPath, DateTime.Now);
            }
            catch {
                output.WriteLine(T("An unexpected error occured while trying to refresh the Visual Studio solution. Please reload it."));
            }
        }

        private static void SetProjectGuid(CsProjFile project) {
            var group = project.BuildProject.PropertyGroups.FirstOrDefault(x => x.Properties.Any(p => p.Name == "ProjectGuid"))
                        ?? project.BuildProject.PropertyGroups.FirstOrDefault() ?? project.BuildProject.AddNewPropertyGroup(true);

            group.SetPropertyValue("ProjectGuid", Guid.NewGuid().ToString().ToUpper(), false);
        }
    }
}