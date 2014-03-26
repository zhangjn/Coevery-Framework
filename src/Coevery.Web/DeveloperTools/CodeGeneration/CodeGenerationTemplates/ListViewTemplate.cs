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
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ListViewTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "11.0.0.0")]
    public partial class ListViewTemplate : ListViewTemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("\r\n");
            this.Write(@"@{
    Style.Require(""jqGrid"");
    Style.Require(""jqGridCustom"");
    Script.Require(""jqGrid"");
	Script.Require(""jqGrid_i18n"");
}

<div class=""row-fluid"">
    <button id=""btnAdd"" class=""btn btn-primary"" type=""button""><i class=""icon-plus""></i> Add</button>
    <button id=""btnEdit"" class=""btn btn-primary"" type=""button""><i class=""icon-edit""></i> Edit</button>
    <button id=""btnDelete"" class=""btn btn-warning"" type=""button""><i class=""icon-trash""></i> Delete</button>
    <button id=""btnRefresh"" class=""btn"" type=""button"">Refresh</button>
</div>

<div class=""row-fluid"">
    <table id=""");
            
            #line 25 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ListViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ViewName.ToLower()));
            
            #line default
            #line hidden
            this.Write("Grid\"></table>\r\n    <section id=\"");
            
            #line 26 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ListViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ViewName.ToLower()));
            
            #line default
            #line hidden
            this.Write("GridPager\"></section>\r\n</div>\r\n@Html.AntiForgeryToken()\r\n\r\n@using (Script.Foot())" +
                    " {\r\n    <script type=\"text/javascript\">\r\n\t\tfunction updateButtonStatus() {\r\n    " +
                    "        var selectedRowIds = $(\'#");
            
            #line 33 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ListViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ViewName.ToLower()));
            
            #line default
            #line hidden
            this.Write(@"Grid').jqGrid('getGridParam', 'selarrrow');
            $(""#btnEdit"").toggle(selectedRowIds.length == 1);
            $(""#btnDelete"").toggle(selectedRowIds.length > 0);
        }

        $.extend(jQuery.jgrid.defaults, {
            prmNames: {
                page: 'page',
                rows: 'pageSize',
                sort: 'sortBy',
                order: 'sortOrder'
            }
        });
        $('#");
            
            #line 46 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ListViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ViewName.ToLower()));
            
            #line default
            #line hidden
            this.Write("Grid\').jqGrid({\r\n            url: \'@Href(\"~/");
            
            #line 47 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ListViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Namespace));
            
            #line default
            #line hidden
            this.Write("/");
            
            #line 47 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ListViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(EntityTypeName));
            
            #line default
            #line hidden
            this.Write(@"/List"")',
            datatype: 'json',
			mtype: 'POST',
            postData: {
                __RequestVerificationToken: function () {
                var magicToken = $(""input[name=__RequestVerificationToken]"").first();
                if (!magicToken) { return null; } // no sense in continuing if form POSTS will fail
                return magicToken.val();
                }
            },
            colModel: ");
            
            #line 57 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ListViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetGridColumnJSONString()));
            
            #line default
            #line hidden
            this.Write(",\r\n            rowNum: 10,\r\n            rowList: [10, 20, 30],\r\n            pager" +
                    ": \'#");
            
            #line 60 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ListViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ViewName.ToLower()));
            
            #line default
            #line hidden
            this.Write(@"GridPager',
            viewrecords: true,
            height: '100%',
            pagerpos: 'right',
            recordpos: 'left',
            sortable: true,
            headertitles: true,
            multiselect: true,
            multiboxonly: true,
            autowidth: true,
            jsonReader: {
                page: 'page',
                total: 'totalPages',
                records: 'totalRecords',
                repeatitems: false
            },
            onSelectRow: function() {
                updateButtonStatus();
            },
            gridComplete: function() {
                updateButtonStatus();
            }
        });
        $('#btnAdd').click(function() {
            window.location.href = '@Href(""~/");
            
            #line 84 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ListViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Namespace));
            
            #line default
            #line hidden
            this.Write("/");
            
            #line 84 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ListViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(EntityTypeName));
            
            #line default
            #line hidden
            this.Write("/Create\")\';\r\n        });\r\n        $(\'#btnEdit\').click(function() {\r\n            v" +
                    "ar selectedRowIds = $(\'#");
            
            #line 87 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ListViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ViewName.ToLower()));
            
            #line default
            #line hidden
            this.Write("Grid\').jqGrid(\'getGridParam\', \'selarrrow\');\r\n            if (selectedRowIds.lengt" +
                    "h == 0) return;\r\n            window.location.href = \'@Href(\"~/");
            
            #line 89 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ListViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Namespace));
            
            #line default
            #line hidden
            this.Write("/");
            
            #line 89 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ListViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(EntityTypeName));
            
            #line default
            #line hidden
            this.Write("/Edit/\")\' + selectedRowIds[0];\r\n        });\r\n        $(\'#btnDelete\').click(functi" +
                    "on () {\r\n            var selectedIds = $(\'#");
            
            #line 92 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ListViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ViewName.ToLower()));
            
            #line default
            #line hidden
            this.Write(@"Grid').jqGrid('getGridParam', 'selarrrow');
		    if (selectedIds.length == 0) return;
		    var magicToken = $(""input[name=__RequestVerificationToken]"").first();
		    if (!magicToken) {
		        return;
		    } // no sense in continuing if form POSTS will fail
		    var confirm = window.confirm($.jgrid.del.msg);
		    if (!confirm) return;

			$.ajax({
		            url: '@Href(""~/");
            
            #line 102 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ListViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Namespace));
            
            #line default
            #line hidden
            this.Write("/");
            
            #line 102 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ListViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(EntityTypeName));
            
            #line default
            #line hidden
            this.Write("/Delete\")\',\r\n\t\t            data: { selectedIds: selectedIds, __RequestVerificatio" +
                    "nToken: magicToken.val() },\r\n\t\t            type: \"POST\",\r\n\t\t            traditio" +
                    "nal: true\r\n\t\t        })\r\n\t\t        .done(function(response) {\r\n\t\t            $(\'" +
                    "#");
            
            #line 108 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ListViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ViewName.ToLower()));
            
            #line default
            #line hidden
            this.Write("Grid\').trigger(\'reloadGrid\');\r\n\t\t        })\r\n\t\t        .fail(function(jqXHR, text" +
                    "Status, errorThrown) {\r\n\t\t            alert(textStatus);\r\n\t\t        });\r\n       " +
                    " });\r\n        $(\'#btnRefresh\').click(function () {\r\n            $(\'#");
            
            #line 115 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ListViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ViewName.ToLower()));
            
            #line default
            #line hidden
            this.Write("Grid\').trigger(\'reloadGrid\');\r\n        });\r\n    </script>\r\n}\r\n\r\n\r\n");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 121 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ListViewTemplate.tt"


    private string GetGridColumnJSONString() {
        var jsonColumns = new JArray();
        var columns = GetColumns(EntityTypeName);
        foreach (var gridColumn in columns) {
            var column = new JObject();
            column["name"] = gridColumn.Name;
			column["index"] = gridColumn.Name;
            column["label"] = gridColumn.Label;
            if (gridColumn.Hidden)
                column["hidden"] = true;
            if (gridColumn.IsKey)
                column["key"] = true;
            jsonColumns.Add(column);
        }
        return JsonConvert.SerializeObject(jsonColumns);
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
        
        #line 1 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ListViewTemplate.tt"

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

private string _EntityTypeNameField;

/// <summary>
/// Access the EntityTypeName parameter of the template.
/// </summary>
private string EntityTypeName
{
    get
    {
        return this._EntityTypeNameField;
    }
}

private string _ViewNameField;

/// <summary>
/// Access the ViewName parameter of the template.
/// </summary>
private string ViewName
{
    get
    {
        return this._ViewNameField;
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
bool EntityTypeNameValueAcquired = false;
if (this.Session.ContainsKey("EntityTypeName"))
{
    if ((typeof(string).IsAssignableFrom(this.Session["EntityTypeName"].GetType()) == false))
    {
        this.Error("The type \'System.String\' of the parameter \'EntityTypeName\' did not match the type" +
                " of the data passed to the template.");
    }
    else
    {
        this._EntityTypeNameField = ((string)(this.Session["EntityTypeName"]));
        EntityTypeNameValueAcquired = true;
    }
}
if ((EntityTypeNameValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("EntityTypeName");
    if ((data != null))
    {
        if ((typeof(string).IsAssignableFrom(data.GetType()) == false))
        {
            this.Error("The type \'System.String\' of the parameter \'EntityTypeName\' did not match the type" +
                    " of the data passed to the template.");
        }
        else
        {
            this._EntityTypeNameField = ((string)(data));
        }
    }
}
bool ViewNameValueAcquired = false;
if (this.Session.ContainsKey("ViewName"))
{
    if ((typeof(string).IsAssignableFrom(this.Session["ViewName"].GetType()) == false))
    {
        this.Error("The type \'System.String\' of the parameter \'ViewName\' did not match the type of th" +
                "e data passed to the template.");
    }
    else
    {
        this._ViewNameField = ((string)(this.Session["ViewName"]));
        ViewNameValueAcquired = true;
    }
}
if ((ViewNameValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("ViewName");
    if ((data != null))
    {
        if ((typeof(string).IsAssignableFrom(data.GetType()) == false))
        {
            this.Error("The type \'System.String\' of the parameter \'ViewName\' did not match the type of th" +
                    "e data passed to the template.");
        }
        else
        {
            this._ViewNameField = ((string)(data));
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
    public class ListViewTemplateBase
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
