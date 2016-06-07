using System;
using System.Reflection;
using Common;
using EnergyTrading.Logging;
using Messaging.Transport;
using Model;

namespace SpreadCalculator
{
    public class SpreadCalculatorMessageHandler : IHandleIncomingMessages<Bond>
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ITransport transport;

        public SpreadCalculatorMessageHandler(ITransport transport)
        {
            if (transport == null)
            {
                throw new ArgumentNullException(nameof(transport));
            }
            this.transport = transport;
        }

        public void Handle(Bond bond)
        {
            Logger.InfoFormat("Spread Calculator Received {0}", bond.LogFormat());
            if (bond.MidPrice.HasValue)
            {
                var difference = bond.MidPrice.Value*0.015m;
                bond.Spread = new PriceSpread {Lower = bond.MidPrice - difference, Upper = bond.MidPrice + difference};
                Logger.InfoFormat("Spread Calculator Updated {0}", bond.LogFormat());
            }
            transport.Send(bond);
            Logger.InfoFormat("Spread Calculator Sent {0}", bond.LogFormat());
        }
    }
}