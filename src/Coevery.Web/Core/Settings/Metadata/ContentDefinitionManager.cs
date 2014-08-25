using System.Linq;
using System.Xml.Linq;
using Coevery.Caching;
using Coevery.ContentManagement;
using Coevery.ContentManagement.MetaData;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.ContentManagement.MetaData.Services;
using Coevery.Core.Settings.Metadata.Parts;
using Coevery.Core.Settings.Metadata.Records;
using Coevery.Data;

namespace Coevery.Core.Settings.Metadata {
    public class ContentDefinitionManager : Component, IContentDefinitionManager {
        private readonly ISignals _signals;
        private readonly IRepository<ContentFieldDefinitionRecord> _fieldDefinitionRepository;
        private readonly ISettingsFormatter _settingsFormatter;
        private readonly IContentDefinitionQuery _contentDefinitionQuery;
        private readonly IContentManager _contentManager;

        public ContentDefinitionManager(
            ISignals signals,
            IRepository<ContentFieldDefinitionRecord> fieldDefinitionRepository,
            ISettingsFormatter settingsFormatter, 
            IContentDefinitionQuery contentDefinitionQuery, 
            IContentManager contentManager) {
            _signals = signals;
            _fieldDefinitionRepository = fieldDefinitionRepository;
            _settingsFormatter = settingsFormatter;
            _contentDefinitionQuery = contentDefinitionQuery;
            _contentManager = contentManager;
        }

        public ContentTypeDefinition GetTypeDefinition(string name) {
            return _contentDefinitionQuery.GetTypeDefinition(name);
        }

        public void DeleteTypeDefinition(string name) {
            var part = _contentManager.Query<ContentTypeDefinitionPart, ContentTypeDefinitionRecord>()
                .Where(x => x.Name == name).List().SingleOrDefault();

            if (part != null) {
                _contentManager.Remove(part.ContentItem);
            }

            // invalidates the cache
            TriggerContentDefinitionSignal();
        }

        public void DeletePartDefinition(string name) {
            // remove parts from current types
            var typesWithPart = _contentDefinitionQuery.ListTypeDefinitions().Where(typeDefinition => typeDefinition.Parts.Any(part => part.PartDefinition.Name == name));

            foreach (var typeDefinition in typesWithPart) {
                this.AlterTypeDefinition(typeDefinition.Name, builder => builder.RemovePart(name));
            }

            var partRecord = _contentManager.Query<ContentPartDefinitionPart, ContentPartDefinitionRecord>()
                .Where(x => x.Name == name).List().SingleOrDefault();

            if (partRecord != null) {
                _contentManager.Remove(partRecord.ContentItem);
            }

            // invalidates the cache
            TriggerContentDefinitionSignal();

        }

        public ContentPartDefinition GetPartDefinition(string name) {
            return _contentDefinitionQuery.GetPartDefinition(name);
        }

        public void StoreTypeDefinition(ContentTypeDefinition contentTypeDefinition, VersionOptions options) {
            Apply(contentTypeDefinition, Acquire(contentTypeDefinition, options), options);
            TriggerContentDefinitionSignal();
        }

        public void StorePartDefinition(ContentPartDefinition contentPartDefinition, VersionOptions options) {
            Apply(contentPartDefinition, Acquire(contentPartDefinition, options));
            TriggerContentDefinitionSignal();
        }

        private void TriggerContentDefinitionSignal() {
            _signals.Trigger(ContentDefinitionQuery.ContentDefinitionSignal);
        }

        private ContentTypeDefinitionRecord Acquire(ContentTypeDefinition contentTypeDefinition, VersionOptions options) {
            var part = _contentManager.Query<ContentTypeDefinitionPart, ContentTypeDefinitionRecord>()
                .ForVersion(VersionOptions.DraftRequired)
                .Where(x => x.Name == contentTypeDefinition.Name).List().SingleOrDefault();
            if (part == null) {
                part = _contentManager.New<ContentTypeDefinitionPart>("ContentTypeDefinition");
                part.Record.Name = contentTypeDefinition.Name;
                part.Record.DisplayName = contentTypeDefinition.DisplayName;
                _contentManager.Create(part.ContentItem, options);
            }
            else if (options.IsPublished) {
                _contentManager.Publish(part.ContentItem);
            }
            return part.Record;
        }

        private ContentPartDefinitionRecord Acquire(ContentPartDefinition contentPartDefinition, VersionOptions options) {
            var part = _contentManager.Query<ContentPartDefinitionPart, ContentPartDefinitionRecord>()
                .ForVersion(VersionOptions.DraftRequired)
                .Where(x => x.Name == contentPartDefinition.Name).List().SingleOrDefault();
            if (part == null) {
                part = _contentManager.New<ContentPartDefinitionPart>("ContentPartDefinition");
                part.Record.Name = contentPartDefinition.Name;
                _contentManager.Create(part.ContentItem, options);
            }
            return part.Record;
        }

        private ContentFieldDefinitionRecord Acquire(ContentFieldDefinition contentFieldDefinition) {
            var result = _fieldDefinitionRepository.Table.SingleOrDefault(x => x.Name == contentFieldDefinition.Name);
            if (result == null) {
                result = new ContentFieldDefinitionRecord { Name = contentFieldDefinition.Name };
                _fieldDefinitionRepository.Create(result);
            }
            return result;
        }

        private void Apply(ContentTypeDefinition model, ContentTypeDefinitionRecord record, VersionOptions options) {
            record.DisplayName = model.DisplayName;
            record.Settings = _settingsFormatter.Map(model.Settings).ToString();

            var toRemove = record.ContentTypePartDefinitionRecords
                .Where(partDefinitionRecord => model.Parts.All(part => partDefinitionRecord.ContentPartDefinitionRecord.Name != part.PartDefinition.Name))
                .ToList();

            foreach (var remove in toRemove) {
                record.ContentTypePartDefinitionRecords.Remove(remove);
            }

            foreach (var part in model.Parts) {
                var partName = part.PartDefinition.Name;
                var typePartRecord = record.ContentTypePartDefinitionRecords.SingleOrDefault(r => r.ContentPartDefinitionRecord.Name == partName);
                if (typePartRecord == null) {
                    typePartRecord = new ContentTypePartDefinitionRecord {ContentPartDefinitionRecord = Acquire(part.PartDefinition, options)};
                    record.ContentTypePartDefinitionRecords.Add(typePartRecord);
                }
                Apply(part, typePartRecord);
            }
        }

        private void Apply(ContentTypePartDefinition model, ContentTypePartDefinitionRecord record) {
            record.Settings = Compose(_settingsFormatter.Map(model.Settings));
        }

        private void Apply(ContentPartDefinition model, ContentPartDefinitionRecord record) {
            record.Settings = _settingsFormatter.Map(model.Settings).ToString();

            var toRemove = record.ContentPartFieldDefinitionRecords
                .Where(partFieldDefinitionRecord => model.Fields.All(partField => partFieldDefinitionRecord.Name != partField.Name))
                .ToList();

            foreach (var remove in toRemove) {
                record.ContentPartFieldDefinitionRecords.Remove(remove);
            }

            foreach (var field in model.Fields) {
                var fieldName = field.Name;
                var partFieldRecord = record.ContentPartFieldDefinitionRecords.SingleOrDefault(r => r.Name == fieldName);
                if (partFieldRecord == null) {
                    partFieldRecord = new ContentPartFieldDefinitionRecord {
                        ContentFieldDefinitionRecord = Acquire(field.FieldDefinition),
                        Name = field.Name
                    };
                    record.ContentPartFieldDefinitionRecords.Add(partFieldRecord);
                }
                Apply(field, partFieldRecord);
            }
        }

        private void Apply(ContentPartFieldDefinition model, ContentPartFieldDefinitionRecord record) {
            record.Settings = Compose(_settingsFormatter.Map(model.Settings));
        }

        static string Compose(XElement map) {
            if (map == null)
                return null;

            return map.ToString();
        }
    }
}
