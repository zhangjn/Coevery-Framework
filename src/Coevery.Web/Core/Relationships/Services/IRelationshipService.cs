using Coevery.Core.Relationships.Records;

namespace Coevery.Core.Relationships.Services {
    public interface IRelationshipService : IDependency {
        RelationshipRecord[] GetRelationships(string entityName);
        OneToManyRelationshipRecord GetOneToMany(int id);
        ManyToManyRelationshipRecord GetManyToMany(int id);
        string GetReferenceField(string entityName, string relationName);
    }
}