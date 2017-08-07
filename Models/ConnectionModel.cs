using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace PlanBeh.Models
{
    public class ConnectionModel
    {
        [XmlElement("ConnectionInputID")]
        [JsonProperty("ConnectionInputID")]
        public int InputID { get; set; }
        [XmlAttribute("ConnectionOutputID")]
        [JsonProperty("ConnectionOutputID")]
        public int OutPutID { get; set; }
    }
}
