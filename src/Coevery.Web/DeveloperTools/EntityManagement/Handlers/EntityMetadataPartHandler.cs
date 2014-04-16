using System;
using System.Collections.Generic;
using System.Linq;
using Coevery.ContentManagement;
using Coevery.ContentManagement.Handlers;
using Coevery.ContentManagement.MetaData;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.ContentManagement.MetaData.Services;
using Coevery.Core.Common.Extensions;
using Coevery.Core.Entities.Events;
using Coevery.Core.Entities.Models;
using Coevery.Data;
using Coevery.Data.Migration.Schema;
using Coevery.DeveloperTools.EntityManagement.Services;

namespace Coevery.DeveloperTools.EntityManagement.Handlers {
    public class EntityMetadataPartHandler : ContentHandler {
        private readonly IRepository<FieldMetadataRecord> _fieldMetadataRepository;
        private readonly IContentManager _contentManager;
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly ISettingsFormatter _settingsFormatter;
        private readonly IEntityEvents _entityEvents;
        private readonly ISchemaUpdateService _schemaUpdateService;
        private readonly IFieldEvents _fieldEvents;

        public EntityMetadataPartHandler(
            IRepository<EntityMetadataRecord> entityMetadataRepository,
            IRepository<FieldMetadataRecord> fieldMetadataRepository,
            IContentManager contentManager,
            IContentDefinitionManager contentDefinitionManager,
            IEntityEvents entityEvents,
            ISchemaUpdateService schemaUpdateService,
            IFieldEvents fieldEvents,
            ISettingsFormatter settingsFormatter) {
            _fieldMetadataRepository = fieldMetadataRepository;
            _contentManager = contentManager;
            _contentDefinitionManager = contentDefinitionManager;
            _entityEvents = entityEvents;
            _schemaUpdateService = schemaUpdateService;
            _fieldEvents = fieldEvents;
            _settingsFormatter = settingsFormatter;

            Filters.Add(StorageFilter.For(entityMetadataRepository));
            OnInitializing<EntityMetadataPart>((context, part) => part.EntitySetting = new SettingsDictionary());
            OnLoaded<EntityMetadataPart>(LazyLoadHandlers);
            OnVersioning<EntityMetadataPart>(OnVersioning);
            OnPublishing<EntityMetadataPart>(OnPublishing);
        }

        private void LazyLoadHandlers(LoadContentContext context, EntityMetadataPart part) {
            part.EntitySettingsField.Getter(() => _settingsFormatter.Parse(part.Record.Settings));
            part.EntitySettingsField.Setter(value => {
                part.Record.Settings = _settingsFormatter.Parse(value);
            });
        }

        private void OnVersioning(VersionContentContext context, EntityMetadataPart existing, EntityMetadataPart building) {
            building.Record.FieldMetadataRecords = new List<FieldMetadataRecord>();
            foreach (var record in existing.Record.FieldMetadataRecords) {
                var newRecord = new FieldMetadataRecord();
                _fieldMetadataRepository.Copy(record, newRecord);
                newRecord.OriginalId = record.Id;
                newRecord.EntityMetadataRecord = building.Record;
                _fieldMetadataRepository.Create(newRecord);
                building.Record.FieldMetadataRecords.Add(newRecord);
            }
        }

        private void OnPublishing(PublishContentContext context, EntityMetadataPart part) {
            if (context.PreviousItemVersionRecord == null) {
                CreateEntity(part);
            }
            else {
                var previousEntity = _contentManager.Get<EntityMetadataPart>(context.Id);
                UpdateEntity(previousEntity, part);
            }
        }

        private void CreateEntity(EntityMetadataPart part) {
            _contentDefinitionManager.AlterTypeDefinition(part.Name, builder => {
                builder.DisplayedAs(part.DisplayName);
                MergeDictionary(part.EntitySetting, builder.WithSetting);
                builder.WithPart(part.Name.ToPartName());
            });

            foreach (var record in part.FieldMetadataRecords) {
                AddField(part.Name, record, false);
            }  

            _entityEvents.OnCreated(part.Name);

            _schemaUpdateService.CreateTable(part.Name, context => {
                foreach (var fieldMetadataRecord in part.FieldMetadataRecords) {
                    var length = GetMaxLength(fieldMetadataRecord.Settings);
                    Action<CreateColumnCommand> columnAction = x => x.WithLength(length);
                    context.FieldColumn(fieldMetadataRecord.Name,
                        fieldMetadataRecord.ContentFieldDefinitionRecord.Name, columnAction);
                }
            });
        }

        private void UpdateEntity(EntityMetadataPart previousEntity, EntityMetadataPart entity) {
            _contentDefinitionManager.AlterTypeDefinition(entity.Name, builder => {
                MergeDictionary(entity.EntitySetting, builder.WithSetting);
                builder.DisplayedAs(entity.DisplayName);
            });

            foreach (var fieldMetadataRecord in previousEntity.FieldMetadataRecords) {
                var exist = entity.FieldMetadataRecords.Any(x => x.OriginalId == fieldMetadataRecord.Id);
                if (!exist) {
                    var record = fieldMetadataRecord;
                    _contentDefinitionManager.AlterPartDefinition(entity.Name.ToPartName(),
                        typeBuilder => typeBuilder.RemoveField(record.Name));
                    _schemaUpdateService.DropColumn(entity.Name, fieldMetadataRecord.Name);
                    _fieldEvents.OnDeleting(entity.Name, fieldMetadataRecord.Name);
                }
            }

            var needUpdateFields = new List<FieldMetadataRecord>();
            foreach (var fieldMetadataRecord in entity.FieldMetadataRecords) {
                if (fieldMetadataRecord.OriginalId != 0) {
                    needUpdateFields.Add(fieldMetadataRecord);
                }
                else {
                    AddField(entity.Name, fieldMetadataRecord);
                    var length = GetMaxLength(fieldMetadataRecord.Settings);
                    _schemaUpdateService.CreateColumn(entity.Name, fieldMetadataRecord.Name, fieldMetadataRecord.ContentFieldDefinitionRecord.Name, length);
                }
            }

            foreach (var fieldMetadataRecord in needUpdateFields) {
                var record = fieldMetadataRecord;
                var settings = _settingsFormatter.Parse(record.Settings);
                _contentDefinitionManager.AlterPartDefinition(entity.Name.ToPartName(), builder =>
                    builder.WithField(record.Name, fieldBuilder => MergeDictionary(settings, builder.WithSetting)));

                var length = GetMaxLength(fieldMetadataRecord.Settings);
                _schemaUpdateService.AlterColumn(entity.Name, fieldMetadataRecord.Name, fieldMetadataRecord.ContentFieldDefinitionRecord.Name, length);
            }
            _entityEvents.OnUpdating(entity.Name);
        }

        private static void MergeDictionary<T>(SettingsDictionary entitySetting, Func<string,string, T> withSetting) {
            foreach (var pair in entitySetting) {
                withSetting(pair.Key, pair.Value);
            }
        }

        private int? GetMaxLength(string settings) {
            var settingDictionary = _settingsFormatter.Parse(settings);
            string maxLengthSetting;
            if (settingDictionary.TryGetValue("TextFieldSettings.MaxLength", out maxLengthSetting)) {
                int length;
                if (int.TryParse(maxLengthSetting, out length)) {
                    return length;
                }
            }
            return null;
        }

        private void AddField(string entityName, FieldMetadataRecord field, bool needEvent = true) {
            var settings = _settingsFormatter.Parse(field.Settings);

            // add field to part
            _contentDefinitionManager.AlterPartDefinition(entityName.ToPartName(), builder => {
                string fieldTypeName = field.ContentFieldDefinitionRecord.Name;
                builder.WithField(field.Name, fieldBuilder => {
                    fieldBuilder.OfType(fieldTypeName).WithSetting("Storage", "Part");
                    MergeDictionary(settings, builder.WithSetting);
                });
            });

            if (needEvent) {
                _fieldEvents.OnCreated(entityName, field.Name, bool.Parse(settings["AddInLayout"]));
            }
        }
    }
}