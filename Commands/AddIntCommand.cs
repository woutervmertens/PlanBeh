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
    public class AddIntCommand : ICommand<int>
    {
        private int _Value;

        public int Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
            }
        }

        public AddIntCommand()
        {
            _Value = 0;
        }
        public AddIntCommand(int value)
        {
            _Value = value;
        }

        public int Do(int input)
        {
            return input + _Value;
        }
        public int Undo(int input)
        {
            return input - _Value;
        }

    }
}
