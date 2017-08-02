using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using PlanBeh.Annotations;
using PlanBeh.Models;
using PlanBeh.Views;
using GalaSoft.MvvmLight.Command;

namespace PlanBeh.ViewModels
{
    public class NodeViewModel : INotifyPropertyChanged
    {
        public MainViewModel MainView;
        public Border WorkSpace;

        private NodeModel _Node;
        public NodeModel Node
        {
            get { return _Node; }
            set
            {
                _Node = value;
                UpdateItem(value);
                OnPropertyChanged("Node");
            }
        }

        private Color _outlineColor;
        public Color OutlineColor
        {
            get { return _outlineColor; }
            set
            {
                _outlineColor = value;
                OnPropertyChanged("OutlineColor");
            }
        }

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                OnPropertyChanged("IsActive");
            }
        }

        private Point _position;
        public Point Position
        {
            get { return _position; }
            set
            {
                _position = value;
                Node.Position = _position;
                InputPos = new Point(_position.X + 25, _position.Y + 35);
                OutputPos = new Point(_position.X + 150 - 25, _position.Y + 35);
                OnPropertyChanged("Position");
            }
        }

        private Point _inputPos;
        public Point InputPos
        {
            get { return _inputPos; }
            set
            {
                _inputPos = value;
                OnPropertyChanged("InputPos");
            }
        }

        private Point _outputPos;
        public Point OutputPos
        {
            get { return _outputPos; }
            set
            {
                _outputPos = value;
                OnPropertyChanged("OutputPos");
            }
        }

        public RelayCommand SetSelectedNodeCommand { get; set; }
        public RelayCommand BlockRelayCommand { get; set; }
        public RelayCommand<object> EndConnectionDragCommand { get; set; }
        public RelayCommand<object> StartConnectionDragCommand { get; set; }

        public RelayCommand StartDragCommand { get; set; }
        public RelayCommand StopDragCommand { get; set; }
        public RelayCommand<object> DragCommand { get; set; }

        public List<NodeViewModel> IncomingConnections = new List<NodeViewModel>();
        public List<NodeViewModel> OutgoingConnections = new List<NodeViewModel>();

        public void SetSelectedNode()
        {
            MainView.SelectedNode = this;
        }

        public void BlockRelay()
        {
            MainView.BlockTrigger = true;
        }

        public void StartConnectionDrag(object obj)
        {
            CanDrag = false;
            MainView.IsConnecting = true;
            MainView.OriginNode = this;
        }

        public void EndConnectionDrag(object obj)
        {
            MainView.TargetNode = this;
            MainView.PlaceConnection();
        }

        public void ResetConnections()
        {
            IncomingConnections = new List<NodeViewModel>();
            OutgoingConnections = new List<NodeViewModel>();
        }

        public void AddOutGoingConnection(NodeViewModel Node)
        {
            OutgoingConnections.Add(Node);
        }

        public void AddIncomingConnection(NodeViewModel Node)
        {
            IncomingConnections.Add(Node);
        }

        public void UpdateIsActive()
        {
            
        }

        private Point StartDragPos = new Point();
        private Point StartPos = new Point();
        private bool IsDragging = false;
        private bool CanDrag = true;

        public void StartDrag()
        {
            if (CanDrag)
            {
                IsDragging = true;
                StartDragPos = Mouse.GetPosition(WorkSpace);
                StartPos = Position;
            }
            CanDrag = true;
        }

        public void StopDrag()
        {
            IsDragging = false;
        }

        public void Drag(object obj)
        {
            if (IsDragging)
            {
                Point p = Mouse.GetPosition(WorkSpace);
                Point diff = new Point(p.X - StartDragPos.X, p.Y - StartDragPos.Y);
                Position = new Point(StartPos.X + diff.X, StartPos.Y + diff.Y);
            }
        }

        public void UpdateItem(NodeModel Node)
        {
            if (Node == null)
                return;
            OutlineColor = Node.GetColor((int)Node.Type);
        }

        public NodeViewModel()
        {
            IncomingConnections = new List<NodeViewModel>();
            OutgoingConnections = new List<NodeViewModel>();

            SetSelectedNodeCommand = new RelayCommand(SetSelectedNode);
            BlockRelayCommand = new RelayCommand(BlockRelay);

            StartConnectionDragCommand = new RelayCommand<object>(StartConnectionDrag);
            EndConnectionDragCommand = new RelayCommand<object>(EndConnectionDrag);

            StartDragCommand = new RelayCommand(StartDrag);
            StopDragCommand = new RelayCommand(StopDrag);
            DragCommand = new RelayCommand<object>(Drag);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
