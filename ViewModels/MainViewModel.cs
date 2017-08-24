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
using PlanBeh.Commands;
using PlanBeh.Views;
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
    public class MainViewModel : PropertyChangedBase
    {
        public enum ThemesEnum
        {
            Default,
                ExpressionDark, ExpressionLight,
                RainierOrange, RainierPurple, RainierRadialBlue,
                ShinyBlue, ShinyRed,
                ShinyDarkTeal, ShinyDarkGreen, ShinyDarkPurple,
                DavesGlossyControls,
                WhistlerBlue,
                BureauBlack, BureauBlue,
                BubbleCreme,
                TwilightBlue,
                UXMusingsRed, UXMusingsGreen,
                UXMusingsRoughRed, UXMusingsRoughGreen,
                UXMusingsBubblyBlue
        }

        public MainWindow MainView { get; set; }
        private Border WorkSpace;
        private int LogicIDCounter = 0;

        private UndoRedoStack<ObservableCollection<ConnectionViewModel>, ConnectionViewModel> UndoRedoManager;

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

        private NodeType _selectedInfoType;
        public NodeType SelectedInfoType
        {
            get { return _selectedInfoType; }
            set
            {
                _selectedInfoType = value;
                SelectedInfoDesc = _nodeData.NodeTypeDescriptions[(int)_selectedInfoType];
                SelectedInfoColor = _nodeData.NodeColorHexes[(int)_selectedInfoType];
                OnPropertyChanged("SelectedInfoType");
            }
        }
        private NodeData _nodeData;
        private String _selectedInfoDesc;
        public String SelectedInfoDesc
        {
            get { return _selectedInfoDesc; }
            set
            {
                _selectedInfoDesc = value;
                OnPropertyChanged("SelectedInfoDesc");
            }
        }
        public String _selectedInfoColor;
        public String SelectedInfoColor
        {
            get { return _selectedInfoColor; }
            set
            {
                _selectedInfoColor = value;
                OnPropertyChanged("SelectedInfoColor");
            }
        }

        public IEnumerable<NodeType> NodeTypes
        {
            get { return Enum.GetValues(typeof(NodeType)).Cast<NodeType>(); }
        }

        public IEnumerable<ThemesEnum> ThemesList
        {
            get { return Enum.GetValues(typeof(ThemesEnum)).Cast<ThemesEnum>(); }
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
                OnPropertyChanged("WorkSpaceCollection");
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

        private float _totalWorkSpaceWidth;
        public float TotalWorkSpaceWidth
        {
            get { return _totalWorkSpaceWidth; }
            set
            {
                _totalWorkSpaceWidth = value;
                OnPropertyChanged("TotalWorkSpaceWidth");
            }
        }

        private float _totalWorkSpaceHeight;
        public float TotalWorkSpaceHeight
        {
            get { return _totalWorkSpaceHeight; }
            set
            {
                _totalWorkSpaceHeight = value;
                OnPropertyChanged("TotalWorkSpaceHeight");
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

        public RelayCommand<object> SetWorkSpaceCommand { get; set; }

        public RelayCommand<object> EditCommand { get; set; }
        public RelayCommand<object> AddCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }

        public RelayCommand UndoConnectionCommand { get; set; }
        public RelayCommand RedoConnectionCommand { get; set; }

        public RelayCommand AddWorkSpaceWidthCommand { get; set; }
        public RelayCommand AddWorkSpaceHeightCommand { get; set; }

        public void UndoConnection()
        {
            if(UndoRedoManager.UndoCount > 0)
                UndoRedoManager.Undo(ref _connectionCollection);
        }

        public void RedoConnection()
        {
            if(UndoRedoManager.RedoCount > 0)
                UndoRedoManager.Redo(ref _connectionCollection);
        }

        public void AddWorkSpaceWidth()
        {
            TotalWorkSpaceWidth += 500;
        }

        public void AddWorkSpaceHeight()
        {
            TotalWorkSpaceHeight += 500;
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
                UndoRedoManager.Do(new ConnectionsCommand(temp), ref _connectionCollection );
                UpdateActivities();
            }
        }

        public void StopConnection()
        {
            IsConnecting = false;
        }

        public void UpdateActivities()
        {
            foreach (NodeViewModel node in NodeCollection)
            {
                node.ResetConnections();
            }

            foreach (ConnectionViewModel connection in ConnectionCollection)
            {
                connection.OriginNode.AddOutGoingConnection(connection.TargetNode);
                connection.TargetNode.AddIncomingConnection(connection.OriginNode);
            }

            foreach (NodeViewModel node in NodeCollection)
            {
                node.Update();
            }
        }

        public void UpdateWorkSpaceCollection()
        {
            if (NodeCollection != null && ConnectionCollection != null)
            {
                WorkSpaceCollection = new CompositeCollection();
                WorkSpaceCollection.Add(new CollectionContainer() { Collection = NodeCollection });
                WorkSpaceCollection.Add(new CollectionContainer() { Collection = ConnectionCollection });
                OnPropertyChanged("PropertyName");
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
            Nullable<bool> dialogResult = saveFileDialog.ShowDialog();
            if(dialogResult == true)
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
                tempWorkSpace.Height = TotalWorkSpaceHeight;
                tempWorkSpace.Width = TotalWorkSpaceWidth;
                using (var fs = File.Create(saveFileDialog.FileName)) //automatically closes file
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
            DialogResult result = openFileDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                if (!openFileDialog.CheckPathExists || !openFileDialog.CheckFileExists) return;
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
                    temp.Update();
                    temp.SetSelectedNode();
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
                TotalWorkSpaceHeight = tempWorkSpace.Height;
                TotalWorkSpaceWidth = tempWorkSpace.Width;
            }
            UpdateWorkSpaceCollection();
            UpdateActivities();
        }

        void OpenEditWindow(object obj)
        {
            if (SelectedNode != null)
            {
                EditAddView editView = new EditAddView(ref _selectedNode, "Edit");
                Nullable<bool> dialogResult = editView.ShowDialog();
                if(dialogResult == true)
                {
                    if (WorkSpace == null)
                        WorkSpace = (Border)obj;
                    OnPropertyChanged("SelectedNode");
                    UpdateActivities();
                }
            }
        }

        void OpenAddWindow(object obj)
        {
            NodeViewModel old = SelectedNode;
            SelectedNode = new NodeViewModel();
            Point scrollPoint = MainView.GetScrollPosition();
            SelectedNode.Position = ((Border)obj).TransformToAncestor(System.Windows.Application.Current.MainWindow).Transform(new Point(scrollPoint.X + 150,scrollPoint.Y + 150));
            SelectedNode.MainView = this;
            SelectedNode.WorkSpace = (Border) obj;
            SelectedNode.Node.ID = LogicIDCounter++;
            EditAddView addView = new EditAddView(ref _selectedNode, "Add");
            Nullable<bool> dialogResult = addView.ShowDialog();

            if (dialogResult == true)
            {

                NodeCollection.Add(SelectedNode);
                

                if (WorkSpace == null)
                    WorkSpace = (Border) obj;

                UpdateActivities();
                OnPropertyChanged("SelectedNode");
                SelectedNode.SetSelectedNode();
                if(old != null)old.SetNotSelected();
            }
            else
            {
                SelectedNode = old;
            }
        }

        //public void PlaceNode(object obj)
        //{
        //    if (!BlockTrigger)
        //    {
        //        //ActiveNode.ID = LogicIDCounter++;
        //        //NodeViewModel temp = new NodeViewModel();
        //        //temp.Node = ActiveNode;
        //        //temp.Position = Mouse.GetPosition((Border)obj);
        //        //temp.MainView = this;
        //        //temp.WorkSpace = (Border)obj;
        //        //NodeCollection.Add(temp);

        //        if (WorkSpace == null)
        //            WorkSpace = (Border)obj;

        //        UpdateActivities();
        //    }
        //    BlockTrigger = false;
        //}

        void DeleteSelectedNode()
        {
            if (SelectedNode != null)
            {
                for (int i = 0; i <= ConnectionCollection.Count - 1; i++)
                {
                    var con = ConnectionCollection[i];
                    if (con.OriginNode == SelectedNode || con.TargetNode == SelectedNode)
                    {
                        ConnectionCollection.Remove(con);
                    } 
                }
                NodeCollection.Remove(SelectedNode);
                if (NodeCollection.Count > 1)
                    SelectedNode = NodeCollection[NodeCollection.Count-1];
                else SelectedNode = null;

                UpdateActivities();
            }
        }

        void SetWorkSpace(object obj)
        {
            if (WorkSpace == null)
                WorkSpace = (Border)obj;
        }

        public MainViewModel()
        {
            MainView = (MainWindow)System.Windows.Application.Current.MainWindow;
            NodeCollection = new ObservableCollection<NodeViewModel>();
            ConnectionCollection = new ObservableCollection<ConnectionViewModel>();

            SetWorkSpaceCommand = new RelayCommand<object>(SetWorkSpace);

            //PlaceNodeCommand = new RelayCommand<object>(PlaceNode);
            PlaceConnectionCommand = new RelayCommand(PlaceConnection);

            SaveCommand = new RelayCommand(SaveXML);
            LoadCommand = new RelayCommand(LoadXML);

            EditCommand = new RelayCommand<object>(OpenEditWindow);
            AddCommand = new RelayCommand<object>(OpenAddWindow);
            DeleteCommand = new RelayCommand(DeleteSelectedNode);

            _nodeData = new NodeData();
            SelectedInfoDesc = _nodeData.NodeTypeDescriptions[(int)_selectedInfoType];
            SelectedInfoColor = _nodeData.NodeColorHexes[(int)_selectedInfoType];

            UndoRedoManager = new UndoRedoStack<ObservableCollection<ConnectionViewModel>, ConnectionViewModel>();
            UndoConnectionCommand = new RelayCommand(UndoConnection);
            RedoConnectionCommand = new RelayCommand(RedoConnection);

            AddWorkSpaceHeightCommand = new RelayCommand(AddWorkSpaceHeight);
            AddWorkSpaceWidthCommand = new RelayCommand(AddWorkSpaceWidth);
            TotalWorkSpaceHeight = 500;
            TotalWorkSpaceWidth = 500;
        }
    }
}
