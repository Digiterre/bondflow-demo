using System;
using Messaging.Transport;
using Model;

namespace SpreadCalculator
{
    public class SpreadCalculatorMessageHandler : IHandleIncomingMessages<Bond>
    {
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
            if (bond.MidPrice.HasValue)
            {
                var difference = bond.MidPrice.Value*0.015m;
                bond.Spread = new PriceSpread {Lower = bond.MidPrice - difference, Upper = bond.MidPrice + difference};
            }
            transport.Send(bond);
        }
    }
}