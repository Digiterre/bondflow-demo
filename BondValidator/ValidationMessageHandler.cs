using System;
using EnergyTrading.Xml.Serialization;
using Messaging.Errors;
using Messaging.Transport;
using Microsoft.Practices.ObjectBuilder2;
using Model;

namespace BondValidator
{
    public class ValidationMessageHandler : IHandleIncomingMessages<Bond>
    {
        private readonly ITransport transport;
        private readonly IRaiseErrorMessages raiseErrorMessages;
        public ValidationMessageHandler(ITransport transport, IRaiseErrorMessages raiseErrorMessages)
        {
            if (transport == null)
            {
                throw new ArgumentNullException(nameof(transport));
            }
            if (raiseErrorMessages == null)
            {
                throw new ArgumentNullException(nameof(raiseErrorMessages));
            }
            this.transport = transport;
            this.raiseErrorMessages = raiseErrorMessages;
        }

        public void Handle(Bond bond)
        {
            if (string.IsNullOrWhiteSpace(bond.Name) || !bond.MidPrice.HasValue || BondType.Unknown.Equals(bond.Type))
            {
                raiseErrorMessages.RaiseError(new BusinessErrorException("BondFlowDemo.Validator.Error", "Bond is not valid"), new ErrorMessageRelatedData { OriginalContent = bond.XmlSerialize() });
            }
            else
            {
                transport.Send(bond);
            }
        }
    }
}