using System.Reflection.Emit;
using Coevery.Events;

namespace Coevery.DeveloperTools.Entities.Events {
    public interface IDynamicTypeGenerationEvents : IEventHandler {
        void OnBuilded(ModuleBuilder moduleBuilder);
    }
}
