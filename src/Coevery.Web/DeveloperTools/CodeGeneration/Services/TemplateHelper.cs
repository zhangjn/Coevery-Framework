using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using Microsoft.VisualStudio.TextTemplating;

namespace Coevery.DeveloperTools.CodeGeneration.Services
{
    public static class TemplateHelper
    {
        public static string ProcessTemplate(string templateFileName, ITextTemplatingSession session = null)
        {
            var codeGenTemplatePath = HostingEnvironment.MapPath("~/DeveloperTools/CodeGeneration/CodeGenerationTemplates/");
            var templateFilePath = codeGenTemplatePath + templateFileName;
            string input = File.ReadAllText(templateFilePath);
            var host = new SimpleTextTemplatingHost();
            host.TemplateFileValue = templateFilePath;
            if (session != null)
            {
                var sessionHost = (ITextTemplatingSessionHost)host;
                sessionHost.Session = session;
            }

            //Transform the text template.
            Engine engine = new Engine();
            string output = engine.ProcessTemplate(input, host);
            return output;
        }
    }
}