using Coevery.ContentManagement;

namespace Coevery.DeveloperTools.Services {
    public interface IContentFieldFormatProvider : IDependency {
        void SetFormat(ContentField field, dynamic formState);
    }
}