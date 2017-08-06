using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;
using PlanBeh.Models;
using PlanBeh.Views;
using System.ComponentModel;
using PlanBeh.Annotations;
using System.Runtime.CompilerServices;

namespace PlanBeh.ViewModels
{
    class EditAddViewModel : INotifyPropertyChanged
    {
        public EditAddView View;
        public RelayCommand ButtonCommand { get; private set; }
        private NodeViewModel _selectedNode;
        private String _name;
        public String Name
        {
            get { return _name; }
            set { _name = value;
                OnPropertyChanged("Name");
            }
        }

        private NodeType _type;
        public NodeType Type
        {
            get { return _type; }
            set { _type = value;
                OnPropertyChanged("Type");
            }
        }

        private String _desc;
        public String Desc
        {
            get { return _desc; }
            set { _desc = value;
                OnPropertyChanged("Desc");
            }
        }

        public EditAddViewModel(ref NodeViewModel selectedNode, EditAddView view)
        {
            _selectedNode = selectedNode;
            _name = _selectedNode.Node.Name;
            _type = _selectedNode.Node.Type;
            _desc = _selectedNode.Node.Description;
            View = view;

            ButtonCommand = new RelayCommand(ButtonClick);
        }
        void ButtonClick()
        {
            _selectedNode.Node.Name = _name;
            _selectedNode.Node.Type = _type;
            _selectedNode.Node.Description = _desc;
            View.DialogResult = true;
            View.Close();
        }

        public IEnumerable<NodeType> NodeTypes
        {
            get { return Enum.GetValues(typeof(NodeType)).Cast<NodeType>(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
