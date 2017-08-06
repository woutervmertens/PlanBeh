using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

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

    public class NodeModel : DiagramObject
    {
        public NodeModel()
        {
            Size.ValueChanged = RecalculateSnaps;
            Location.ValueChanged = RecalculateSnaps;
        }

        private void RecalculateSnaps()
        {
            Snaps.ForEach(x => x.Recalculate());
        }

        private List<Spot> _snaps;
        public List<Spot> Snaps
        {
            get { return _snaps ?? (_snaps = new List<Spot>()); }
        }

        private Point originalSize;
        private BindablePoint _size;
        public BindablePoint Size
        {
            get { return _size ?? (_size = new BindablePoint()); }
        }
        private NodeType _type;
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
        public string IconPath { get; set; }
        public Point Position { get; set; }
        private string _name;
        public string Name
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
