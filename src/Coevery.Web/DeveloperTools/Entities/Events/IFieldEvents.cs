using Coevery.Events;

namespace Coevery.DeveloperTools.Entities.Events {
    public interface IFieldEvents : IEventHandler {
        void OnCreated(string entityName, string fieldName, bool isInLayout);
        void OnDeleting(string entityName, string fieldName);
    }
}