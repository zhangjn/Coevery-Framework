using System.Reflection.Emit;
using Coevery.Events;

namespace Coevery.DeveloperTools.EntityManagement.Events {
    public interface IDynamicTypeGenerationEvents : IEventHandler {
        void OnBuilded(ModuleBuilder moduleBuilder);
    }
}
