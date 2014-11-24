using Coevery.Tasks;
using JetBrains.Annotations;

namespace Coevery.PropertyManagement.Services {
    [UsedImplicitly]
    public class ScheduledTaskExecutor : IBackgroundTask {
        private readonly IRecordStatusUpdator _recordStatusUpdator;
        public ScheduledTaskExecutor(IRecordStatusUpdator recordStatusUpdator) {
            _recordStatusUpdator = recordStatusUpdator;
        }

        public void Sweep() {
            _recordStatusUpdator.UpdateRecordStatus();
        }
    }
}
