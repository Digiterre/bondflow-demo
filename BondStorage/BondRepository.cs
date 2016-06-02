using System.Collections.Generic;
using System.IO;
using EnergyTrading.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Model;

namespace BondStorage
{
    public class BondRepository
    {
        private static List<Bond> bonds = null;
        private static string fileName = "Bonds.xml";

        private static void LoadBonds()
        {
            if (bonds == null)
            {
                if (File.Exists(fileName))
                {
                    var source = File.ReadAllText(fileName);
                    bonds = source.DeserializeDataContractXmlString<List<Bond>>();
                }
                else
                {
                    bonds = new List<Bond>();
                }
            }
        }

        private static void SaveBonds()
        {
            File.WriteAllText(fileName, bonds.DataContractSerialize());
        }

        public static void Add(Bond bond)
        {
            LoadBonds();
            bonds.Add(bond);
            SaveBonds();
        }


    }
}