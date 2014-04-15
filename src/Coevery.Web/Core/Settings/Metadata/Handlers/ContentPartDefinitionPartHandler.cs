using System.Collections.Generic;
using Coevery.ContentManagement.Handlers;
using Coevery.Core.Entities.Models;
using Coevery.Core.Settings.Metadata.Records;
using Coevery.Data;
using JetBrains.Annotations;

namespace Coevery.Core.Settings.Metadata.Handlers {
    [UsedImplicitly]
    public class ContentPartDefinitionPartHandler : ContentHandler {
        private readonly IRepository<ContentPartFieldDefinitionRecord> _partFieldDefinitionRepository;

        public ContentPartDefinitionPartHandler(IRepository<ContentPartDefinitionRecord> repository,
            IRepository<ContentPartFieldDefinitionRecord> partFieldDefinitionRepository) {
            _partFieldDefinitionRepository = partFieldDefinitionRepository;
            Filters.Add(new ActivatingFilter<ContentPartDefinitionPart>("ContentPartDefinition"));
            Filters.Add(StorageFilter.For(repository));

            OnVersioning<ContentPartDefinitionPart>(OnVersioning);
        }

        private void OnVersioning(VersionContentContext context, ContentPartDefinitionPart existing, ContentPartDefinitionPart building) {
            var partFieldDefinitionRecords = new List<ContentPartFieldDefinitionRecord>();
            foreach (var existingPartField in existing.Record.ContentPartFieldDefinitionRecords) {
                var buildingPartField = new ContentPartFieldDefinitionRecord();
                _partFieldDefinitionRepository.Copy(existingPartField, buildingPartField);
                partFieldDefinitionRecords.Add(buildingPartField);
            }
            building.Record.ContentPartFieldDefinitionRecords = partFieldDefinitionRecords;
        }
    }
}
