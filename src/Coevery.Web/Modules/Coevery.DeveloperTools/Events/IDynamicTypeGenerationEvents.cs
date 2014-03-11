using System.Reflection.Emit;
using Coevery.Events;

namespace Coevery.DeveloperTools.Events {
    public interface IDynamicTypeGenerationEvents : IEventHandler {
        void OnBuilded(ModuleBuilder moduleBuilder);
    }
}
