using Coevery.ContentManagement.MetaData.Models;
using Coevery.Events;

namespace Coevery.Core.Entities.Events {
    public interface IFieldEvents : IEventHandler {
        void OnAdding(string entityName, string fieldName, SettingsDictionary settings);
        void OnDeleting(string entityName, string fieldName);
    }
}