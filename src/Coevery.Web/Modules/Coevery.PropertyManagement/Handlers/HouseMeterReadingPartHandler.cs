using Coevery.ContentManagement.Handlers;
using Coevery.Data;
using Coevery.PropertyManagement.Models;

namespace Coevery.PropertyManagement.Handlers {
    public class HouseMeterReadingPartHandler : ContentHandler {
        public HouseMeterReadingPartHandler(IRepository<HouseMeterReadingPartRecord> repository,
            IRepository<HouseMeterRecord> houseMeterTypeRepository)
        {
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(DeleteRecordFilter.For(repository));
            OnInitialized<HouseMeterReadingPart>(
              (context, part) =>
              {
                  part._houseMeterIdField.Setter(value =>
                  {
                      part.HouseMeter = (value != null ? houseMeterTypeRepository.Get(value.Value) : null);
                      return value;
                  });
                  part._houseMeterIdField.Loader(value => part.HouseMeter != null ? part.HouseMeter.Id : (int?)null);
              });
        }
    }
}