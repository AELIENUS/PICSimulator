using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Memory
{
    /// <summary>
    /// Defines the interface for RAM classes for PIC controllers.
    /// RAM has to be directly addressable via an index. 
    /// </summary>
    public interface IPICRAM
    {
        byte this[int index] { get; set; }
    }
}
