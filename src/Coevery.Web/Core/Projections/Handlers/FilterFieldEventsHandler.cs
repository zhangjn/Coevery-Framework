using System.Linq;
using Coevery.Core.Common.Extensions;
using Coevery.Core.Entities.Events;
using Coevery.Core.Projections.Models;
using Coevery.Data;

namespace Coevery.Core.Projections.Handlers {
    public class FilterFieldEventsHandler : IFieldEvents {
        private readonly IRepository<EntityFilterRecord> _entityFilterRepository;

        public FilterFieldEventsHandler(IRepository<EntityFilterRecord> entityFilterRepository) {
            _entityFilterRepository = entityFilterRepository;
        }

        public void OnDeleting(string entityName, string fieldName) {
            var entityFilterRecords = _entityFilterRepository.Table.Where(x => x.EntityName == entityName).ToList();
            foreach (var entityFilterRecord in entityFilterRecords) {
                string type = string.Format("{0}.{1}.", entityName.ToPartName(), fieldName);
                var filters = entityFilterRecord.FilterGroupRecord.Filters.Where(x => x.Type == type).ToList();
                filters.ForEach(record => entityFilterRecord.FilterGroupRecord.Filters.Remove(record));
                if (entityFilterRecord.FilterGroupRecord.Filters.Count == 0) {
                    _entityFilterRepository.Delete(entityFilterRecord);
                }
                else {
                    _entityFilterRepository.Update(entityFilterRecord);
                }
            }
        }
    }
}