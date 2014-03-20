using System;

namespace Coevery.DeveloperTools.CodeGeneration.Services {
    public interface IDynamicAssemblyBuilder : IDependency {
        bool Build(string moduleId);
        Type GetFieldType(string fieldNameType);
    }
}