using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace PlanBeh.Models
{
    public class WorkSpace
    {
        [XmlArray("Nodes")]
        [XmlArrayItem("Node")]
        [JsonProperty("Nodes")]
        public List<NodeModel> Nodes { get; set; } = new List<NodeModel>();

        [XmlArray("Connections")]
        [XmlArrayItem("Connection")]
        [JsonProperty("Connections")]
        public List<ConnectionModel> Connections { get; set; } = new List<ConnectionModel>();

        [XmlElement("Height")]
        [JsonProperty("Height")]
        public float Height { get; set; }

        [XmlElement("Width")]
        [JsonProperty("Width")]
        public float Width { get; set; }
    }
}
