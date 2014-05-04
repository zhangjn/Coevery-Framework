using System.Collections.Generic;
using Coevery.ContentManagement.Handlers;
using Coevery.ContentManagement.MetaData.Services;
using Coevery.Core.Settings.Metadata.Parts;
using Coevery.Core.Settings.Metadata.Records;
using Coevery.Data;
using JetBrains.Annotations;

namespace Coevery.Core.Settings.Metadata.Handlers {
    [UsedImplicitly]
    public class ContentPartDefinitionPartHandler : ContentHandler {
        private readonly IRepository<ContentPartFieldDefinitionRecord> _partFieldDefinitionRepository;
        private readonly ISettingsFormatter _settingsFormatter;

        public ContentPartDefinitionPartHandler(
            IRepository<ContentPartDefinitionRecord> repository,
            IRepository<ContentPartFieldDefinitionRecord> partFieldDefinitionRepository,
            ISettingsFormatter settingsFormatter) {
            _partFieldDefinitionRepository = partFieldDefinitionRepository;
            _settingsFormatter = settingsFormatter;
            Filters.Add(new ActivatingFilter<ContentPartDefinitionPart>("ContentPartDefinition"));
            Filters.Add(StorageFilter.For(repository));

            OnActivated<ContentPartDefinitionPart>((context, part) => LazyLoadHandlers(part));
            OnVersioning<ContentPartDefinitionPart>(OnVersioning);
        }

        private void LazyLoadHandlers(ContentPartDefinitionPart part) {
            part._fieldDefinitions.Loader(() => {
                var fieldDefinitions = new List<FieldDefinition>();
                foreach (var record in part.ContentPartFieldDefinitionRecords) {
                    var settings = _settingsFormatter.Parse(record.Settings);
                    fieldDefinitions.Add(new FieldDefinition {
                        Name = record.Name,
                        DisplayName = settings["DisplayName"],
                        FieldType = record.ContentFieldDefinitionRecord.Name,
                        Settings = settings
                    });
                }

                return fieldDefinitions;
            });
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