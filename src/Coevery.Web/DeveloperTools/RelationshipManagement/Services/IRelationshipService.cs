﻿using System.Web.Mvc;
using Coevery.Core.Relationships.Models;
using Coevery.Core.Relationships.Records;

namespace Coevery.DeveloperTools.RelationshipManagement.Services {
    public interface IRelationshipService : IDependency {
        string CheckRelationName(string name);
        SelectListItem[] GetEntityNames(string excludeEntity);
        SelectListItem[] GetFieldNames(string entityName);
        RelationshipRecord[] GetRelationships(string entityName);
        OneToManyRelationshipRecord GetOneToMany(int id);
        ManyToManyRelationshipRecord GetManyToMany(int id);
        string GetReferenceField(string entityName, string relationName);

        int CreateEntityQuery(string entityName);
        int CreateOneToManyRelationship(string fieldName, string relationName, string primaryEntityName, string relatedEntityName);
        string CreateRelationship(OneToManyRelationshipModel oneToMany);
        string CreateRelationship(ManyToManyRelationshipModel manyToMany);
        string EditRelationship(int relationshipId, ManyToManyRelationshipModel manyToMany);
        string EditRelationship(int relationshipId, OneToManyRelationshipModel oneToMany);
        void DeleteRelationship(RelationshipRecord relationship);
        void DeleteRelationship(OneToManyRelationshipRecord record);
    }
}