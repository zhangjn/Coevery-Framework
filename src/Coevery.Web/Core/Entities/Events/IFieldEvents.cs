using Coevery.Events;

namespace Coevery.Core.Entities.Events {
    public interface IFieldEvents : IEventHandler {
        void OnDeleting(string entityName, string fieldName);
    }
}