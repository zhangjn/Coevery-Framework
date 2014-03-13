﻿using System;
using Coevery.DeveloperTools.Projections.Models;

namespace Coevery.DeveloperTools.Projections.Services {
    public interface IFieldIndexService : IDependency {
        void Set(FieldIndexPart part, string partName, string fieldName, string valueName, object value, Type valueType);
        T Get<T>(FieldIndexPart part, string partName, string fieldName, string valueName);
    }
}
