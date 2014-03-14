using Coevery.Events;

namespace Coevery.Core.Entities.Events {
    public interface IEntityEvents : IEventHandler {
        void OnCreated(string entityName);
        void OnUpdating(string entityName);
        void OnDeleting(string entityName);
    }
}
