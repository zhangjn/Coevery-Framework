using Coevery.ContentManagement.MetaData.Models;
using Coevery.Core.Entities.Events;
using Coevery.DeveloperTools.FormDesigner.Services;

namespace Coevery.DeveloperTools.FormDesigner.Handlers {
    public class FormDesignerFieldEventsHandler : IFieldEvents {
        private readonly ILayoutManager _layoutManager;

        public FormDesignerFieldEventsHandler(ILayoutManager layoutManager) {
            _layoutManager = layoutManager;
        }

        public void OnAdding(string entityName, string fieldName, SettingsDictionary settings) {
            _layoutManager.AddField(fieldName, settings);
        }

        public void OnDeleting(string entityName, string fieldName) {
            _layoutManager.DeleteField(entityName, fieldName);
        }
    }
}