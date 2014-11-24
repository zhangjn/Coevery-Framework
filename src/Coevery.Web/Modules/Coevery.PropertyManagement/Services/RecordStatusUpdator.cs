using System;
using System.Collections.Generic;
using System.Linq;
using Coevery.ContentManagement;
using Coevery.Data;
using Coevery.Logging;
using Coevery.PropertyManagement.Models;
using Coevery.Services;


namespace Coevery.PropertyManagement.Services
{
    public interface IRecordStatusUpdator : IDependency {
        void UpdateRecordStatus();
    }

    public class RecordStatusUpdator : IRecordStatusUpdator {

        private readonly IClock _clock;
        private readonly IRepository<ContractPartRecord> _repository;
        private readonly ITransactionManager _transactionManager;
        public RecordStatusUpdator(IClock clock, 
            IRepository<ContractPartRecord> repository, 
            ITransactionManager transactionManager) {
            _clock = clock;
            _repository = repository;
            _transactionManager = transactionManager;
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        public void UpdateRecordStatus() {
            var expiredContracts = _repository.Fetch(x => x.EndDate <= _clock.UtcNow && x.ContractStatus != ContractStatusOption.终止)
                .ToList();
            ProcessContractStatus(expiredContracts, x => x.ContractStatus = ContractStatusOption.终止);
            expiredContracts.SelectMany(x => x.Houses).Select(x => x.House).ToList().ForEach(x => x.HouseStatus = HouseStatusOption.空置);

            var inprogressContracts = _repository.Fetch(x => x.EndDate > _clock.UtcNow
                                                             && x.BeginDate <= _clock.UtcNow
                                                             && x.ContractStatus != ContractStatusOption.终止).ToList();

            ProcessContractStatus(inprogressContracts, x => x.ContractStatus = ContractStatusOption.正在执行);
            inprogressContracts.SelectMany(x => x.Houses).ToList().ForEach(x => x.House.HouseStatus = x.Contract.HouseStatus);
        }

        private void ProcessContractStatus(IEnumerable<ContractPartRecord> expiredContracts, Action<ContractPartRecord> action) {
            foreach (var contract in expiredContracts) {
                _transactionManager.RequireNew();

                try {
                    action(contract);
                }
                catch (Exception ex) {
                    Logger.Warning(ex, "Unable to update the status of cotnract #{0}", contract.Id);
                    _transactionManager.Cancel();
                }
            }
        }
    }
}