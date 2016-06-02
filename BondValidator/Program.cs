using System;
using EnergyTrading.Configuration;
using EnergyTrading.Container.Unity;
using Messaging.Unity.Configuration;
using TransportMetadata.Registrars;

namespace BondValidator
{
    class Program
    {
        static void Main(string[] args)
        {
            var configurator = new TransportConfigurator()
                                   .ConfigureLogging()
                                   .ConfigureContainer()
                                   .Register<MetadataMapperRegistrar>()
                                   .StandardRegistrars()
                                   .ConfigureRegistrars();

            // Any other start up tasks we want.
            ConfigurationBootStrapper.Initialize(configurator.Container.ResolveAllToEnumerable<IGlobalConfigurationTask>());

            // Get and start the publisher
            var receivingManager = configurator.AsReceivingManager();
            receivingManager.Start();
            Console.WriteLine("Enter quit to exit");
            while (true)
            {
                var entered = Console.ReadLine();
                if (entered == "quit")
                {
                    return;
                }
            }
        }
    }
}
