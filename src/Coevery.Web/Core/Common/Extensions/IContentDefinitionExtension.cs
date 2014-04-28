using System.Collections.Generic;
using System.Linq;
using Coevery.Caching;
using Coevery.ContentManagement.MetaData;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.Core.Entities.Models;
using Coevery.Data;

namespace Coevery.Core.Common.Extensions {
    public class EntityNames {
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string CollectionDisplayName { get; set; }
    }

    public interface IContentDefinitionExtension : IDependency {
        IEnumerable<ContentTypeDefinition> ListUserDefinedTypeDefinitions();
        IEnumerable<ContentPartDefinition> ListUserDefinedPartDefinitions();
        EntityNames GetEntityNames(string entityName);
    }

    public class ContentDefinitionExtension : IContentDefinitionExtension {
        private const string ContentDefinitionSignal = "ContentDefinitionManager";
        private readonly IContentDefinitionQuery _contentDefinitionQuery;
        private readonly IRepository<EntityMetadataRecord> _entityMetadataRepository;
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;

        public ContentDefinitionExtension(
            IRepository<EntityMetadataRecord> entityMetadataRepository,
            ICacheManager cacheManager,
            ISignals signals, 
            IContentDefinitionQuery contentDefinitionQuery) {
            _entityMetadataRepository = entityMetadataRepository;
            _cacheManager = cacheManager;
            _signals = signals;
            _contentDefinitionQuery = contentDefinitionQuery;
        }

        public IEnumerable<ContentTypeDefinition> ListUserDefinedTypeDefinitions() {
            return _cacheManager.Get("UserContentTypeDefinitions", ctx => {
                MonitorContentDefinitionSignal(ctx);

                var metaEntities = _entityMetadataRepository.Table.Select(x => x.Name).Distinct().ToList();
                if (metaEntities.Count == 0) {
                    return Enumerable.Empty<ContentTypeDefinition>();
                }

                return (from type in _contentDefinitionQuery.ListTypeDefinitions()
                    where metaEntities.Contains(type.Name)
                    select type).ToList();
            });
        }

        public IEnumerable<ContentPartDefinition> ListUserDefinedPartDefinitions() {
            return _cacheManager.Get("UserContentPartDefinitions", ctx => {
                MonitorContentDefinitionSignal(ctx);

                var metaEntities = _entityMetadataRepository.Table.Select(x => x.Name).Distinct().ToList();
                if (metaEntities.Count == 0) {
                    return Enumerable.Empty<ContentPartDefinition>();
                }

                return (from part in _contentDefinitionQuery.ListPartDefinitions()
                    where metaEntities.Contains(part.Name.RemovePartSuffix())
                    select part).ToList();
            });
        }

        public EntityNames GetEntityNames(string entityName) {
            var entity = _contentDefinitionQuery.GetTypeDefinition(entityName);
            if (entity == null) {
                return null;
            }
            var setting = entity.Settings;
            return setting.ContainsKey("CollectionDisplayName")
                ? new EntityNames {
                    Name = entity.Name,
                    DisplayName = entity.DisplayName,
                    CollectionDisplayName = setting["CollectionDisplayName"]
                }
                : null;
        }

        private void MonitorContentDefinitionSignal(AcquireContext<string> ctx) {
            ctx.Monitor(_signals.When(ContentDefinitionSignal));
        }
    }
}