﻿using System.Web.Mvc;
using Coevery.ContentManagement.Handlers;

namespace Coevery.Core.Forms.Services {
    public interface IFormManager : IDependency {
        dynamic Build(string formName, string prefix = "");
        dynamic Bind(dynamic formShape, IValueProvider state, string prefix = "");
        void Validate(ValidatingContext context);
        
        string Export(string formName, string state, ExportContentContext exportContext);
        string Import(string formName, string state, ImportContentContext exportContext);
    }
}