using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TuyaCommander
{
    internal class TuyaCommanderConfig
    {
        public string Id { get; set; }
        public string Secret { get; set; }
        public int Region { get; set; }
        public int Devices { get; set; }

        public void LoadConfig(string filePath)
        {
            XElement root = XElement.Load(filePath);
            Id = root.Element("id").Value;
            Secret = root.Element("secret").Value;
            Region = Convert.ToInt32(root.Element("region").Value);
            Devices = Convert.ToInt32(root.Element("devices").Value);
        }

        public void SaveConfig(string filePath)
        {
            XElement root = new XElement("tuyacommander",
                new XElement("id", Id),
                new XElement("secret", Secret),
                new XElement("region", Region),
                new XElement("devices", Devices)
            );
            root.Save(filePath);
        }
    }
}