using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coevery.ContentManagement.MetaData.Models;

namespace Coevery.ContentManagement.MetaData {
    public interface IContentDefinitionQuery : IDependency {
        IEnumerable<ContentTypeDefinition> ListTypeDefinitions();
        IEnumerable<ContentPartDefinition> ListPartDefinitions();
        IEnumerable<ContentFieldDefinition> ListFieldDefinitions();

        ContentTypeDefinition GetTypeDefinition(string name);
        ContentPartDefinition GetPartDefinition(string name);
    }
}
