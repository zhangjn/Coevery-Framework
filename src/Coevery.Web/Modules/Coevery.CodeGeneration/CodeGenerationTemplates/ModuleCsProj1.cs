﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 11.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Coevery.CodeGeneration.CodeGenerationTemplates
{
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\Modules\Coevery.CodeGeneration\CodeGenerationTemplates\ModuleCsProj.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "11.0.0.0")]
    public partial class ModuleCsProj : ModuleCsProjBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write(@"<?xml version=""1.0"" encoding=""utf-8""?>
<Project ToolsVersion=""4.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <Import Project=""$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props"" Condition=""Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')"" />
  <PropertyGroup>
    <Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>
    <Platform Condition="" '$(Platform)' == '' "">AnyCPU</Platform>
    <ProductVersion>9.0.30729</ProductVersion>
    <SchemaVersion>2.0</SchemaVersion>
    <ProjectGuid>{");
            
            #line 15 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\Modules\Coevery.CodeGeneration\CodeGenerationTemplates\ModuleCsProj.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ModuleProjectGuid));
            
            #line default
            #line hidden
            this.Write("}</ProjectGuid>\r\n    <ProjectTypeGuids>{349c5851-65df-11da-9384-00065b846f21};{fa" +
                    "e04ec0-301f-11d3-bf4b-00c04f79efbc}</ProjectTypeGuids>\r\n    <OutputType>Library<" +
                    "/OutputType>\r\n    <AppDesignerFolder>Properties</AppDesignerFolder>\r\n    <RootNa" +
                    "mespace>");
            
            #line 19 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\Modules\Coevery.CodeGeneration\CodeGenerationTemplates\ModuleCsProj.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ModuleName));
            
            #line default
            #line hidden
            this.Write("</RootNamespace>\r\n    <AssemblyName>");
            
            #line 20 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\Modules\Coevery.CodeGeneration\CodeGenerationTemplates\ModuleCsProj.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ModuleName));
            
            #line default
            #line hidden
            this.Write("</AssemblyName>\r\n    <TargetFrameworkVersion>v4.5</TargetFrameworkVersion>\r\n    <" +
                    "MvcBuildViews>false</MvcBuildViews>\r\n    <FileUpgradeFlags>\r\n    </FileUpgradeFl" +
                    "ags>\r\n    <OldToolsVersion>4.0</OldToolsVersion>\r\n    <UpgradeBackupLocation />\r" +
                    "\n    <TargetFrameworkProfile />\r\n  </PropertyGroup>\r\n  <PropertyGroup Condition=" +
                    "\" \'$(Configuration)|$(Platform)\' == \'Debug|AnyCPU\' \">\r\n    <DebugSymbols>true</D" +
                    "ebugSymbols>\r\n    <DebugType>full</DebugType>\r\n    <Optimize>false</Optimize>\r\n " +
                    "   <OutputPath>bin\\</OutputPath>\r\n    <DefineConstants>DEBUG;TRACE</DefineConsta" +
                    "nts>\r\n    <ErrorReport>prompt</ErrorReport>\r\n    <WarningLevel>4</WarningLevel>\r" +
                    "\n    <CodeAnalysisRuleSet>AllRules.ruleset</CodeAnalysisRuleSet>\r\n    <Prefer32B" +
                    "it>false</Prefer32Bit>\r\n  </PropertyGroup>\r\n  <PropertyGroup Condition=\" \'$(Conf" +
                    "iguration)|$(Platform)\' == \'Release|AnyCPU\' \">\r\n    <DebugType>pdbonly</DebugTyp" +
                    "e>\r\n    <Optimize>true</Optimize>\r\n    <OutputPath>bin\\</OutputPath>\r\n    <Defin" +
                    "eConstants>TRACE</DefineConstants>\r\n    <ErrorReport>prompt</ErrorReport>\r\n    <" +
                    "WarningLevel>4</WarningLevel>\r\n    <CodeAnalysisRuleSet>AllRules.ruleset</CodeAn" +
                    "alysisRuleSet>\r\n    <Prefer32Bit>false</Prefer32Bit>\r\n  </PropertyGroup>\r\n  <Ite" +
                    "mGroup>\r\n    <Reference Include=\"Microsoft.CSharp\" />\r\n    <Reference Include=\"S" +
                    "ystem\" />\r\n    <Reference Include=\"System.Data\" />\r\n    <Reference Include=\"Syst" +
                    "em.ComponentModel.DataAnnotations\">\r\n      <RequiredTargetFramework>3.5</Require" +
                    "dTargetFramework>\r\n    </Reference>\r\n    <Reference Include=\"System.Web.DynamicD" +
                    "ata\" />\r\n    <Reference Include=\"System.Web.Mvc, Version=5.1.0.0, Culture=neutra" +
                    "l, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL\">\r\n      <Specifi" +
                    "cVersion>False</SpecificVersion>\r\n      <HintPath>..\\..\\..\\..\\lib\\aspnetmvc\\Syst" +
                    "em.Web.Mvc.dll</HintPath>\r\n    </Reference>\r\n    <Reference Include=\"System.Web\"" +
                    " />\r\n    <Reference Include=\"System.Web.Extensions\" />\r\n    <Reference Include=\"" +
                    "System.Web.Abstractions\" />\r\n    <Reference Include=\"System.Web.Routing\" />\r\n   " +
                    " <Reference Include=\"System.Xml\" />\r\n    <Reference Include=\"System.Configuratio" +
                    "n\" />\r\n    <Reference Include=\"System.Xml.Linq\" />\r\n  </ItemGroup>\r\n  ");
            
            #line 70 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\Modules\Coevery.CodeGeneration\CodeGenerationTemplates\ModuleCsProj.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(FileIncludes));
            
            #line default
            #line hidden
            this.Write("\r\n  <ItemGroup>\r\n    ");
            
            #line 72 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\Modules\Coevery.CodeGeneration\CodeGenerationTemplates\ModuleCsProj.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(CoeveryReferences));
            
            #line default
            #line hidden
            this.Write("\r\n    </ItemGroup>\r\n  <PropertyGroup>\r\n    <VisualStudioVersion Condition=\"\'$(Vis" +
                    "ualStudioVersion)\' == \'\'\">10.0</VisualStudioVersion>\r\n    <VSToolsPath Condition" +
                    "=\"\'$(VSToolsPath)\' == \'\'\">$(MSBuildExtensionsPath32)\\Microsoft\\VisualStudio\\v$(V" +
                    "isualStudioVersion)</VSToolsPath>\r\n  </PropertyGroup>\r\n  <Import Project=\"$(MSBu" +
                    "ildBinPath)\\Microsoft.CSharp.targets\" />\r\n  <Import Project=\"$(VSToolsPath)\\WebA" +
                    "pplications\\Microsoft.WebApplication.targets\" Condition=\"\'$(VSToolsPath)\' != \'\'\"" +
                    " />\r\n  <Import Project=\"$(MSBuildExtensionsPath32)\\Microsoft\\VisualStudio\\v10.0\\" +
                    "WebApplications\\Microsoft.WebApplication.targets\" Condition=\"false\" />\r\n  <!-- T" +
                    "o modify your build process, add your task inside one of the targets below and u" +
                    "ncomment it. \r\n       Other similar extension points exist, see Microsoft.Common" +
                    ".targets.\r\n  <Target Name=\"BeforeBuild\">\r\n  </Target> -->\r\n  <Target Name=\"After" +
                    "Build\" DependsOnTargets=\"AfterBuildCompiler\">\r\n    <PropertyGroup>\r\n      <Areas" +
                    "ManifestDir>$(ProjectDir)\\..\\Manifests</AreasManifestDir>\r\n    </PropertyGroup>\r" +
                    "\n    <!-- If this is an area child project, uncomment the following line:\r\n    <" +
                    "CreateAreaManifest AreaName=\"$(AssemblyName)\" AreaType=\"Child\" AreaPath=\"$(Proje" +
                    "ctDir)\" ManifestPath=\"$(AreasManifestDir)\" ContentFiles=\"@(Content)\" />\r\n    -->" +
                    "\r\n    <!-- If this is an area parent project, uncomment the following lines:\r\n  " +
                    "  <CreateAreaManifest AreaName=\"$(AssemblyName)\" AreaType=\"Parent\" AreaPath=\"$(P" +
                    "rojectDir)\" ManifestPath=\"$(AreasManifestDir)\" ContentFiles=\"@(Content)\" />\r\n   " +
                    " <CopyAreaManifests ManifestPath=\"$(AreasManifestDir)\" CrossCopy=\"false\" RenameV" +
                    "iews=\"true\" />\r\n    -->\r\n  </Target>\r\n  <Target Name=\"AfterBuildCompiler\" Condit" +
                    "ion=\"\'$(MvcBuildViews)\'==\'true\'\">\r\n    <AspNetCompiler VirtualPath=\"temp\" Physic" +
                    "alPath=\"$(ProjectDir)\\..\\$(ProjectName)\" />\r\n  </Target>\r\n  <ProjectExtensions>\r" +
                    "\n    <VisualStudio>\r\n      <FlavorProperties GUID=\"{349c5851-65df-11da-9384-0006" +
                    "5b846f21}\">\r\n        <WebProjectProperties>\r\n          <UseIIS>False</UseIIS>\r\n " +
                    "         <AutoAssignPort>True</AutoAssignPort>\r\n          <DevelopmentServerPort" +
                    ">45979</DevelopmentServerPort>\r\n          <DevelopmentServerVPath>/</Development" +
                    "ServerVPath>\r\n          <IISUrl>\r\n          </IISUrl>\r\n          <NTLMAuthentica" +
                    "tion>False</NTLMAuthentication>\r\n          <UseCustomServer>True</UseCustomServe" +
                    "r>\r\n          <CustomServerUrl>http://coevery.codeplex.com</CustomServerUrl>\r\n  " +
                    "        <SaveServerSettingsInUserFile>False</SaveServerSettingsInUserFile>\r\n    " +
                    "    </WebProjectProperties>\r\n      </FlavorProperties>\r\n    </VisualStudio>\r\n  <" +
                    "/ProjectExtensions>\r\n</Project>");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 1 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\Modules\Coevery.CodeGeneration\CodeGenerationTemplates\ModuleCsProj.tt"

private global::System.Guid _ModuleProjectGuidField;

/// <summary>
/// Access the ModuleProjectGuid parameter of the template.
/// </summary>
private global::System.Guid ModuleProjectGuid
{
    get
    {
        return this._ModuleProjectGuidField;
    }
}

private string _ModuleNameField;

/// <summary>
/// Access the ModuleName parameter of the template.
/// </summary>
private string ModuleName
{
    get
    {
        return this._ModuleNameField;
    }
}

private string _FileIncludesField;

/// <summary>
/// Access the FileIncludes parameter of the template.
/// </summary>
private string FileIncludes
{
    get
    {
        return this._FileIncludesField;
    }
}

private string _CoeveryReferencesField;

/// <summary>
/// Access the CoeveryReferences parameter of the template.
/// </summary>
private string CoeveryReferences
{
    get
    {
        return this._CoeveryReferencesField;
    }
}


/// <summary>
/// Initialize the template
/// </summary>
public virtual void Initialize()
{
    if ((this.Errors.HasErrors == false))
    {
bool ModuleProjectGuidValueAcquired = false;
if (this.Session.ContainsKey("ModuleProjectGuid"))
{
    if ((typeof(global::System.Guid).IsAssignableFrom(this.Session["ModuleProjectGuid"].GetType()) == false))
    {
        this.Error("The type \'System.Guid\' of the parameter \'ModuleProjectGuid\' did not match the typ" +
                "e of the data passed to the template.");
    }
    else
    {
        this._ModuleProjectGuidField = ((global::System.Guid)(this.Session["ModuleProjectGuid"]));
        ModuleProjectGuidValueAcquired = true;
    }
}
if ((ModuleProjectGuidValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("ModuleProjectGuid");
    if ((data != null))
    {
        if ((typeof(global::System.Guid).IsAssignableFrom(data.GetType()) == false))
        {
            this.Error("The type \'System.Guid\' of the parameter \'ModuleProjectGuid\' did not match the typ" +
                    "e of the data passed to the template.");
        }
        else
        {
            this._ModuleProjectGuidField = ((global::System.Guid)(data));
        }
    }
}
bool ModuleNameValueAcquired = false;
if (this.Session.ContainsKey("ModuleName"))
{
    if ((typeof(string).IsAssignableFrom(this.Session["ModuleName"].GetType()) == false))
    {
        this.Error("The type \'System.String\' of the parameter \'ModuleName\' did not match the type of " +
                "the data passed to the template.");
    }
    else
    {
        this._ModuleNameField = ((string)(this.Session["ModuleName"]));
        ModuleNameValueAcquired = true;
    }
}
if ((ModuleNameValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("ModuleName");
    if ((data != null))
    {
        if ((typeof(string).IsAssignableFrom(data.GetType()) == false))
        {
            this.Error("The type \'System.String\' of the parameter \'ModuleName\' did not match the type of " +
                    "the data passed to the template.");
        }
        else
        {
            this._ModuleNameField = ((string)(data));
        }
    }
}
bool FileIncludesValueAcquired = false;
if (this.Session.ContainsKey("FileIncludes"))
{
    if ((typeof(string).IsAssignableFrom(this.Session["FileIncludes"].GetType()) == false))
    {
        this.Error("The type \'System.String\' of the parameter \'FileIncludes\' did not match the type o" +
                "f the data passed to the template.");
    }
    else
    {
        this._FileIncludesField = ((string)(this.Session["FileIncludes"]));
        FileIncludesValueAcquired = true;
    }
}
if ((FileIncludesValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("FileIncludes");
    if ((data != null))
    {
        if ((typeof(string).IsAssignableFrom(data.GetType()) == false))
        {
            this.Error("The type \'System.String\' of the parameter \'FileIncludes\' did not match the type o" +
                    "f the data passed to the template.");
        }
        else
        {
            this._FileIncludesField = ((string)(data));
        }
    }
}
bool CoeveryReferencesValueAcquired = false;
if (this.Session.ContainsKey("CoeveryReferences"))
{
    if ((typeof(string).IsAssignableFrom(this.Session["CoeveryReferences"].GetType()) == false))
    {
        this.Error("The type \'System.String\' of the parameter \'CoeveryReferences\' did not match the t" +
                "ype of the data passed to the template.");
    }
    else
    {
        this._CoeveryReferencesField = ((string)(this.Session["CoeveryReferences"]));
        CoeveryReferencesValueAcquired = true;
    }
}
if ((CoeveryReferencesValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("CoeveryReferences");
    if ((data != null))
    {
        if ((typeof(string).IsAssignableFrom(data.GetType()) == false))
        {
            this.Error("The type \'System.String\' of the parameter \'CoeveryReferences\' did not match the t" +
                    "ype of the data passed to the template.");
        }
        else
        {
            this._CoeveryReferencesField = ((string)(data));
        }
    }
}


    }
}


        
        #line default
        #line hidden
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "11.0.0.0")]
    public class ModuleCsProjBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
