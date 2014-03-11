using System;

namespace Coevery.DeveloperTools.DynamicTypeGeneration {
    public interface IDynamicAssemblyBuilder : IDependency {
        bool Build();
        Type GetFieldType(string fieldNameType);
    }
}