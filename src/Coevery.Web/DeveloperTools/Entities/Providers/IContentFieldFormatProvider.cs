using Coevery.ContentManagement;

namespace Coevery.DeveloperTools.Entities.Providers {
    public interface IContentFieldFormatProvider : IDependency {
        void SetFormat(ContentField field, dynamic formState);
    }
}