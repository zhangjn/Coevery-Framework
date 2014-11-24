using Coevery.ContentManagement.Handlers;
using Coevery.Data;
using Coevery.Localization;
using Coevery.PropertyManagement.Models;
using Coevery.UI.Notify;

namespace Coevery.PropertyManagement.Handlers {
    public class BuildingPartHandler : ContentHandler {
        private readonly IRepository<HousePartRecord> _houseRepository;
        private readonly ICoeveryServices _coeveryServices;
        public BuildingPartHandler(IRepository<BuildingPartRecord> repository, 
            IRepository<HousePartRecord> houseRepository, 
            ICoeveryServices coeveryServices)
        {
            _houseRepository = houseRepository;
            _coeveryServices = coeveryServices;
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(DeleteRecordFilter.For(repository));
        }

        public Localizer T { get; set; }
        protected override void Removing(RemoveContentContext context)
        {
            base.Removing(context);
            if (context.Cancel) return;
            var count = _houseRepository.Count(x => x.Building.Id == context.Id);
            if (count > 0)
            {
                _coeveryServices.Notifier.Error(T("无法删除这条数据因为它已经被其他的数据使用！"));
                context.Cancel = true;
            }
        }
    }
}