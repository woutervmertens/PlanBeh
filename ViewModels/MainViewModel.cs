using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using PlanBeh.Annotations;
using PlanBeh.Models;
using GalaSoft.MvvmLight.Command;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace PlanBeh.ViewModels
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private Border WorkSpace;
        private int LogicIDCounter = 0;

        private bool _isInEditMode;
        public bool IsInEditMode
        {
            get { return _isInEditMode; }
            set
            {
                _isInEditMode = value;
                OnPropertyChanged("IsInEditMode");
            }
        }

        private NodeModel _activeNode;
        public NodeModel ActiveNode
        {
            get { return _activeNode; }
            set
            {
                _activeNode = value;
                OnPropertyChanged("ActiveNode");
            }
        }

        private NodeViewModel _selectedNode;
        public NodeViewModel SelectedNode
        {
            get { return _selectedNode; }
            set
            {
                _selectedNode = value;
                OnPropertyChanged("SelectedNode");
            }
        }

        private bool _blockTrigger;
        public bool BlockTrigger
        {
            get { return _blockTrigger; }
            set
            {
                _blockTrigger = value;
                OnPropertyChanged("BlockTrigger");
            }
        }

        private CompositeCollection _workspaceCollection;
        public CompositeCollection WorkSpaceCollection
        {
            get { return _workspaceCollection; }
            set
            {
                _workspaceCollection = value;
                OnPropertyChanged("PropertyName");
            }
        }

        private ObservableCollection<NodeViewModel> _NodeCollection;
        public ObservableCollection<NodeViewModel> NodeCollection
        {
            get { return _NodeCollection; }
            set
            {
                _NodeCollection = value;
                UpdateWorkSpaceCollection();
                OnPropertyChanged("NodeCollection");
            }
        }

        private ObservableCollection<ConnectionViewModel> _connectionCollection;
        public ObservableCollection<ConnectionViewModel> ConnectionCollection
        {
            get { return _connectionCollection; }
            set
            {
                _connectionCollection = value;
                UpdateWorkSpaceCollection();
                OnPropertyChanged("ConnectionCollection");
            }
        }

        private bool _isConnecting;
        public bool IsConnecting
        {
            get { return _isConnecting; }
            set
            {
                _isConnecting = value;
                OnPropertyChanged("IsConnecting");
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

        public void SetActiveNode(NodeModel obj)
        {
            ActiveNode = obj;
        }

        public RelayCommand<object> PlaceNodeCommand { get; set; }
        public RelayCommand PlaceConnectionCommand { get; set; }

        public RelayCommand StopConnectingCommand { get; set; }

        public RelayCommand SaveCommand { get; set; }
        public RelayCommand LoadCommand { get; set; }

        public RelayCommand EditCommand { get; set; }
        public RelayCommand AddCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }

        public void PlaceNode(object obj)
        {
            if (!BlockTrigger)
            {
                ActiveNode.ID = LogicIDCounter++;
                NodeViewModel temp = new NodeViewModel();
                temp.Node = ActiveNode;
                temp.Position = Mouse.GetPosition((Border)obj);
                temp.MainView = this;
                temp.WorkSpace = (Border)obj;
                NodeCollection.Add(temp);

                if (WorkSpace == null)
                    WorkSpace = (Border)obj;

                UpdateActivities();
            }
            BlockTrigger = false;
        }

        public void PlaceConnection()
        {
            if (OriginNode != null && TargetNode != null)
            {
                IsConnecting = false;
                ConnectionViewModel temp = new ConnectionViewModel();
                temp.OriginNode = _originNode;
                temp.TargetNode = _targetNode;
                temp.MainView = this;
                temp.WorkSpace = WorkSpace;
                _connectionCollection.Add(temp);
                UpdateActivities();
            }
        }

        public void StopConnection()
        {
            IsConnecting = false;
        }

        public void UpdateActivities()
        {
            foreach (NodeViewModel Node in NodeCollection)
            {
                Node.ResetConnections();
            }

            foreach (ConnectionViewModel connection in ConnectionCollection)
            {
                connection.OriginNode.AddOutGoingConnection(connection.TargetNode);
                connection.TargetNode.AddIncomingConnection(connection.OriginNode);
            }

            foreach (NodeViewModel Node in NodeCollection)
            {
                Node.UpdateIsActive();
            }
        }

        public void UpdateWorkSpaceCollection()
        {
            if (NodeCollection != null && ConnectionCollection != null)
            {
                WorkSpaceCollection = new CompositeCollection();
                WorkSpaceCollection.Add(new CollectionContainer() { Collection = NodeCollection });
                WorkSpaceCollection.Add(new CollectionContainer() { Collection = ConnectionCollection });
            }
        }

        void SaveXML()
        {
            XmlSerializer xs = new XmlSerializer(typeof(WorkSpace));
            WorkSpace tempWorkSpace = new WorkSpace();

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "XML File (*.xml)|*.xml";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.ShowDialog();
            {
                foreach (ConnectionViewModel connection in ConnectionCollection)
                {
                    ConnectionModel temp = new ConnectionModel();
                    temp.InputID = connection.OriginNode.Node.ID;
                    temp.OutPutID = connection.TargetNode.Node.ID;
                    tempWorkSpace.Connections.Add(temp);
                }
                foreach (NodeViewModel Node in NodeCollection)
                {
                    tempWorkSpace.Nodes.Add(Node.Node);
                }
                using (var fs = File.OpenWrite(saveFileDialog.FileName))
                {
                    xs.Serialize(fs, tempWorkSpace);
                }
            }
        }

        void LoadXML()
        {
            XmlSerializer xs = new XmlSerializer(typeof(WorkSpace));
            WorkSpace tempWorkSpace = new WorkSpace();

            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "XML File (*.xml)|*.xml";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.ShowDialog();
            {
                using (var fs = File.OpenRead(openFileDialog.FileName))
                {
                    tempWorkSpace = xs.Deserialize(fs) as WorkSpace;
                }

                WorkSpaceCollection = new CompositeCollection();
                NodeCollection = new ObservableCollection<NodeViewModel>();
                ConnectionCollection = new ObservableCollection<ConnectionViewModel>();

                foreach (NodeModel Node in tempWorkSpace.Nodes)
                {
                    NodeViewModel temp = new NodeViewModel();
                    temp.Node = Node;
                    temp.Position = Node.Position;
                    temp.MainView = this;
                    temp.WorkSpace = WorkSpace;
                    NodeCollection.Add(temp);
                }
                foreach (ConnectionModel connection in tempWorkSpace.Connections)
                {
                    ConnectionViewModel temp = new ConnectionViewModel();
                    temp.OriginNode = NodeCollection[connection.InputID];
                    temp.TargetNode = NodeCollection[connection.OutPutID];
                    temp.MainView = this;
                    temp.WorkSpace = WorkSpace;
                    ConnectionCollection.Add(temp);
                }
            }
            UpdateWorkSpaceCollection();
            UpdateActivities();
        }

        void OpenEditWindow()
        {

        }

        void OpenAddWindow()
        {

        }

        void DeleteSelectedNode()
        {

        }

        public MainViewModel()
        {
            NodeCollection = new ObservableCollection<NodeViewModel>();
            ConnectionCollection = new ObservableCollection<ConnectionViewModel>();

            PlaceNodeCommand = new RelayCommand<object>(PlaceNode);
            PlaceConnectionCommand = new RelayCommand(PlaceConnection);

            SaveCommand = new RelayCommand(SaveXML);
            LoadCommand = new RelayCommand(LoadXML);

            EditCommand = new RelayCommand(OpenEditWindow);
            AddCommand = new RelayCommand(OpenAddWindow);
            DeleteCommand = new RelayCommand(DeleteSelectedNode);
        }
            
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
