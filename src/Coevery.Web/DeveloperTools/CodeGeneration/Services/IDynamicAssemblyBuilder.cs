using System;

namespace Coevery.DeveloperTools.CodeGeneration.Services {
    public interface IDynamicAssemblyBuilder : IDependency {
        bool Build();
        Type GetFieldType(string fieldNameType);
    }
}