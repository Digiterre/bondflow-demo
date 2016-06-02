using Messaging.Transport;
using Model;

namespace BondStorage
{
    public class StorageMessageHandler : IHandleIncomingMessages<Bond>
    {
        public void Handle(Bond bond)
        {
            BondRepository.Add(bond);
        }
    }
}