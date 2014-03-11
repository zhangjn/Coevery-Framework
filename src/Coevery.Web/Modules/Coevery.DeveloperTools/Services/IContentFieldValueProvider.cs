using Coevery.ContentManagement;

namespace Coevery.Core.Entities.Services {
    public interface IContentFieldValueProvider : IDependency {
        object GetValue(ContentItem contentItem, ContentField field);
    }
}