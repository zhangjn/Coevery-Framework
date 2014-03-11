using Coevery.ContentManagement;

namespace Coevery.DeveloperTools.Providers {
    public interface IContentFieldValueProvider : IDependency {
        object GetValue(ContentItem contentItem, ContentField field);
    }
}