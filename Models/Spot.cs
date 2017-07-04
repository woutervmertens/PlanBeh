using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlanBeh.Models
{
    public class Spot : DiagramObject
    {
        private Node _parent;
        public Node Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
            }
        }

        private BindablePoint _offset;
        public BindablePoint Offset
        {
            get { return _offset ?? (_offset = new BindablePoint()); }
        }

        private double _angle;
        public double Angle
        {
            get { return _angle; }
            set
            {
                _angle = value;
                OnPropertyChanged("Angle");
            }
        }

        private bool _isConnected;
        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                _isConnected = value;
                OnPropertyChanged("IsConnected");
            }
        }
        public Spot(Node parent)
        {
            Parent = parent;
            Offset.ValueChanged = Recalculate;
        }

        private double? _lockX;
        public bool LockX
        {
            get { return _lockX != null; }
            set
            {
                _lockX = value ? Offset.X : (double?)null;
                OnPropertyChanged("LockX");
            }
        }

        private double? _lockY;
        public bool LockY
        {
            get { return _lockY != null; }
            set
            {
                _lockY = value ? Offset.Y : (double?)null;
                OnPropertyChanged("LockY");
            }
        }

        public void Recalculate()
        {
            if (Offset.X < 0)
                Offset.X = 0;
            if (Offset.X > 1)
                Offset.X = 1;

            if (Offset.Y < 0)
                Offset.Y = 0;
            if (Offset.Y > 1)
                Offset.Y = 1;

            Location.Value = Point.Add(Parent.Location.Value, new Vector((_lockX ?? Offset.X) * Parent.Size.X, (_lockY ?? Offset.Y) * Parent.Size.Y));
        }
    }
}
