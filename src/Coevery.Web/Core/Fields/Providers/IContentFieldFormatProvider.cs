using Coevery.ContentManagement;

namespace Coevery.Core.Fields.Providers {
    public interface IContentFieldFormatProvider : IDependency {
        void SetFormat(ContentField field, dynamic formState);
    }
}