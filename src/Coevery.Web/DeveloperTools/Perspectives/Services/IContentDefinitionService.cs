using System.Collections.Generic;
using Coevery.DeveloperTools.Perspectives.ViewModels;

namespace Coevery.DeveloperTools.Perspectives.Services
{
    public interface IContentDefinitionService : IDependency {
        IEnumerable<EditTypeViewModel> GetUserDefinedTypes();
    }
}