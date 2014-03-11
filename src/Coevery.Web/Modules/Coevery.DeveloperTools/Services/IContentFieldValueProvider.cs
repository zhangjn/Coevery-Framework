using Coevery.ContentManagement;

namespace Coevery.DeveloperTools.Services {
    public interface IContentFieldValueProvider : IDependency {
        object GetValue(ContentItem contentItem, ContentField field);
    }
}