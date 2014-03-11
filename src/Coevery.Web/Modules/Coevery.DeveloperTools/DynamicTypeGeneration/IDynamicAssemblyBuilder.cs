using System;

namespace Coevery.Core.Entities.DynamicTypeGeneration {
    public interface IDynamicAssemblyBuilder : IDependency {
        bool Build();
        Type GetFieldType(string fieldNameType);
    }
}