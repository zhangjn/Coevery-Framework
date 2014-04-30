using System.Collections.Generic;
using System.Linq;
using Coevery.ContentManagement;
using Coevery.ContentManagement.Handlers;
using Coevery.ContentManagement.MetaData;
using Coevery.ContentManagement.MetaData.Services;
using Coevery.Core.Common.Extensions;
using Coevery.Core.Settings.Metadata.Parts;
using Coevery.Core.Settings.Metadata.Records;
using Coevery.Data;
using JetBrains.Annotations;

namespace Coevery.Core.Settings.Metadata.Handlers {
    [UsedImplicitly]
    public class ContentTypeDefinitionPartHandler : ContentHandler {
        private readonly ISettingsFormatter _settingsFormatter;
        private readonly IContentManager _contentManager;
        private readonly IContentDefinitionEditorEvents _contentDefinitionEditorEvents;

        public ContentTypeDefinitionPartHandler(
            IRepository<ContentTypeDefinitionRecord> typeDefinitionRepository,
            ISettingsFormatter settingsFormatter,
            IContentManager contentManager,
            IContentDefinitionEditorEvents contentDefinitionEditorEvents) {
            _settingsFormatter = settingsFormatter;
            _contentManager = contentManager;
            _contentDefinitionEditorEvents = contentDefinitionEditorEvents;
            Filters.Add(new ActivatingFilter<ContentTypeDefinitionPart>("ContentTypeDefinition"));
            Filters.Add(StorageFilter.For(typeDefinitionRepository));

            OnActivated<ContentTypeDefinitionPart>((context, part) => LazyLoadHandlers(part));
            OnVersioning<ContentTypeDefinitionPart>(OnVersioning);
            OnCreating<ContentTypeDefinitionPart>(OnCreating);
            OnRemoving<ContentTypeDefinitionPart>(OnRemoving);
            OnPublishing<ContentTypeDefinitionPart>(OnPublishing);
        }

        private void OnPublishing(PublishContentContext context, ContentTypeDefinitionPart part) {
            var partDefinition = GetPartDefinition(part.Name, VersionOptions.Latest);
            _contentManager.Publish(partDefinition.ContentItem);
        }

        private void LazyLoadHandlers(ContentTypeDefinitionPart part) {
            part._entitySettings.Loader(() => _settingsFormatter.Parse(part.Record.Settings));
            part._entitySettings.Setter(value => {
                part.Record.Settings = _settingsFormatter.Parse(value);
                return value;
            });
        }

        private void OnVersioning(VersionContentContext context, ContentTypeDefinitionPart existing, ContentTypeDefinitionPart building) {
            building.Record.ContentTypePartDefinitionRecords = new List<ContentTypePartDefinitionRecord>();
            var partDefinition = GetPartDefinition(existing.Name, VersionOptions.DraftRequired);

            building.Record.ContentTypePartDefinitionRecords.Add(new ContentTypePartDefinitionRecord {
                ContentPartDefinitionRecord = partDefinition.Record
            });
        }

        private void OnCreating(CreateContentContext context, ContentTypeDefinitionPart part) {
            var partDefinition = _contentManager.New<ContentPartDefinitionPart>("ContentPartDefinition");
            partDefinition.Name = part.Name.ToPartName();
            _contentManager.Create(partDefinition, VersionOptions.Draft);

            part.Record.ContentTypePartDefinitionRecords.Add(new ContentTypePartDefinitionRecord {
                ContentPartDefinitionRecord = partDefinition.Record
            });
        }

        private void OnRemoving(RemoveContentContext context, ContentTypeDefinitionPart part) {
            var partDefinition = GetPartDefinition(part.Name, VersionOptions.Latest);
            foreach (var field in partDefinition.FieldDefinitions) {
                _contentDefinitionEditorEvents.FieldDeleted(field.FieldType, field.Name, field.Settings);
            }

            part.Record.ContentTypePartDefinitionRecords.Clear();
            _contentManager.Remove(partDefinition.ContentItem);
        }

        private ContentPartDefinitionPart GetPartDefinition(string entityName, VersionOptions options) {
            var partName = entityName.ToPartName();
            return _contentManager.Query<ContentPartDefinitionPart, ContentPartDefinitionRecord>(options)
                .Where(x => x.Name == partName)
                .List()
                .FirstOrDefault();
        }
    }
}