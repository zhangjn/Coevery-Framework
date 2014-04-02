using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Web;
using System.Web.Hosting;
using Coevery.Environment;
using Microsoft.CSharp;
using Microsoft.VisualStudio.TextTemplating;

namespace Coevery.DeveloperTools.CodeGeneration.Services {
    public class DefaultTemplateGenerator : ITemplateGenerator {
        private readonly IAssemblyLoader _assemblyLoader;
        public DefaultTemplateGenerator(IAssemblyLoader assemblyLoader) {
            _assemblyLoader = assemblyLoader;
        }

        public string ProcessTemplate(string templateFileName, ITextTemplatingSession session = null) {
            var codeGenTemplatePath = HostingEnvironment.MapPath("~/DeveloperTools/CodeGeneration/CodeGenerationTemplates/");
            var templateFilePath = codeGenTemplatePath + templateFileName;
            string input = File.ReadAllText(templateFilePath);
            var host = new SimpleTextTemplatingHost(_assemblyLoader);
            host.TemplateFileValue = templateFilePath;
            if (session != null) {
                var sessionHost = (ITextTemplatingSessionHost) host;
                sessionHost.Session = session;
            }

            //Transform the text template.
            Engine engine = new Engine();
            string output = engine.ProcessTemplate(input, host);
            if (host.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Template:" + templateFilePath);
                foreach (CompilerError error in host.Errors)
                {
                    sb.AppendLine(String.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));
                }

                throw new InvalidOperationException(sb.ToString());
            }
            return output;
        }
    }
}