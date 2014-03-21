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
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ControllerTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "11.0.0.0")]
    public partial class ControllerTemplate : ControllerTemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("using System.Web.Mvc;\r\nusing Coevery;\r\nusing Coevery.ContentManagement;\r\nusing Co" +
                    "every.Data;\r\nusing Coevery.Localization;\r\nusing ");
            
            #line 9 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Namespace));
            
            #line default
            #line hidden
            this.Write(".Models;\r\n\r\nnamespace ");
            
            #line 11 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Namespace));
            
            #line default
            #line hidden
            this.Write(".Controllers {\r\n\t[Themed]\r\n    public class ");
            
            #line 13 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(EntityName));
            
            #line default
            #line hidden
            this.Write("Controller : Controller , IUpdateModel{\r\n        private readonly ITransactionMan" +
                    "ager _transactionManager;\r\n\r\n        public ");
            
            #line 16 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(EntityName));
            
            #line default
            #line hidden
            this.Write(@"Controller (ICoeveryServices services, ITransactionManager transactionManager) {
            Services = services;
            _transactionManager = transactionManager;
            T = NullLocalizer.Instance;
        }

		public ICoeveryServices Services { get; set; }
		public Localizer T { get; set; }

		public ActionResult Index() {
            var contentItem = Services.ContentManager.New(""");
            
            #line 26 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(EntityName));
            
            #line default
            #line hidden
            this.Write("\");\r\n            contentItem.Weld(new ");
            
            #line 27 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(EntityName));
            
            #line default
            #line hidden
            this.Write(@"Part());
            var model = Services.ContentManager.BuildDisplay(contentItem, ""List"");
            return View(model);
        }

        public ActionResult Detail(int id) {
            var contentItem = Services.ContentManager.Get(id, VersionOptions.Latest);
            if (contentItem == null) {
                return HttpNotFound();
            }

            dynamic model = Services.ContentManager.BuildDisplay(contentItem, ""Detail"");
            return View((object) model);
        }

        public ActionResult Create() {
            var contentItem = Services.ContentManager.New(""");
            
            #line 43 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(EntityName));
            
            #line default
            #line hidden
            this.Write(@""");
            var model = Services.ContentManager.BuildEditor(contentItem, ""Create"");
            return View(model);
        }

        [HttpPost, ActionName(""Create"")]
        public ActionResult CreatePost() {
            var contentItem = Services.ContentManager.New(""");
            
            #line 50 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(EntityName));
            
            #line default
            #line hidden
            this.Write("\");\r\n            dynamic model = Services.ContentManager.UpdateEditor(contentItem" +
                    ", this, \"Create\");\r\n            if (!ModelState.IsValid) {\r\n                retu" +
                    "rn View(\"Create\", (object) model);\r\n            }\r\n            Services.ContentM" +
                    "anager.Create(contentItem, VersionOptions.Draft);\r\n            Services.ContentM" +
                    "anager.Publish(contentItem);\r\n            return RedirectToAction(\"Edit\", new { " +
                    "id = contentItem.Id });\r\n        }\r\n\r\n        public ActionResult Edit(int id) {" +
                    "\r\n            var contentItem = Services.ContentManager.Get(id, VersionOptions.L" +
                    "atest);\r\n            if (contentItem == null) {\r\n                return HttpNotF" +
                    "ound();\r\n            }\r\n\r\n            dynamic model = Services.ContentManager.Bu" +
                    "ildEditor(contentItem, \"Edit\");\r\n            return View((object)model);\r\n      " +
                    "  }\r\n\r\n        [HttpPost, ActionName(\"Edit\")]\r\n        public ActionResult EditP" +
                    "ost(int id, FormCollection collection) {\r\n\t\t\tvar contentItem = Services.ContentM" +
                    "anager.Get(id, VersionOptions.Latest);\r\n            if (contentItem == null) {\r\n" +
                    "                return HttpNotFound();\r\n            }\r\n\r\n            dynamic mod" +
                    "el = Services.ContentManager.UpdateEditor(contentItem, this, \"Edit\");\r\n         " +
                    "   if (!ModelState.IsValid) {\r\n                _transactionManager.Cancel();\r\n  " +
                    "              return View(\"Edit\", (object) model);\r\n            }\r\n            r" +
                    "eturn RedirectToAction(\"Edit\", new {id});\r\n        }\r\n\r\n        public ActionRes" +
                    "ult Delete(int id) {\r\n            return View();\r\n        }\r\n\r\n        [HttpPost" +
                    "]\r\n        public ActionResult DeletePost(int id, FormCollection collection) {\r\n" +
                    "            try {\r\n                // TODO: Add delete logic here\r\n\r\n           " +
                    "     return RedirectToAction(\"Index\");\r\n            }\r\n            catch {\r\n    " +
                    "            return View();\r\n            }\r\n        }\r\n\r\n        bool IUpdateMode" +
                    "l.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties" +
                    ", string[] excludeProperties) {\r\n            return TryUpdateModel(model, prefix" +
                    ", includeProperties, excludeProperties);\r\n        }\r\n\r\n        void IUpdateModel" +
                    ".AddModelError(string key, LocalizedString errorMessage) {\r\n            ModelSta" +
                    "te.AddModelError(key, errorMessage.ToString());\r\n        }\r\n    }\r\n}\r\n");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 1 "F:\Shinetech\Coevery-Framework-V1\src\Coevery.Web\DeveloperTools\CodeGeneration\CodeGenerationTemplates\ControllerTemplate.tt"

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

private string _EntityNameField;

/// <summary>
/// Access the EntityName parameter of the template.
/// </summary>
private string EntityName
{
    get
    {
        return this._EntityNameField;
    }
}


/// <summary>
/// Initialize the template
/// </summary>
public virtual void Initialize()
{
    if ((this.Errors.HasErrors == false))
    {
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
bool EntityNameValueAcquired = false;
if (this.Session.ContainsKey("EntityName"))
{
    if ((typeof(string).IsAssignableFrom(this.Session["EntityName"].GetType()) == false))
    {
        this.Error("The type \'System.String\' of the parameter \'EntityName\' did not match the type of " +
                "the data passed to the template.");
    }
    else
    {
        this._EntityNameField = ((string)(this.Session["EntityName"]));
        EntityNameValueAcquired = true;
    }
}
if ((EntityNameValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("EntityName");
    if ((data != null))
    {
        if ((typeof(string).IsAssignableFrom(data.GetType()) == false))
        {
            this.Error("The type \'System.String\' of the parameter \'EntityName\' did not match the type of " +
                    "the data passed to the template.");
        }
        else
        {
            this._EntityNameField = ((string)(data));
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
    public class ControllerTemplateBase
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
