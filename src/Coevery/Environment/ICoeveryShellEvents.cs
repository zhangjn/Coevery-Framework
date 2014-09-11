using Coevery.Events;

namespace Coevery.Environment {
    public interface ICoeveryShellEvents : IEventHandler {
        void Activated();
        void Terminating();
    }
}
