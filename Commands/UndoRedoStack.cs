using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanBeh.Commands
{
    public class UndoRedoStack<T>
    {
        private Stack<ICommand<T>> _undo;
        private Stack<ICommand<T>> _redo;

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
            _undo = new Stack<ICommand<T>>();
            _redo = new Stack<ICommand<T>>();
        }
        public T Do(ICommand<T> cmd, T input)
        {
            T output = cmd.Do(input);
            _undo.Push(cmd);
            _redo.Clear();
            return output;
        }
        public T Undo(T input)
        {
            if(_undo.Count > 0)
            {
                ICommand<T> cmd = _undo.Pop();
                T output = cmd.Undo(input);
                _redo.Push(cmd);
                return output;
            }
            else
            {
                return input;
            }
        }
        public T Redo(T input)
        {
            if(_redo.Count > 0)
            {
                ICommand<T> cmd = _redo.Pop();
                T output = cmd.Do(input);
                _undo.Push(cmd);
                return output;
            }
            else
            {
                return input;
            }
        }
    }
}
