using System;
using System.Collections.Generic;
using System.Linq;
using Coevery.ContentManagement;
using Coevery.ContentManagement.Handlers;
using Coevery.ContentManagement.MetaData;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.Core.Common.Extensions;
using Coevery.Core.Settings.Metadata.Parts;
using Coevery.Core.Settings.Metadata.Records;
using Coevery.Data.Migration.Schema;
using Coevery.DeveloperTools.EntityManagement.Services;

namespace Coevery.DeveloperTools.EntityManagement.Handlers {
    public class ContentTypeDefinitionPartHandler : ContentHandler {
        private readonly IContentManager _contentManager;
        private readonly IContentDefinitionEditorEvents _contentDefinitionEditorEvents;
        private readonly ISchemaUpdateService _schemaUpdateService;

        public ContentTypeDefinitionPartHandler(
            IContentManager contentManager,
            IContentDefinitionEditorEvents contentDefinitionEditorEvents,
            ISchemaUpdateService schemaUpdateService) {
            _contentManager = contentManager;
            _contentDefinitionEditorEvents = contentDefinitionEditorEvents;
            _schemaUpdateService = schemaUpdateService;
            OnVersioning<ContentTypeDefinitionPart>(OnVersioning);
            OnCreating<ContentTypeDefinitionPart>(OnCreating);
            OnRemoving<ContentTypeDefinitionPart>(OnRemoving);
            OnPublishing<ContentTypeDefinitionPart>(OnPublishing);
        }

        private void OnPublishing(PublishContentContext context, ContentTypeDefinitionPart part) {
            if (!part.Customized) {
                return;
            }

            var publishedPartDefinition = GetPartDefinition(part.Name, VersionOptions.Published);
            var lastPartDefinition = GetPartDefinition(part.Name, VersionOptions.Latest);
            _contentManager.Publish(lastPartDefinition.ContentItem);

            if (context.PreviousItemVersionRecord == null) {
                _schemaUpdateService.CreateTable(part.Name, ctx => {
                    foreach (var fieldDefinition in lastPartDefinition.FieldDefinitions) {
                        var length = GetMaxLength(fieldDefinition.Settings);
                        Action<CreateColumnCommand> columnAction = x => x.WithLength(length);
                        ctx.FieldColumn(fieldDefinition.Name, fieldDefinition.FieldType, columnAction);
                    }
                });
            }
            else {
                var newFieldDefinitions = lastPartDefinition.FieldDefinitions;
                var oldFieldDefinitions = publishedPartDefinition.FieldDefinitions;
                foreach (var fieldDefinition in oldFieldDefinitions) {
                    var exist = newFieldDefinitions.Any(x => x.Name == fieldDefinition.Name);
                    if (!exist) {
                        _schemaUpdateService.DropColumn(part.Name, fieldDefinition.Name);
                    }
                }

                foreach (var fieldDefinition in newFieldDefinitions) {
                    var exist = oldFieldDefinitions.Any(x => x.Name == fieldDefinition.Name);
                    if (!exist) {
                        var length = GetMaxLength(fieldDefinition.Settings);
                        _schemaUpdateService.CreateColumn(part.Name, fieldDefinition.Name, fieldDefinition.FieldType, length);
                    }
                    else {
                        var length = GetMaxLength(fieldDefinition.Settings);
                        _schemaUpdateService.AlterColumn(part.Name, fieldDefinition.Name, fieldDefinition.FieldType, length);
                    }
                }
            }
        }

        private void OnVersioning(VersionContentContext context, ContentTypeDefinitionPart existing, ContentTypeDefinitionPart building) {
            if (!existing.Customized) {
                return;
            }

            building.Record.ContentTypePartDefinitionRecords = new List<ContentTypePartDefinitionRecord>();
            var partDefinition = GetPartDefinition(existing.Name, VersionOptions.DraftRequired);

            building.Record.ContentTypePartDefinitionRecords.Add(new ContentTypePartDefinitionRecord {
                ContentPartDefinitionRecord = partDefinition.Record
            });
        }

        private void OnCreating(CreateContentContext context, ContentTypeDefinitionPart part) {
            if (!part.Customized) {
                return;
            }

            var partDefinition = _contentManager.New<ContentPartDefinitionPart>("ContentPartDefinition");
            partDefinition.Name = part.Name.ToPartName();
            _contentManager.Create(partDefinition, VersionOptions.Draft);

            part.Record.ContentTypePartDefinitionRecords.Add(new ContentTypePartDefinitionRecord {
                ContentPartDefinitionRecord = partDefinition.Record
            });
        }

        private void OnRemoving(RemoveContentContext context, ContentTypeDefinitionPart part) {
            if (!part.Customized) {
                return;
            }

            var hasPublished = part.HasPublished();
            if (hasPublished) {
                _schemaUpdateService.DropTable(part.Name);
            }

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

        private int? GetMaxLength(SettingsDictionary settings) {
            string maxLengthSetting;
            if (settings.TryGetValue("TextFieldSettings.MaxLength", out maxLengthSetting)) {
                int length;
                if (int.TryParse(maxLengthSetting, out length)) {
                    return length;
                }
            }
            return null;
        }
    }
}