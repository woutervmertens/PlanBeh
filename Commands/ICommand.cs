using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanBeh.Commands
{
    public interface ICommand<T, R>
    {
        void Do(ref T input);
        void Undo(ref T input);
    }
}
