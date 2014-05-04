using System.Collections.Generic;
using System.Linq;
using Coevery.ContentManagement;
using Coevery.ContentManagement.MetaData;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.ContentManagement.MetaData.Services;
using Coevery.Core.Common.Extensions;
using Coevery.Core.Entities.Events;
using Coevery.Core.Settings.Metadata.Parts;
using Coevery.Core.Settings.Metadata.Records;
using Coevery.Data;
using Coevery.Localization;

namespace Coevery.DeveloperTools.EntityManagement.Services {
    public interface IEntityMetadataService : IDependency {
        void CreateEntity(string name, string displayName, SettingsDictionary settings);
        bool DeleteEntity(string name);
        IEnumerable<ContentTypeDefinitionPart> GetEntities();
        IEnumerable<ContentTypeDefinitionPart> GetEntities(VersionOptions options);
        ContentTypeDefinitionPart GetEntity(string name);
        ContentTypeDefinitionPart GetEntity(string name, VersionOptions options);

        IEnumerable<FieldDefinition> GetFields(string entityName);
        void AddField(string entityName, string fieldName, string fieldDisplayName, string fieldType, bool addInLayout, IUpdateModel updateModel);
        void UpdateField(string entityName, string fieldName, string displayName, IUpdateModel updateModel);
        bool DeleteField(string filedName, string entityName);
    }

    public class EntityMetadataService : IEntityMetadataService {
        private readonly ISettingsFormatter _settingsFormatter;
        private readonly IRepository<ContentFieldDefinitionRecord> _fieldDefinitionRepository;
        private readonly IContentDefinitionEditorEvents _contentDefinitionEditorEvents;
        private readonly IFieldEvents _fieldEvents;

        public EntityMetadataService(
            ICoeveryServices services,
            IRepository<ContentFieldDefinitionRecord> fieldDefinitionRepository,
            IContentDefinitionEditorEvents contentDefinitionEditorEvents,
            ISettingsFormatter settingsFormatter,
            IFieldEvents fieldEvents) {
            _fieldDefinitionRepository = fieldDefinitionRepository;
            _contentDefinitionEditorEvents = contentDefinitionEditorEvents;
            _settingsFormatter = settingsFormatter;
            _fieldEvents = fieldEvents;
            Services = services;
            T = NullLocalizer.Instance;
        }

        public ICoeveryServices Services { get; private set; }
        public Localizer T { get; set; }

        #region Entity Methods

        public IEnumerable<ContentTypeDefinitionPart> GetEntities() {
            return GetEntities(VersionOptions.Latest);
        }

        public IEnumerable<ContentTypeDefinitionPart> GetEntities(VersionOptions options) {
            return Services.ContentManager.Query<ContentTypeDefinitionPart, ContentTypeDefinitionRecord>(options)
                .Where(x => x.Customized)
                .List();
        }

        public ContentTypeDefinitionPart GetEntity(string name) {
            return GetEntity(name, VersionOptions.Latest);
        }

        public ContentTypeDefinitionPart GetEntity(string name, VersionOptions options) {
            return Services.ContentManager.Query<ContentTypeDefinitionPart, ContentTypeDefinitionRecord>(options)
                .Where(x => x.Name == name)
                .List()
                .FirstOrDefault();
        }

        public void CreateEntity(string name, string displayName, SettingsDictionary settings) {
            var entityDraft = Services.ContentManager.New<ContentTypeDefinitionPart>("ContentTypeDefinition");
            entityDraft.DisplayName = displayName;
            entityDraft.Name = name;
            entityDraft.EntitySettings = settings;
            entityDraft.Customized = true;
            Services.ContentManager.Create(entityDraft, VersionOptions.Draft);
        }

        public bool DeleteEntity(string name) {
            var entity = GetEntity(name);
            if (entity == null) {
                return false;
            }

            Services.ContentManager.Remove(entity.ContentItem);
            return true;
        }

        #endregion

        # region Field Methods

        public IEnumerable<FieldDefinition> GetFields(string entityName) {
            var part = GetPartDefinition(entityName);
            if (part == null) {
                return Enumerable.Empty<FieldDefinition>();
            }
            return part.FieldDefinitions;
        }

        public void AddField(string entityName, string fieldName, string fieldDisplayName, string fieldType, bool addInLayout, IUpdateModel updateModel) {
            var entity = GetEntity(entityName, VersionOptions.DraftRequired);
            if (entity == null) {
                updateModel.AddModelError("Entity", T("The entity with name \"{0}\" doesn't exist!", entityName));
                return;
            }

            var partDefinition = GetPartDefinition(entityName);
            bool exist = partDefinition.ContentPartFieldDefinitionRecords.Any(x => x.Name == fieldName);
            if (exist) {
                updateModel.AddModelError("Name", T("A field with the same name already exists."));
                return;
            }

            var fieldRecord = new ContentPartFieldDefinitionRecord {
                Name = fieldName,
                ContentFieldDefinitionRecord = FetchFieldDefinition(fieldType)
            };
            var settingsDictionary = new SettingsDictionary();
            settingsDictionary["DisplayName"] = fieldDisplayName;
            settingsDictionary["EntityName"] = entityName;
            settingsDictionary["Storage"] = "Part";
            _contentDefinitionEditorEvents.UpdateFieldSettings(fieldType, fieldName, settingsDictionary, updateModel);
            fieldRecord.Settings = _settingsFormatter.Parse(settingsDictionary);
            partDefinition.ContentPartFieldDefinitionRecords.Add(fieldRecord);

            if (addInLayout) {
                var settings = entity.EntitySettings;
                _fieldEvents.OnAdding(entityName, fieldName, settings);
                entity.EntitySettings = settings;
            }
        }

        public void UpdateField(string entityName, string fieldName, string displayName, IUpdateModel updateModel) {
            var entity = GetEntity(entityName, VersionOptions.DraftRequired);
            if (entity == null) {
                updateModel.AddModelError("Entity", T("The entity with name {0} doesn't exist!", entityName));
                return;
            }

            var partDefinition = GetPartDefinition(entityName);
            var fieldRecord = partDefinition.ContentPartFieldDefinitionRecords.FirstOrDefault(x => x.Name == fieldName);
            if (fieldRecord == null) {
                updateModel.AddModelError("Field", T("The field doesn't exists."));
                return;
            }
            var settingsDictionary = _settingsFormatter.Parse(fieldRecord.Settings);
            settingsDictionary["DisplayName"] = displayName;
            _contentDefinitionEditorEvents.UpdateFieldSettings(fieldRecord.ContentFieldDefinitionRecord.Name, fieldName, settingsDictionary, updateModel);
            fieldRecord.Settings = _settingsFormatter.Parse(settingsDictionary);
        }

        public bool DeleteField(string fieldName, string entityName) {
            var entity = GetEntity(entityName, VersionOptions.DraftRequired);
            if (entity == null) {
                return false;
            }

            var partDefinition = GetPartDefinition(entityName);
            var fieldRecord = partDefinition.ContentPartFieldDefinitionRecords.FirstOrDefault(x => x.Name == fieldName);
            if (fieldRecord == null) {
                return false;
            }

            _fieldEvents.OnDeleting(entityName, fieldName);
            _contentDefinitionEditorEvents.FieldDeleted(fieldRecord.ContentFieldDefinitionRecord.Name, fieldRecord.Name, _settingsFormatter.Parse(fieldRecord.Settings));
            partDefinition.ContentPartFieldDefinitionRecords.Remove(fieldRecord);
            return true;
        }

        # endregion

        #region Private Methods

        private ContentPartDefinitionPart GetPartDefinition(string entityName) {
            var partName = entityName.ToPartName();
            return Services.ContentManager.Query<ContentPartDefinitionPart, ContentPartDefinitionRecord>(VersionOptions.Latest)
                .Where(x => x.Name == partName)
                .List()
                .FirstOrDefault();
        }

        private ContentFieldDefinitionRecord FetchFieldDefinition(string fieldType) {
            var baseFieldDefinition = _fieldDefinitionRepository.Get(def => def.Name == fieldType);
            if (baseFieldDefinition == null) {
                baseFieldDefinition = new ContentFieldDefinitionRecord {Name = fieldType};
                _fieldDefinitionRepository.Create(baseFieldDefinition);
            }
            return baseFieldDefinition;
        }

        #endregion
    }
}