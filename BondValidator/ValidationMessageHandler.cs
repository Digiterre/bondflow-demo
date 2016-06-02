using System;
using Messaging.Transport;
using Microsoft.Practices.ObjectBuilder2;
using Model;

namespace BondValidator
{
    public class ValidationMessageHandler : IHandleIncomingMessages<Bond>
    {
        private readonly ITransport transport;
        public ValidationMessageHandler(ITransport transport)
        {
            if (transport == null)
            {
                throw new ArgumentNullException(nameof(transport));
            }
            this.transport = transport;
        }

        public void Handle(Bond bond)
        {
            if (string.IsNullOrWhiteSpace(bond.Name) || !bond.MidPrice.HasValue)
            {
                // error case : really should do something here
            }
            else
            {
                transport.Send(bond);
            }
        }
    }
}