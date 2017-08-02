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

        public Color GetColor(int nt)
        {
            return (Color)ColorConverter.ConvertFromString("#FF323232");
        }

        private Point originalSize;
        private BindablePoint _size;
        public BindablePoint Size
        {
            get { return _size ?? (_size = new BindablePoint()); }
        }
        public NodeType Type { get; set; }
        public int ID { get; set; }
        public string Description { get; set; }
        public string IconPath { get; set; }
        public Point Position { get; set; }
        public string Name { get; set; }
    }
}
