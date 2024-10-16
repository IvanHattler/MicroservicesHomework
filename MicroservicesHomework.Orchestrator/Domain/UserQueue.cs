using System.Collections.Concurrent;

namespace MicroservicesHomework.Orchestrator.Domain
{
    public class UserQueue
    {
        private IDictionary<string, ConcurrentQueue<long>> _queue;
    }
}
