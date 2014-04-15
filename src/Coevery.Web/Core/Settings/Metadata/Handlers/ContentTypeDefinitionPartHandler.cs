using System.Collections.Generic;
using Coevery.ContentManagement.Handlers;
using Coevery.Core.Settings.Metadata.Records;
using Coevery.Data;
using JetBrains.Annotations;

namespace Coevery.Core.Settings.Metadata.Handlers {
    [UsedImplicitly]
    public class ContentTypeDefinitionPartHandler : ContentHandler {
        private readonly IRepository<ContentTypePartDefinitionRecord> _typePartDefinitionRepository;

        public ContentTypeDefinitionPartHandler(
            IRepository<ContentTypeDefinitionRecord> typeDefinitionRepository,
            IRepository<ContentTypePartDefinitionRecord> typePartDefinitionRepository) {
            _typePartDefinitionRepository = typePartDefinitionRepository;
            Filters.Add(new ActivatingFilter<ContentTypeDefinitionPart>("ContentTypeDefinition"));
            Filters.Add(StorageFilter.For(typeDefinitionRepository));

            OnVersioning<ContentTypeDefinitionPart>(OnVersioning);
        }

        private void OnVersioning(VersionContentContext context, ContentTypeDefinitionPart existing, ContentTypeDefinitionPart building) {
            var typePartDefinitionRecords = new List<ContentTypePartDefinitionRecord>();
            foreach (var existingPart in existing.Record.ContentTypePartDefinitionRecords) {
                var buildingPart = new ContentTypePartDefinitionRecord();
                _typePartDefinitionRepository.Copy(existingPart, buildingPart);
                typePartDefinitionRecords.Add(buildingPart);
            }
            building.Record.ContentTypePartDefinitionRecords = typePartDefinitionRecords;
        }
    }
}