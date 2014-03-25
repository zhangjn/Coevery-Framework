using Coevery.ContentManagement;
using Coevery.Core.Common.Extensions;
using Coevery.Core.Common.Models;
using Coevery.Core.Entities.Events;
using Coevery.Core.Navigation.Models;

namespace Coevery.DeveloperTools.Perspectives.Handlers {
    public class PerspectiveEntityEventsHandler : IEntityEvents {
        private readonly IContentDefinitionExtension _contentService;
        private readonly IContentManager _contentManager;

        public PerspectiveEntityEventsHandler(
            IContentDefinitionExtension contentService,
            IContentManager contentManager) {
            _contentService = contentService;
            _contentManager = contentManager;
        }

        public void OnCreated(string entityName) { }
        public void OnUpdating(string entityName) {
            var menuItems = _contentManager.Query<ModuleMenuItemPart>().List();
            foreach (var navigation in menuItems)
            {
                if (navigation.ContentTypeDefinitionRecord.Name == entityName && navigation.Is<MenuPart>())
                {
                    navigation.As<MenuPart>().MenuText =
                        _contentService.GetEntityNames(entityName).CollectionDisplayName;
                }
            }
        }

        public void OnDeleting(string entityName) {
            foreach (var navigation in _contentManager.List<MenuPart>("ModuleMenuItem")) {
                if (navigation.MenuText == _contentService.GetEntityNames(entityName).CollectionDisplayName) {
                    _contentManager.Remove(navigation.ContentItem);
                }
            }
        }
    }
}