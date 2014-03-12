using Coevery.ContentManagement;

namespace Coevery.DeveloperTools.Entities.Providers {
    public interface IContentFieldValueProvider : IDependency {
        object GetValue(ContentItem contentItem, ContentField field);
    }
}