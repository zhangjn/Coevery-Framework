using Coevery.ContentManagement;

namespace Coevery.DeveloperTools.Providers {
    public interface IContentFieldFormatProvider : IDependency {
        void SetFormat(ContentField field, dynamic formState);
    }
}