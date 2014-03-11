using System.Collections.Generic;
using Coevery.ContentManagement.ViewModels;
using Coevery.Core.Entities.ViewModels;

namespace Coevery.Core.Entities.Services {
    public interface IContentDefinitionService : IDependency {
        IEnumerable<EditTypeViewModel> GetUserDefinedTypes();
        EditTypeViewModel GetType(string name);
        void RemoveType(string name, bool deleteContent);
        IEnumerable<TemplateViewModel> GetFields();
        
    }
}