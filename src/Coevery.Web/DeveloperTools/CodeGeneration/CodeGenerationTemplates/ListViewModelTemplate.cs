﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 11.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Coevery.DeveloperTools.CodeGeneration.CodeGenerationTemplates
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using Coevery.Core.Projections.Descriptors.Property;
    using Coevery.Core.Projections.Models;
    using Coevery.ContentManagement;
    using Coevery.Core.Projections.Services;
    using Coevery.DeveloperTools.CodeGeneration.Services;
    using Coevery.DeveloperTools.CodeGeneration.Utils;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ListViewModelTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "11.0.0.0")]
    public partial class ListViewModelTemplate : ListViewModelTemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("\r\n");
            this.Write("namespace ");
            
            #line 8 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ListViewModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Namespace));
            
            #line default
            #line hidden
            this.Write(".ViewModels {\r\n    public sealed class ");
            
            #line 9 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ListViewModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ModelDefinition.Name));
            
            #line default
            #line hidden
            this.Write("ListViewModel {\r\n\t\tpublic int Id { get; set; }\r\n");
            
            #line 11 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ListViewModelTemplate.tt"
foreach (var field in GetFields()) { 
            
            #line default
            #line hidden
            this.Write("\t\tpublic ");
            
            #line 12 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ListViewModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(field.Type.GetFriendlyName()));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 12 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ListViewModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(field.Name));
            
            #line default
            #line hidden
            this.Write("{ get; set; }\r\n");
            
            #line 13 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ListViewModelTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write("    }\r\n}\r\n");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 16 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ListViewModelTemplate.tt"


    private IEnumerable<DynamicFieldDefinition> GetFields() {
        var columns = GetColumns(ModelDefinition.Name);
        foreach (var col in columns) {
            var field = ModelDefinition.Fields.FirstOrDefault(f => f.Name == col.Name);
            if (field != null) {
                yield return field;
            }
        }
    }


        
        #line default
        #line hidden
        
        #line 14 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ListViewCommon.ttinclude"

    private IEnumerable<GridColumn> GetColumns(string entityTypeName) {
        var listViewPart = ContentManager.Query<ListViewPart, ListViewPartRecord>("ListViewPage")
            .Where(v => v.IsDefault && v.ItemContentType == entityTypeName).List().FirstOrDefault();

        var keyColumn = new GridColumn();
        keyColumn.Name = "Id";
        keyColumn.Label = "Id";
        keyColumn.Hidden = true;
        keyColumn.IsKey = true;

        IEnumerable<FieldDescriptor> properties = Enumerable.Empty<FieldDescriptor>();
        LayoutRecord layoutRecord = null;

        #region Load Properties

        var projectionPart = listViewPart.As<ProjectionPart>();
        if (projectionPart != null) {
            var queryPartRecord = projectionPart.Record.QueryPartRecord;
            if (queryPartRecord.Layouts.Any())
                layoutRecord = queryPartRecord.Layouts[0];
        }

        if (layoutRecord != null) {
            var allFielDescriptors = ProjectionManager.DescribeProperties().ToList();
            properties = layoutRecord.Properties.OrderBy(p => p.Position)
                .Select(p => allFielDescriptors.SelectMany(x => x.Descriptors)
                    .Select(d => new FieldDescriptor {Descriptor = d, Property = p}).FirstOrDefault(x => x.Descriptor.Category == p.Category && x.Descriptor.Type == p.Type)).ToList();
        }

        #endregion


        var columns = new List<GridColumn> {keyColumn};

        foreach (var property in properties.Select(x => x.Property)) {
            var column = new GridColumn();
            var filedName = property.GetFieldName();
            column.Name = filedName;
            column.Label = T(property.Description).Text;
            if (property.LinkToContent) {}
            //column["sortable"] = false;
            columns.Add(column);
        }

        return columns;
    }

    class GridColumn {
        public string Name { get; set; }
		public string Label { get; set; }
		public bool Hidden { get; set; }
		public bool IsKey { get; set; }
    }

	class FieldDescriptor
    {
        public PropertyDescriptor Descriptor { get; set; }
        public PropertyRecord Property { get; set; }
    }


        
        #line default
        #line hidden
        
        #line 1 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ListViewModelTemplate.tt"

private global::Coevery.ContentManagement.IContentManager _ContentManagerField;

/// <summary>
/// Access the ContentManager parameter of the template.
/// </summary>
private global::Coevery.ContentManagement.IContentManager ContentManager
{
    get
    {
        return this._ContentManagerField;
    }
}

private global::Coevery.Core.Projections.Services.IProjectionManager _ProjectionManagerField;

/// <summary>
/// Access the ProjectionManager parameter of the template.
/// </summary>
private global::Coevery.Core.Projections.Services.IProjectionManager ProjectionManager
{
    get
    {
        return this._ProjectionManagerField;
    }
}

private global::Coevery.Localization.Localizer _TField;

/// <summary>
/// Access the T parameter of the template.
/// </summary>
private global::Coevery.Localization.Localizer T
{
    get
    {
        return this._TField;
    }
}

private string _NamespaceField;

/// <summary>
/// Access the Namespace parameter of the template.
/// </summary>
private string Namespace
{
    get
    {
        return this._NamespaceField;
    }
}

private global::Coevery.DeveloperTools.CodeGeneration.Services.DynamicDefinition _ModelDefinitionField;

/// <summary>
/// Access the ModelDefinition parameter of the template.
/// </summary>
private global::Coevery.DeveloperTools.CodeGeneration.Services.DynamicDefinition ModelDefinition
{
    get
    {
        return this._ModelDefinitionField;
    }
}


/// <summary>
/// Initialize the template
/// </summary>
public virtual void Initialize()
{
    if ((this.Errors.HasErrors == false))
    {
bool ContentManagerValueAcquired = false;
if (this.Session.ContainsKey("ContentManager"))
{
    if ((typeof(global::Coevery.ContentManagement.IContentManager).IsAssignableFrom(this.Session["ContentManager"].GetType()) == false))
    {
        this.Error("The type \'Coevery.ContentManagement.IContentManager\' of the parameter \'ContentMan" +
                "ager\' did not match the type of the data passed to the template.");
    }
    else
    {
        this._ContentManagerField = ((global::Coevery.ContentManagement.IContentManager)(this.Session["ContentManager"]));
        ContentManagerValueAcquired = true;
    }
}
if ((ContentManagerValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("ContentManager");
    if ((data != null))
    {
        if ((typeof(global::Coevery.ContentManagement.IContentManager).IsAssignableFrom(data.GetType()) == false))
        {
            this.Error("The type \'Coevery.ContentManagement.IContentManager\' of the parameter \'ContentMan" +
                    "ager\' did not match the type of the data passed to the template.");
        }
        else
        {
            this._ContentManagerField = ((global::Coevery.ContentManagement.IContentManager)(data));
        }
    }
}
bool ProjectionManagerValueAcquired = false;
if (this.Session.ContainsKey("ProjectionManager"))
{
    if ((typeof(global::Coevery.Core.Projections.Services.IProjectionManager).IsAssignableFrom(this.Session["ProjectionManager"].GetType()) == false))
    {
        this.Error("The type \'Coevery.Core.Projections.Services.IProjectionManager\' of the parameter " +
                "\'ProjectionManager\' did not match the type of the data passed to the template.");
    }
    else
    {
        this._ProjectionManagerField = ((global::Coevery.Core.Projections.Services.IProjectionManager)(this.Session["ProjectionManager"]));
        ProjectionManagerValueAcquired = true;
    }
}
if ((ProjectionManagerValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("ProjectionManager");
    if ((data != null))
    {
        if ((typeof(global::Coevery.Core.Projections.Services.IProjectionManager).IsAssignableFrom(data.GetType()) == false))
        {
            this.Error("The type \'Coevery.Core.Projections.Services.IProjectionManager\' of the parameter " +
                    "\'ProjectionManager\' did not match the type of the data passed to the template.");
        }
        else
        {
            this._ProjectionManagerField = ((global::Coevery.Core.Projections.Services.IProjectionManager)(data));
        }
    }
}
bool TValueAcquired = false;
if (this.Session.ContainsKey("T"))
{
    if ((typeof(global::Coevery.Localization.Localizer).IsAssignableFrom(this.Session["T"].GetType()) == false))
    {
        this.Error("The type \'Coevery.Localization.Localizer\' of the parameter \'T\' did not match the " +
                "type of the data passed to the template.");
    }
    else
    {
        this._TField = ((global::Coevery.Localization.Localizer)(this.Session["T"]));
        TValueAcquired = true;
    }
}
if ((TValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("T");
    if ((data != null))
    {
        if ((typeof(global::Coevery.Localization.Localizer).IsAssignableFrom(data.GetType()) == false))
        {
            this.Error("The type \'Coevery.Localization.Localizer\' of the parameter \'T\' did not match the " +
                    "type of the data passed to the template.");
        }
        else
        {
            this._TField = ((global::Coevery.Localization.Localizer)(data));
        }
    }
}
bool NamespaceValueAcquired = false;
if (this.Session.ContainsKey("Namespace"))
{
    if ((typeof(string).IsAssignableFrom(this.Session["Namespace"].GetType()) == false))
    {
        this.Error("The type \'System.String\' of the parameter \'Namespace\' did not match the type of t" +
                "he data passed to the template.");
    }
    else
    {
        this._NamespaceField = ((string)(this.Session["Namespace"]));
        NamespaceValueAcquired = true;
    }
}
if ((NamespaceValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("Namespace");
    if ((data != null))
    {
        if ((typeof(string).IsAssignableFrom(data.GetType()) == false))
        {
            this.Error("The type \'System.String\' of the parameter \'Namespace\' did not match the type of t" +
                    "he data passed to the template.");
        }
        else
        {
            this._NamespaceField = ((string)(data));
        }
    }
}
bool ModelDefinitionValueAcquired = false;
if (this.Session.ContainsKey("ModelDefinition"))
{
    if ((typeof(global::Coevery.DeveloperTools.CodeGeneration.Services.DynamicDefinition).IsAssignableFrom(this.Session["ModelDefinition"].GetType()) == false))
    {
        this.Error("The type \'Coevery.DeveloperTools.CodeGeneration.Services.DynamicDefinition\' of th" +
                "e parameter \'ModelDefinition\' did not match the type of the data passed to the t" +
                "emplate.");
    }
    else
    {
        this._ModelDefinitionField = ((global::Coevery.DeveloperTools.CodeGeneration.Services.DynamicDefinition)(this.Session["ModelDefinition"]));
        ModelDefinitionValueAcquired = true;
    }
}
if ((ModelDefinitionValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("ModelDefinition");
    if ((data != null))
    {
        if ((typeof(global::Coevery.DeveloperTools.CodeGeneration.Services.DynamicDefinition).IsAssignableFrom(data.GetType()) == false))
        {
            this.Error("The type \'Coevery.DeveloperTools.CodeGeneration.Services.DynamicDefinition\' of th" +
                    "e parameter \'ModelDefinition\' did not match the type of the data passed to the t" +
                    "emplate.");
        }
        else
        {
            this._ModelDefinitionField = ((global::Coevery.DeveloperTools.CodeGeneration.Services.DynamicDefinition)(data));
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
    public class ListViewModelTemplateBase
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
