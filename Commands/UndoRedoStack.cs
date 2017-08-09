using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanBeh.ViewModels;

namespace PlanBeh.Commands
{
    public class UndoRedoStack<T, R>
    {
        private Stack<ICommand<T, R>> _undo;
        private Stack<ICommand<T, R>> _redo;

        public int UndoCount
        {
            get
            {
                return _undo.Count;
            }
        }
        public int RedoCount
        {
            get
            {
                return _redo.Count;
            }
        }
        public UndoRedoStack()
        {
            Reset();
        }
        public void Reset()
        {
            _undo = new Stack<ICommand<T, R>>();
            _redo = new Stack<ICommand<T, R>>();
        }
        public void Do(ICommand<T, R> cmd, ref T input)
        {
            cmd.Do(ref input);
            _undo.Push(cmd);
            _redo.Clear();
        }
        public void Undo(ref T input)
        {
            if (_undo.Count > 0)
            {
                ICommand<T, R> cmd = _undo.Pop();
                cmd.Undo(ref input);
                _redo.Push(cmd);
            }
        }
        public void Redo(ref T input)
        {
            if (_redo.Count > 0)
            {
                ICommand<T, R> cmd = _redo.Pop();
                cmd.Do(ref input);
                _undo.Push(cmd);
            }
        }
    }
}
