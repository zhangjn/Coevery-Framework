using System;

namespace Coevery.DeveloperTools.Entities.DynamicTypeGeneration {
    public interface IDynamicAssemblyBuilder : IDependency {
        bool Build();
        Type GetFieldType(string fieldNameType);
    }
}