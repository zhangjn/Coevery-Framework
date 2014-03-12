using Coevery.DeveloperTools.Entities.Events;
using Coevery.DeveloperTools.Formdesigner.Services;

namespace Coevery.DeveloperTools.Formdesigner.Handlers {
    public class FormDesignerEntityEventsHandler : IEntityEvents {
        private readonly ILayoutManager _layoutManager;

        public FormDesignerEntityEventsHandler(ILayoutManager layoutManager) {
            _layoutManager = layoutManager;
        }

        public void OnCreated(string entityName) {
            _layoutManager.GenerateDefaultLayout(entityName);
        }

        public void OnDeleting(string entityName) {
        }

        public void OnUpdating(string entityName) {
        }
    }
}