using Coevery.ContentManagement;
using Coevery.ContentManagement.Handlers;
using Coevery.Core.Common.Models;
using Coevery.Data;
using JetBrains.Annotations;

namespace Coevery.DeveloperTools.Perspectives.Handlers {
    [UsedImplicitly]
    public class ModuleMenuItemPartHandler : ContentHandler {
        private readonly IContentManager _contentManager;

        public ModuleMenuItemPartHandler(IContentManager contentManager, IRepository<ModuleMenuItemPartRecord> repository) {
            _contentManager = contentManager;
            Filters.Add(StorageFilter.For(repository));

            //OnLoading<ModuleMenuItemPart>((context, part) => part..Loader(p => contentManager.Get(part.Record.ContentTypeRecored.Id)));
        }

        protected override void GetItemMetadata(GetContentItemMetadataContext context) {
            base.GetItemMetadata(context);

            if (context.ContentItem.ContentType != "ModuleMenuItem") {
                return;
            }

            var moduleMenuItemPart = context.ContentItem.As<ModuleMenuItemPart>();
            // the display route for the menu item is the one for the referenced content item
            if (moduleMenuItemPart != null) {
                // if the content doesn't exist anymore
                if (moduleMenuItemPart.Record.ContentTypeDefinitionRecord == null) {
                    return;
                }

                // context.Metadata.DisplayRouteValues = _contentManager.GetItemMetadata(moduleMenuItemPart.Content).DisplayRouteValues;
            }
        }
    }
}