using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.CodeLogic
{
    public interface ICodeToExecute<T>
    {
        T this[int commandIndex] { get; set; }
    }
}
