using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlanBeh.Models
{
    public class Node : DiagramObject
    {
        public Node()
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

    }
}
