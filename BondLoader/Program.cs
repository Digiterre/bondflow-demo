using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnergyTrading.Configuration;
using EnergyTrading.Container.Unity;
using Messaging.Unity.Configuration;
using Model;
using TransportMetadata.Metadata;
using TransportMetadata.Registrars;

namespace BondLoader
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
            var sendingManager = configurator.AsSendingManager();

            while (true)
            {
                Console.WriteLine("Enter file path to publish contents (or quit to exit) :");
                var path = Console.ReadLine();
                if (path == "quit")
                {
                    break;
                }
                List<string> lines = new List<string>();
                if (path.ToLowerInvariant().StartsWith("text:"))
                {
                    lines.Add(path.Substring(5));
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                    {
                        Console.WriteLine("Error -- Cannot publish contents. Input file does not exist");
                        continue;
                    }

                    lines = File.ReadAllLines(path).ToList();
                }

                if (lines.Count == 0)
                {
                    Console.WriteLine("cannot publish as there is no contents");
                    continue;
                }

                foreach (var line in lines)
                {
                    var bond = new Bond();
                    var parts = line.Split(',');
                    bond.Name = parts[0];
                    if (parts.Length > 1)
                    {
                        bond.Type = parts[1];
                    }
                    decimal price;
                    if (parts.Length > 2 && decimal.TryParse(parts[2], out price))
                    {
                        bond.MidPrice = price;
                    }

                    var metadata = new MessageMetadata
                    {
                        MessageDetails = new MessageDetails { Category = "TEST" },
                        OriginInfo = new OriginInfo { OriginatingSequenceId = "originatingsequence", MasterDataSourceName = "mdmName", OriginatingProcesUniqueName = "Publisher Console " + Environment.MachineName }
                    };

                    var message = new Tuple<Bond, MessageMetadata>(bond, metadata);

                    sendingManager.Send(message);
                }
            }

            // Stop every thing.
            configurator.Shutdown();
            sendingManager.Stop();
        }
    }
}
