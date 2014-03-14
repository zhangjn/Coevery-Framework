using System;

namespace Coevery.DeveloperTools.EntityManagement.DynamicTypeGeneration {
    public interface IDynamicAssemblyBuilder : IDependency {
        bool Build();
        Type GetFieldType(string fieldNameType);
    }
}