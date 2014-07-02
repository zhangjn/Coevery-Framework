using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Coevery.ContentManagement;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.ContentManagement.ViewModels;
using Coevery.Core.Fields.Settings;
using Coevery.Core.Relationships.Records;
using Coevery.Core.Relationships.Settings;
using Coevery.Core.Relationships.ViewModels;
using Coevery.Data;
using Coevery.DeveloperTools.EntityManagement.Services;
using Coevery.DeveloperTools.RelationshipManagement.Services;
using Coevery.Localization;

namespace Coevery.DeveloperTools.RelationshipManagement.Settings {
    public class ReferenceFieldEditorEvents : FieldEditorEvents {
        private readonly IRelationshipService _relationshipService;
        private readonly IRepository<OneToManyRelationshipRecord> _repository;
        private readonly IEntityMetadataService _entityMetadataService;

        public Localizer T { get; set; }

        public ReferenceFieldEditorEvents(
            IRelationshipService relationshipService,
            IRepository<OneToManyRelationshipRecord> repository,
            IEntityMetadataService entityMetadataService) {
            _relationshipService = relationshipService;
            _repository = repository;
            _entityMetadataService = entityMetadataService;
            T = NullLocalizer.Instance;
        }

        public override IEnumerable<TemplateViewModel> FieldTypeDescriptor() {
            var model = string.Empty;
            yield return DisplayTemplate(model, "Reference", null);
        }

        public override void UpdateFieldSettings(string fieldType, string fieldName, SettingsDictionary settingsDictionary, IUpdateModel updateModel) {
            if (fieldType != "ReferenceField") {
                return;
            }

            var model = new ReferenceFieldSettings();
            if (updateModel.TryUpdateModel(model, "ReferenceFieldSettings", null, null)) {
                if (string.IsNullOrEmpty(model.ContentTypeName)) {
                    throw new Exception("primary entity is null", new ArgumentNullException());
                }
                if (model.QueryId <= 0) {
                    //model.QueryId = _relationshipService.CreateEntityQuery(model.ContentTypeName);
                    model.QueryId = 1;
                }

                if (model.RelationshipId <= 0) {
                    var entityName = settingsDictionary["EntityName"];
                    model.RelationshipId = _relationshipService.CreateOneToManyRelationship(fieldName, model.RelationshipName, model.ContentTypeName, entityName);
                }

                UpdateSettings(model, settingsDictionary, "ReferenceFieldSettings");
                settingsDictionary["ReferenceFieldSettings.ContentTypeName"] = model.ContentTypeName;
                settingsDictionary["ReferenceFieldSettings.RelationshipName"] = model.RelationshipName;
                settingsDictionary["ReferenceFieldSettings.DisplayAsLink"] = model.DisplayAsLink.ToString();
                settingsDictionary["ReferenceFieldSettings.QueryId"] = model.QueryId.ToString(CultureInfo.InvariantCulture);
                settingsDictionary["ReferenceFieldSettings.RelationshipId"] = model.RelationshipId.ToString(CultureInfo.InvariantCulture);
                settingsDictionary["ReferenceFieldSettings.IsUnique"] = model.IsUnique.ToString();
                settingsDictionary["ReferenceFieldSettings.DisplayFieldName"] = model.DisplayFieldName;
                settingsDictionary["ReferenceFieldSettings.DeleteAction"] = model.DeleteAction.ToString();
            }
        }

        public override void FieldDeleted(string fieldType, string fieldName, SettingsDictionary settingsDictionary) {
            if (fieldType != "ReferenceField") {
                return;
            }
            //var relationshipName = settingsDictionary["ReferenceFieldSettings.RelationshipName"];
            //var record = _repository.Table
            //    .FirstOrDefault(x => x.Relationship.Name == relationshipName
            //                         && x.Relationship.RelatedEntity.ContentItemVersionRecord.Latest);
            //if (record != null) {
            //    _relationshipService.DeleteRelationship(record);
            //}
        }

        public override IEnumerable<TemplateViewModel> PartFieldEditor(ContentPartFieldDefinition definition) {
            if (definition.FieldDefinition.Name == "ReferenceField" ||
                definition.FieldDefinition.Name == "ReferenceFieldCreate") {
                var entities = _entityMetadataService.GetEntities().OrderBy(x => x.Name);
                var model = definition.Settings.GetModel<ReferenceFieldSettings>();

                model.Entities = entities.Select(item => new EntityViewModel {
                    Name = item.Name,
                    Fields = _entityMetadataService.GetFields(item.Name).Where(x => x.FieldType == "TextField").Select(x => x.Name).ToList(),
                    Selected = item.Name == model.ContentTypeName
                }).ToList();

                yield return DefinitionTemplate(model);
            }
        }
    }
}