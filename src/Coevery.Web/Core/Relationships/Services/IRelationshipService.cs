using System.Web.Mvc;
using Coevery.Core.Relationships.Records;

namespace Coevery.Core.Relationships.Services {
    public interface IRelationshipService : IDependency {
        string CheckRelationName(string name);
        SelectListItem[] GetEntityNames(string excludeEntity);
        SelectListItem[] GetFieldNames(string entityName);
        RelationshipRecord[] GetRelationships(string entityName);
        OneToManyRelationshipRecord GetOneToMany(int id);
        ManyToManyRelationshipRecord GetManyToMany(int id);
        string GetReferenceField(string entityName, string relationName);
    }
}