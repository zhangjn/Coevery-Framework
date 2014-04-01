using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.VisualStudio.TextTemplating;

namespace Coevery.DeveloperTools.CodeGeneration.Services {
    public interface ITemplateGenerator : IDependency {
        string ProcessTemplate(string templateFileName, ITextTemplatingSession session = null);
    }
}