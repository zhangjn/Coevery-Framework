using System.Linq;
using System.Web.Mvc;
using Coevery.ContentManagement;
using Coevery.ContentManagement.MetaData;
using Coevery.Core.Common.Extensions;
using Coevery.Core.Entities.Models;
using Coevery.Core.Relationships.Records;
using Coevery.Core.Relationships.Settings;
using Coevery.Data;
using Coevery.Utility.Extensions;

namespace Coevery.Core.Relationships.Services {
    public class RelationshipService : IRelationshipService {
        #region Class definition

        private readonly IRepository<RelationshipRecord> _relationshipRepository;
        private readonly IRepository<OneToManyRelationshipRecord> _oneToManyRepository;
        private readonly IRepository<ManyToManyRelationshipRecord> _manyToManyRepository;

        private readonly IContentDefinitionExtension _contentDefinitionExtension;
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IContentManager _contentManager;

        public RelationshipService(
            IRepository<RelationshipRecord> relationshipRepository,
            IRepository<OneToManyRelationshipRecord> oneToManyRepository,
            IRepository<ManyToManyRelationshipRecord> manyToManyRepository,
            IContentDefinitionExtension contentDefinitionExtension,
            IContentDefinitionManager contentDefinitionManager,
            IContentManager contentManager) {
            _relationshipRepository = relationshipRepository;
            _oneToManyRepository = oneToManyRepository;
            _manyToManyRepository = manyToManyRepository;
            _contentDefinitionManager = contentDefinitionManager;
            _contentManager = contentManager;
            _contentDefinitionExtension = contentDefinitionExtension;
        }

        #endregion

        #region GetMethods

        public string CheckRelationName(string name) {
            string errorMessage = null;
            if (!string.Equals(name, name.ToSafeName())) {
                errorMessage += "The name:\"" + name + "\" is not legal!\n";
            }
            if (_relationshipRepository.Fetch(relation => relation.Name == name).FirstOrDefault() != null) {
                errorMessage += "The name:\"" + name + "\" already exists!\n";
            }
            return errorMessage;
        }

        public string GetReferenceField(string entityName, string relationName) {
            var reference = _contentDefinitionManager
                .GetPartDefinition(entityName.ToPartName())
                .Fields.FirstOrDefault(field => field.FieldDefinition.Name == "ReferenceField"
                                                && field.Settings.TryGetModel<ReferenceFieldSettings>().RelationshipName == relationName);
            return reference == null ? null : reference.Name;
        }

        public SelectListItem[] GetFieldNames(string entityName) {
            var entity = _contentDefinitionManager.GetPartDefinition(entityName.ToPartName());
            return entity == null
                ? null
                : (from field in entity.Fields
                    select new SelectListItem {
                        Value = field.Name,
                        Text = field.DisplayName,
                        Selected = false
                    }).ToArray();
        }

        public SelectListItem[] GetEntityNames(string excludeEntity) {
            var entities = _contentDefinitionExtension.ListUserDefinedTypeDefinitions();
            return entities == null
                ? null
                : (from entity in entities
                    where entity.Name != excludeEntity
                    select new SelectListItem {
                        Value = entity.Name,
                        Text = entity.DisplayName,
                        Selected = false
                    }).ToArray();
        }

        public OneToManyRelationshipRecord GetOneToMany(int id) {
            return _oneToManyRepository.Get(record => record.Relationship.Id == id);
        }

        public ManyToManyRelationshipRecord GetManyToMany(int id) {
            return _manyToManyRepository.Get(record => record.Relationship.Id == id);
        }

        public RelationshipRecord[] GetRelationships(string entityName) {
            var entity = _contentManager
                .Query<EntityMetadataPart>(VersionOptions.Latest, "EntityMetadata")
                .List().FirstOrDefault(x => x.Name == entityName);

            if (entity == null) {
                return null;
            }
            return (from record in _relationshipRepository.Table
                where record.PrimaryEntity.ContentItemVersionRecord.Latest
                      && record.RelatedEntity.ContentItemVersionRecord.Latest
                      && (record.PrimaryEntity == entity.Record || record.RelatedEntity == entity.Record)
                select record).ToArray();
        }

        #endregion
    }
}