using System.Collections.Generic;
using Coevery.ContentManagement.ViewModels;
using Coevery.DeveloperTools.EntityManagement.ViewModels;

namespace Coevery.DeveloperTools.EntityManagement.Services {
    public interface IContentDefinitionService : IDependency {
        IEnumerable<EditTypeViewModel> GetUserDefinedTypes();
        EditTypeViewModel GetType(string name);
        void RemoveType(string name, bool deleteContent);
        IEnumerable<TemplateViewModel> GetFields();
    }
}