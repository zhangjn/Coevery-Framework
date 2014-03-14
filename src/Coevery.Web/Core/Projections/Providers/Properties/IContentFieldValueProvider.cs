using Coevery.ContentManagement;

namespace Coevery.Core.Projections.Providers.Properties {
    public interface IContentFieldValueProvider : IDependency {
        object GetValue(ContentItem contentItem, ContentField field);
    }
}