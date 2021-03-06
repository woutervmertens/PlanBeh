﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using PlanBeh.Annotations;
using PlanBeh.Models;

namespace PlanBeh.ViewModels
{
    public class ConnectionViewModel : PropertyChangedBase
    {
        public MainViewModel MainView;
        public Border WorkSpace;

        private Point _position;
        public Point Position
        {
            get { return _position; }
            set
            {
                _position = value;
                OnPropertyChanged("Position");
            }
        }

        private NodeViewModel _originNode;
        public NodeViewModel OriginNode
        {
            get { return _originNode; }
            set
            {
                _originNode = value;
                OnPropertyChanged("OriginNode");
            }
        }

        private NodeViewModel _targetNode;
        public NodeViewModel TargetNode
        {
            get { return _targetNode; }
            set
            {
                _targetNode = value;
                OnPropertyChanged("TargetNode");
            }
        }

        public ConnectionViewModel()
        {

        }
    }
}
