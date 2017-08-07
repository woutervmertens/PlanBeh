using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace PlanBeh.Models
{
    public enum NodeType
    {
        CONDITIONAL = 0,
        ACTION,
        SEQUENCE,
        SELECTOR,
        PARTIALSEQUENCE,
        PARTIALSELECTOR,
        PARALLEL,
        PRIORITYLIST,
        RANDOMSELECTOR,
        RANDOMSEQUENCE
    }

    public class NodeModel : PropertyChangedBase
    {
        private NodeType _type;
        [XmlElement("nodeType")]
        [JsonProperty("nodeType")]
        public NodeType Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
                OnPropertyChanged("Type");
            }
        }
        private int _id;
        [XmlElement("logicID")]
        [JsonProperty("logicID")]
        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged("ID");
            }
        }
        private string _description;
        [XmlElement("nodeDescription")]
        [JsonProperty("nodeDescription")]
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }
        [XmlElement("nodePos")]
        [JsonProperty("nodePos")]
        public Point Position { get; set; }
        private string _name;
        [XmlElement("nodeName")]
        [JsonProperty("nodeName")]
        public string NodeName
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
    }
}
