using System.Reflection;
using Common;
using EnergyTrading.Logging;
using Messaging.Transport;
using Model;

namespace BondStorage
{
    public class StorageMessageHandler : IHandleIncomingMessages<Bond>
    {
        public static readonly ILogger Logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Handle(Bond bond)
        {
            Logger.InfoFormat("Storage Received {0}", bond.LogFormat());
            BondRepository.Add(bond);
            Logger.InfoFormat("Storage Stored {0}", bond.LogFormat());
        }
    }
}