using System.Linq;
using Coevery.ContentManagement;
using Coevery.ContentManagement.MetaData;
using Coevery.Core.Common.Extensions;
using Coevery.Core.Relationships.Records;
using Coevery.Core.Relationships.Settings;
using Coevery.Data;

namespace Coevery.Core.Relationships.Services {
    public class RelationshipService : IRelationshipService {
        private readonly IRepository<RelationshipRecord> _relationshipRepository;
        private readonly IRepository<OneToManyRelationshipRecord> _oneToManyRepository;
        private readonly IRepository<ManyToManyRelationshipRecord> _manyToManyRepository;

        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IContentManager _contentManager;

        public RelationshipService(
            IRepository<RelationshipRecord> relationshipRepository,
            IRepository<OneToManyRelationshipRecord> oneToManyRepository,
            IRepository<ManyToManyRelationshipRecord> manyToManyRepository,
            IContentDefinitionManager contentDefinitionManager,
            IContentManager contentManager) {
            _relationshipRepository = relationshipRepository;
            _oneToManyRepository = oneToManyRepository;
            _manyToManyRepository = manyToManyRepository;
            _contentDefinitionManager = contentDefinitionManager;
            _contentManager = contentManager;
        }

        public string GetReferenceField(string entityName, string relationName) {
            var reference = _contentDefinitionManager
                .GetPartDefinition(entityName.ToPartName())
                .Fields.FirstOrDefault(field => field.FieldDefinition.Name == "ReferenceField"
                                                && field.Settings.TryGetModel<ReferenceFieldSettings>().RelationshipName == relationName);
            return reference == null ? null : reference.Name;
        }


        public OneToManyRelationshipRecord GetOneToMany(int id) {
            return _oneToManyRepository.Get(record => record.Relationship.Id == id);
        }

        public ManyToManyRelationshipRecord GetManyToMany(int id) {
            return _manyToManyRepository.Get(record => record.Relationship.Id == id);
        }

        public RelationshipRecord[] GetRelationships(string entityName) {
            //var entity = _contentManager
            //    .Query<EntityMetadataPart>(VersionOptions.Latest, "EntityMetadata")
            //    .List().FirstOrDefault(x => x.Name == entityName);

            //if (entity == null) {
            //    return null;
            //}
            //return (from record in _relationshipRepository.Table
            //    where record.PrimaryEntity.ContentItemVersionRecord.Latest
            //          && record.RelatedEntity.ContentItemVersionRecord.Latest
            //          && (record.PrimaryEntity == entity.Record || record.RelatedEntity == entity.Record)
            //    select record).ToArray();

            return null;
        }
    }
}