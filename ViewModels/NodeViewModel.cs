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
    public class NodeViewModel : PropertyChangedBase
    {
        public MainViewModel MainView;
        public Border WorkSpace;
        private NodeData _nodedata;

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

        private String _outlineColor;
        public String OutlineColor
        {
            get { return _outlineColor; }
            set
            {
                _outlineColor = value;
                OnPropertyChanged("OutlineColor");
            }
        }

        private String _backColor;
        public String BackColor
        {
            get { return _backColor; }
            set
            {
                _backColor = value;
                OnPropertyChanged("BackColor");
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
            if(MainView.SelectedNode != null) MainView.SelectedNode.SetNotSelected();
            MainView.SelectedNode = this;
            _backColor = _outlineColor;
            OnPropertyChanged("BackColor");
        }

        public void SetNotSelected()
        {
            _backColor = "#FF323232";
            OnPropertyChanged("BackColor");
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

        public void Update()
        {
            OutlineColor = _nodedata.NodeColorHexes[(int)Node.Type];
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
            SetSelectedNode();
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

        public void UpdateItem(NodeModel node)
        {
            if (node == null)
                return;
            OutlineColor = _nodedata.NodeColorHexes[(int)node.Type];
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

            _nodedata = new NodeData();
            _Node = new NodeModel();
            _Node.NodeName = "";
            _Node.Type = NodeType.ACTION;
            _Node.Description = "";
            _outlineColor = _nodedata.NodeColorHexes[(int) _Node.Type];
            _backColor = "#FF323232";
        }
    }
}
