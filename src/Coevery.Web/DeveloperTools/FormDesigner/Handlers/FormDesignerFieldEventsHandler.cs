using Coevery.Core.Entities.Events;
using Coevery.DeveloperTools.FormDesigner.Services;

namespace Coevery.DeveloperTools.FormDesigner.Handlers {
    public class FormDesignerFieldEventsHandler : IFieldEvents {
        private readonly ILayoutManager _layoutManager;

        public FormDesignerFieldEventsHandler(ILayoutManager layoutManager) {
            _layoutManager = layoutManager;
        }

        public void OnCreated(string entityName, string fieldName, bool isInLayout) {
            if (isInLayout) {
                _layoutManager.AddField(entityName, fieldName);
            }
        }

        public void OnDeleting(string entityName, string fieldName) {
            _layoutManager.DeleteField(entityName, fieldName);
        }
    }
}