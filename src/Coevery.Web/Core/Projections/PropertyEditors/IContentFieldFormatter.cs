using Coevery.ContentManagement;

namespace Coevery.Core.Projections.PropertyEditors {
    public interface IContentFieldFormatter : IDependency {
        void SetFormat(ContentField field, dynamic formState);
    }
}