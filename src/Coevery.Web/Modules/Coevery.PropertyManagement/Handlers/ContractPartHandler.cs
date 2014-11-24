using Coevery.ContentManagement.Handlers;
using Coevery.Data;
using Coevery.Localization;
using Coevery.PropertyManagement.Models;
using Coevery.PropertyManagement.Services;
using Coevery.UI.Notify;

namespace Coevery.PropertyManagement.Handlers {
    public class ContractPartHandler : ContentHandler {
        private readonly IRepository<BillRecord> _billRepository;
        private readonly ICoeveryServices _coeveryServices;
        private readonly IRecordStatusUpdator _recordStatusUpdator;

        public ContractPartHandler(
            IRepository<ContractPartRecord> repository,
            IRepository<CustomerPartRecord> customerRepository,
            IRepository<BillRecord> billRepository,
            ICoeveryServices coeveryServices,
            IRecordStatusUpdator recordStatusUpdator) {
            _billRepository = billRepository;
            _coeveryServices = coeveryServices;
            _recordStatusUpdator = recordStatusUpdator;
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(DeleteRecordFilter.For(repository));

            OnInitialized<ContractPart>(
                (context, part) => {
                    part._RenterField.Setter(value => {
                        part.Renter = (value != null ? customerRepository.Get(value.Value) : null);
                        return value;
                    });
                    part._RenterField.Loader(value => part.Renter != null ? part.Renter.Id : (int?) null);
                });
            OnPublished<ContractPart>((context, part) => _recordStatusUpdator.UpdateRecordStatus());
        }

        public Localizer T { get; set; }

        protected override void Removing(RemoveContentContext context)
        {
            base.Removing(context);
            if (context.Cancel) return;
            var count = _billRepository.Count(x => x.Contract.Id == context.Id);
            if (count > 0)
            {
                _coeveryServices.Notifier.Error(T("无法删除这条数据因为它已经被其他的数据使用！"));
                context.Cancel = true;
            }
        }
    }
}