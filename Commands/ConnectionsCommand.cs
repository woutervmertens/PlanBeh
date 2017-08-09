using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using PlanBeh.ViewModels;

namespace PlanBeh.Commands
{
    public class ConnectionsCommand : ICommand<ObservableCollection<ConnectionViewModel>, ConnectionViewModel>
    {
        private ConnectionViewModel _con;

        public ConnectionsCommand(ConnectionViewModel con)
        {
            _con = con;
        }

        public void Do(ref ObservableCollection<ConnectionViewModel> col)
        {
            col.Add(_con);
        }

        public void Undo(ref ObservableCollection<ConnectionViewModel> col)
        {
            col.Remove(_con);
        }
    }

    //HOW TO USE: (Con and ConCol)
    //UndoRedoStack<ObservableCollection<ConnectionViewModel>, ConnectionViewModel> ur = new UndoRedoStack<ObservableCollection<ConnectionViewModel>, ConnectionViewModel>();
    //
    ////Add connector:
    //ur.Do(new ConnectionsCommand(Con), ref ConCol);
    //ur.Undo(ref ConCol);
    //ur.Redo(ref ConCol);
}
