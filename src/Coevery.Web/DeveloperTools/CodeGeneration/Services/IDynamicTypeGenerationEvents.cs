using System.Reflection.Emit;
using Coevery.Events;

namespace Coevery.DeveloperTools.CodeGeneration.Services {
    public interface IDynamicTypeGenerationEvents : IEventHandler {
        void OnBuilded(ModuleBuilder moduleBuilder);
    }
}
