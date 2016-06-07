using System;
using System.Reflection;
using Common;
using EnergyTrading.Logging;
using EnergyTrading.Xml.Serialization;
using Messaging.Errors;
using Messaging.Transport;
using Microsoft.Practices.ObjectBuilder2;
using Model;

namespace BondValidator
{
    public class ValidationMessageHandler : IHandleIncomingMessages<Bond>
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
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
            Logger.InfoFormat("Validator Received {0}", bond.LogFormat());
            if (!bond.MidPrice.HasValue)
            {
                raiseErrorMessages.RaiseError(new BusinessErrorException("BondFlowDemo.Validator.Error", "Bond is not valid"), new ErrorMessageRelatedData { OriginalContent = bond.XmlSerialize() });
                Logger.InfoFormat("Validator Raised Error : Missing Price : {0}", bond.LogFormat());
            }
            else
            {
                transport.Send(bond);
                Logger.InfoFormat("Validator Sent Valid {0}", bond.LogFormat());
            }
        }
    }
}