using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlanBeh.Models
{
    public class Connector : DiagramObject
    {
        public override BindablePoint Location
        {
            get { return new BindablePoint(); }
        }

        private Spot _start;
        public Spot Start
        {
            get { return _start; }
            set
            {
                _start = value;
                OnPropertyChanged("Start");
            }
        }

        private Spot _end;
        public Spot End
        {
            get { return _end; }
            set
            {
                _end = value;
                OnPropertyChanged("End");
                MidPoint.Value = new Point(((End.Location.X + Start.Location.X) / 2),
                                     ((End.Location.Y + Start.Location.Y) / 2));
            }
        }

        private BindablePoint _midPoint;
        public BindablePoint MidPoint
        {
            get { return _midPoint ?? (_midPoint = new BindablePoint()); }
        }
    }
}
