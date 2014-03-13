using Coevery.ContentManagement;

namespace Coevery.Core.Fields.Providers {
    public interface IContentFieldValueProvider : IDependency {
        object GetValue(ContentItem contentItem, ContentField field);
    }
}