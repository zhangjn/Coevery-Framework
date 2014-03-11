using System.Reflection.Emit;
using Coevery.Events;

namespace Coevery.Core.Entities.Events {
    public interface IDynamicTypeGenerationEvents : IEventHandler {
        void OnBuilded(ModuleBuilder moduleBuilder);
    }
}
