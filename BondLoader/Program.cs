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
            if (args.Length == 1 && string.Equals(args[0], "generate", StringComparison.InvariantCultureIgnoreCase))
            {
                GenerateDataFile();
                return;
            }
            var configurator = new TransportConfigurator()
                                   .ConfigureLogging()
                                   .ConfigureContainer()
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
                    bond.Identifier = DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + "-" + parts[0];
                    if (parts.Length > 1)
                    {
                        bond.Name = parts[1];
                    }
                    if (parts.Length > 2)
                    {
                        bond.Type = parts[2];
                    }
                    decimal price;
                    if (parts.Length > 3 && decimal.TryParse(parts[3], out price))
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

        static void GenerateDataFile()
        {
            var random = new Random();
            var fileBuilder = new StringBuilder();
            for (int i = 1; i <= 1000; ++i)
            {
                var builder = new StringBuilder();
                builder.Append(i);
                builder.Append(",NewBond");
                builder.Append(i);
                var type = i%2 == 0
                    ? (i%3 == 0 ? "Zero Coupon" : "Government")
                    : (i%3 == 0 ? "Corporate" : "Zero Coupon");
                builder.Append(",");
                builder.Append(type);
                builder.Append(",");
                builder.Append(random.Next(1000));
                builder.Append(".");
                builder.Append(random.Next(100).ToString("00"));
                fileBuilder.AppendLine(builder.ToString());
            }
            File.WriteAllText("BondInputData.csv", fileBuilder.ToString());
        }
    }
}
