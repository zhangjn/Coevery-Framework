﻿using System.Collections.Generic;
using System.Linq;
using Coevery.ContentManagement;
using Coevery.ContentManagement.Handlers;
using Coevery.ContentManagement.MetaData;
using Coevery.Core.OptionSet.Fields;
using Coevery.Core.OptionSet.Models;
using Coevery.Data;

namespace Coevery.Core.OptionSet.Handlers {
    public class OptionItemContainerPartHandler : ContentHandler {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IContentManager _contentManager;

        public OptionItemContainerPartHandler(
            IContentDefinitionManager contentDefinitionManager,
            IRepository<OptionItemContainerPartRecord> repository,
            IContentManager contentManager) {
            _contentDefinitionManager = contentDefinitionManager;
            _contentManager = contentManager;

            Filters.Add(StorageFilter.For(repository));

            // Tells how to load the field terms on demand, when a content item it loaded or when it has been created
            OnLoaded<OptionItemContainerPart>((context, part) => InitializerTermsLoader(part));
            OnCreated<OptionItemContainerPart>((context, part) => InitializerTermsLoader(part));

            OnIndexing<OptionItemContainerPart>(
                (context, part) => {
                    foreach (var term in part.OptionItems) {
                        var optionContentItem = context.ContentManager.Get<OptionItemPart>(term.OptionItemRecord.Id);
                        context.DocumentIndex.Add(term.Field, optionContentItem.Name).Analyze();
                        context.DocumentIndex.Add(term.Field + "-id", optionContentItem.Id).Store();
                    }
                });

            OnVersioning<OptionItemContainerPart>(OnVersioning);
        }

        private void OnVersioning(VersionContentContext context, OptionItemContainerPart existing, OptionItemContainerPart building) {
            building.Record.OptionItems = new List<OptionItemContentItem>();
        }

        private void InitializerTermsLoader(OptionItemContainerPart part) {
            foreach (var field in part.ContentItem.Parts.SelectMany(p => p.Fields).OfType<OptionSetField>()) {
                var tempField = field.Name;
                var fieldTermRecordIds = part.Record.OptionItems.Where(t => t.Field == tempField).Select(tci => tci.OptionItemRecord.Id);
                field.OptionItemsField.Loader(value => fieldTermRecordIds.Select(id => _contentManager.Get<OptionItemPart>(id)).ToList());
            }

            part._optionItemParts.Loader(value =>
                part.OptionItems.Select(
                    x => new OptionItemContentItemPart {Field = x.Field, OptionItemPart = _contentManager.Get<OptionItemPart>(x.OptionItemRecord.Id)}
                    ));
        }

        protected override void Activating(ActivatingContentContext context) {
            base.Activating(context);

            // weld the TermsPart dynamically, if a field has been assigned to one of its parts
            var contentTypeDefinition = _contentDefinitionManager.GetTypeDefinition(context.ContentType);
            if (contentTypeDefinition == null) {
                return;
            }

            if (contentTypeDefinition.Parts.Any(
                part => part.PartDefinition.Fields.Any(
                    field => field.FieldDefinition.Name == typeof(OptionSetField).Name))) {
                context.Builder.Weld<OptionItemContainerPart>();
            }
        }
    }
}