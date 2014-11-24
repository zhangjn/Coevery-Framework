using Coevery.ContentManagement.Handlers;
using Coevery.Data;
using Coevery.PropertyManagement.Models;

namespace Coevery.PropertyManagement.Handlers {
    public class ChargeItemSettingPartHandler : ContentHandler {
        public ChargeItemSettingPartHandler(IRepository<ChargeItemSettingPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(DeleteRecordFilter.For(repository));
        }
    }
}