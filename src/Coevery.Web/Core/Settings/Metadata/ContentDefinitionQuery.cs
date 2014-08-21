using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Coevery.Caching;
using Coevery.ContentManagement.MetaData;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.ContentManagement.MetaData.Services;
using Coevery.Core.Settings.Metadata.Records;
using Coevery.Data;
using Coevery.Logging;

namespace Coevery.Core.Settings.Metadata {
    public class ContentDefinitionQuery : Component, IContentDefinitionQuery {
        public const string ContentDefinitionSignal = "ContentDefinitionManager";
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;
        private readonly IRepository<ContentTypeDefinitionRecord> _typeDefinitionRepository;
        private readonly IRepository<ContentPartDefinitionRecord> _partDefinitionRepository;
        private readonly IRepository<ContentFieldDefinitionRecord> _fieldDefinitionRepository;
        private readonly ISettingsFormatter _settingsFormatter;

        public ContentDefinitionQuery(
            ICacheManager cacheManager,
            ISignals signals,
            IRepository<ContentTypeDefinitionRecord> typeDefinitionRepository,
            IRepository<ContentPartDefinitionRecord> partDefinitionRepository,
            IRepository<ContentFieldDefinitionRecord> fieldDefinitionRepository,
            ISettingsFormatter settingsFormatter) {
            _cacheManager = cacheManager;
            _signals = signals;
            _typeDefinitionRepository = typeDefinitionRepository;
            _partDefinitionRepository = partDefinitionRepository;
            _fieldDefinitionRepository = fieldDefinitionRepository;
            _settingsFormatter = settingsFormatter;
        }

        public ContentTypeDefinition GetTypeDefinition(string name) {
            if (String.IsNullOrWhiteSpace(name)) {
                return null;
            }

            var contentTypeDefinitions = AcquireContentTypeDefinitions();
            if (contentTypeDefinitions.ContainsKey(name)) {
                return contentTypeDefinitions[name];
            }

            return null;
        }

        public ContentPartDefinition GetPartDefinition(string name) {
            if (String.IsNullOrWhiteSpace(name)) {
                return null;
            }

            var contentPartDefinitions = AcquireContentPartDefinitions();
            if (contentPartDefinitions.ContainsKey(name)) {
                return contentPartDefinitions[name];
            }

            return null;
        }

        public IEnumerable<ContentTypeDefinition> ListTypeDefinitions() {
            return AcquireContentTypeDefinitions().Values;
        }

        public IEnumerable<ContentPartDefinition> ListPartDefinitions() {
            return AcquireContentPartDefinitions().Values;
        }

        public IEnumerable<ContentFieldDefinition> ListFieldDefinitions() {
            return AcquireContentFieldDefinitions().Values;
        }

        private void MonitorContentDefinitionSignal(AcquireContext<string> ctx) {
            ctx.Monitor(_signals.When(ContentDefinitionSignal));
        }

        private IDictionary<string, ContentTypeDefinition> AcquireContentTypeDefinitions() {
            return _cacheManager.Get("ContentTypeDefinitions", ctx => {
                MonitorContentDefinitionSignal(ctx);

                AcquireContentPartDefinitions();
                
                var contentTypeDefinitionRecords = _typeDefinitionRepository.Table
                    .FetchMany(x => x.ContentTypePartDefinitionRecords)
                    .ThenFetch(x => x.ContentPartDefinitionRecord)
                    .Where(t=>t.ContentItemVersionRecord.Published)
                    .Select(Build);

                return contentTypeDefinitionRecords.ToDictionary(x => x.Name, y => y, StringComparer.OrdinalIgnoreCase);
            });
        }

        private IDictionary<string, ContentPartDefinition> AcquireContentPartDefinitions() {
            return _cacheManager.Get("ContentPartDefinitions", ctx => {
                MonitorContentDefinitionSignal(ctx);

                var contentPartDefinitionRecords = _partDefinitionRepository.Table
                    .FetchMany(x => x.ContentPartFieldDefinitionRecords)
                    .ThenFetch(x => x.ContentFieldDefinitionRecord)
                    .Where(x=>x.ContentItemVersionRecord.Published)
                    .Select(Build);

                return contentPartDefinitionRecords.ToDictionary(x => x.Name, y => y, StringComparer.OrdinalIgnoreCase);
            });
        }

        private IDictionary<string, ContentFieldDefinition> AcquireContentFieldDefinitions() {
            return _cacheManager.Get("ContentFieldDefinitions", ctx => {
                MonitorContentDefinitionSignal(ctx);

                return _fieldDefinitionRepository.Table.Select(Build).ToDictionary(x => x.Name, y => y);
            });
        }

        private ContentTypeDefinition Build(ContentTypeDefinitionRecord source) {
            var settings = _settingsFormatter.Map(Parse(source.Settings));
            return new ContentTypeDefinition(
                source.Name,
                source.DisplayName,
                source.ContentTypePartDefinitionRecords.Select(Build),
                settings);
        }

        private ContentTypePartDefinition Build(ContentTypePartDefinitionRecord source) {
            return new ContentTypePartDefinition(
                Build(source.ContentPartDefinitionRecord),
                _settingsFormatter.Map(Parse(source.Settings)));
        }

        private ContentPartDefinition Build(ContentPartDefinitionRecord source) {
            return new ContentPartDefinition(
                source.Name,
                source.ContentPartFieldDefinitionRecords.Select(Build),
                _settingsFormatter.Map(Parse(source.Settings)));
        }

        private ContentPartFieldDefinition Build(ContentPartFieldDefinitionRecord source) {
            return new ContentPartFieldDefinition(
                Build(source.ContentFieldDefinitionRecord),
                source.Name,
                _settingsFormatter.Map(Parse(source.Settings)));
        }

        private ContentFieldDefinition Build(ContentFieldDefinitionRecord source) {
            return new ContentFieldDefinition(source.Name);
        }

        private XElement Parse(string settings) {
            if (string.IsNullOrEmpty(settings))
                return null;

            try {
                return XElement.Parse(settings);
            }
            catch (Exception ex) {
                Logger.Error(ex, "Unable to parse settings xml");
                return null;
            }
        }
    }
}