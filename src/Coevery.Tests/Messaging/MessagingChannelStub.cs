using System.Collections.Generic;
using Coevery.Messaging.Services;
using Coevery.Messaging.Models;

namespace Coevery.Tests.Messaging {
    public class MessagingChannelStub : IMessageChannel {
        public List<IDictionary<string, object>> Messages { get; private set; }
        
        public MessagingChannelStub() {
            Messages = new List<IDictionary<string, object>>();
        }

        public void Process(IDictionary<string, object> parameters) {
            Messages.Add(parameters);
        }
    }
}
