using System.Collections.Generic;
using System.Linq;
using Coevery.Core.Common.Extensions;
using Coevery.DeveloperTools.Perspectives.ViewModels;

namespace Coevery.DeveloperTools.Perspectives.Services
{
    public class ContentDefinitionService : IContentDefinitionService
    {
        private readonly IContentDefinitionExtension _contentDefinitionExtension;

        public ContentDefinitionService(IContentDefinitionExtension contentDefinitionExtension)
        {
            _contentDefinitionExtension = contentDefinitionExtension;
        }


        public IEnumerable<EditTypeViewModel> GetUserDefinedTypes()
        {
            return _contentDefinitionExtension.ListUserDefinedTypeDefinitions()==null?null: 
                _contentDefinitionExtension.ListUserDefinedTypeDefinitions().Select(ctd => new EditTypeViewModel(ctd)).OrderBy(m => m.DisplayName);
        }
    }
}