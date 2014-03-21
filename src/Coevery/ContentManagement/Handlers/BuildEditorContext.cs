using Coevery.DisplayManagement;

namespace Coevery.ContentManagement.Handlers {
    public class BuildEditorContext : BuildShapeContext {
        public BuildEditorContext(IShape model, IContent content, string displayType, string groupId, IShapeFactory shapeFactory)
            : base(model, content, groupId, shapeFactory) {
            DisplayType = displayType;
        }

        public string DisplayType { get; private set; }
    }
}