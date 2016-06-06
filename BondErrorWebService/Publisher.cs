using System;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using EnergyTrading.Configuration;
using EnergyTrading.Container.Unity;
using EnergyTrading.Xml.Serialization;
using Messaging.Transport;
using Messaging.Unity.Configuration;
using Model;
using TransportMetadata.Metadata;

namespace BondErrorWebService
{
    public class Publisher
    {
        public static ITransportConfigurator configurator;
        public static IManageSendingMessages sendingManager;
        private static object lockObject = new object();

        public static void Start()
        {
            lock (lockObject)
            {
                if (configurator == null)
                {
                    configurator = new TransportConfigurator()
                        .ConfigureLogging()
                        .ConfigureContainer()
                        .StandardRegistrars()
                        .ConfigureRegistrars();

                    // Any other start up tasks we want.
                    ConfigurationBootStrapper.Initialize(
                        configurator.Container.ResolveAllToEnumerable<IGlobalConfigurationTask>());

                    sendingManager = configurator.AsSendingManager();
                }
            }
        }

        public static void Stop()
        {
            if (sendingManager != null)
            {
                sendingManager.Stop();
                sendingManager = null;
            }
            if (configurator != null)
            {
                configurator.Shutdown();
                configurator = null;
            }
        }

        public static void SendBond(Bond bond)
        {
            Start();
            var metadata = new MessageMetadata
            {
                MessageDetails = new MessageDetails { Category = "TEST" },
                OriginInfo = new OriginInfo { OriginatingSequenceId = "originatingsequence", MasterDataSourceName = "mdmName", OriginatingProcesUniqueName = "Fixed Bond Error " + Environment.MachineName }
            };

            sendingManager.Send(new Tuple<Bond, MessageMetadata>(bond, metadata));
        }
    }
}