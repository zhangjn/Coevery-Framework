using System;
using Coevery.ContentManagement.Handlers;
using Coevery.Data;
using Coevery.Localization;
using Coevery.PropertyManagement.Models;
using Coevery.UI.Notify;
using Coevery.Users.Models;

namespace Coevery.PropertyManagement.Handlers {
    public class HousePartHandler : ContentHandler {
        private readonly IRepository<ContractHouseRecord> _contractHouseRepository;
        private readonly ICoeveryServices _coeveryServices;
        public HousePartHandler(IRepository<HousePartRecord> repository,
            IRepository<CustomerPartRecord> customerRepository,
            IRepository<UserPartRecord> userRepository,
            IRepository<ApartmentPartRecord> appartmentRepository,
            IRepository<BuildingPartRecord> buildingRepository, 
            ICoeveryServices coeveryServices, 
            IRepository<ContractHouseRecord> contractHouseRepository) {
            _coeveryServices = coeveryServices;
            _contractHouseRepository = contractHouseRepository;
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(DeleteRecordFilter.For(repository));

            OnInitialized<HousePart>(
                (context, part) => {
                    part._ownerIdField.Setter(value => {
                        part.Owner = (value != null ? customerRepository.Get(value.Value) : null);
                        return value;
                    });
                    part._ownerIdField.Loader(value => part.Owner != null ? part.Owner.Id : (int?) null);
                });

            OnInitialized<HousePart>(
                (context, part) => {
                    part._officerIdField.Setter(value => {
                        part.Officer = (value != null ? userRepository.Get(value.Value) : null);
                        return value;
                    });
                    part._officerIdField.Loader(value => part.Officer != null ? part.Officer.Id : (int?) null);
                });

            OnInitialized<HousePart>(
                (context, part) => {
                    part._buildingIdField.Setter(value => {
                        part.Building = (value != null ? buildingRepository.Get(value.Value) : null);
                        return value;
                    });
                    part._buildingIdField.Loader(value => part.Building != null ? part.Building.Id : (int?) null);
                });

            OnInitialized<HousePart>(
                (context, part) => {
                    part._apartmentIdField.Setter(value => {
                        part.Apartment = (value != null ? appartmentRepository.Get(value.Value) : null);
                        return value;
                    });
                    part._apartmentIdField.Loader(value => part.Apartment != null ? part.Apartment.Id : (int?) null);
                });
        }
        public Localizer T { get; set; }
        protected override void Removing(RemoveContentContext context)
        {
            base.Removing(context);
            if (context.Cancel) return;
            var count = _contractHouseRepository.Count(x => x.House.Id == context.Id&&x.Contract.EndDate>DateTime.Now);
            if (count > 0)
            {
                _coeveryServices.Notifier.Error(T("无法删除这条数据因为它已经被其他的数据使用！"));
                context.Cancel = true;
            }
        }
    }
}