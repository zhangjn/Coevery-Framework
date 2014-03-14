﻿using Coevery.Core.Entities.Events;
using Coevery.DeveloperTools.RelationshipManagement.Services;

namespace Coevery.DeveloperTools.RelationshipManagement.Handlers {
    public class RelationshipEntityEventsHandler : IEntityEvents {
        private readonly IRelationshipService _relationshipService;

        public RelationshipEntityEventsHandler(IRelationshipService relationshipService) {
            _relationshipService = relationshipService;
        }

        public void OnCreated(string entityName) {}
        public void OnUpdating(string entityName) {}

        public void OnDeleting(string entityName) {
            var relationships = _relationshipService.GetRelationships(entityName);
            if (relationships == null) {
                return;
            }
            foreach (var relationship in relationships) {
                _relationshipService.DeleteRelationship(relationship);
            }
        }
    }
}