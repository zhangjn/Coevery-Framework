using Coevery.ContentManagement;

namespace Coevery.Core.Entities.Services {
    public interface IContentFieldFormatProvider : IDependency {
        void SetFormat(ContentField field, dynamic formState);
    }
}