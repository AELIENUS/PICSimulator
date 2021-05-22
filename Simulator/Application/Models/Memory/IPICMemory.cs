using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Memory
{
        /// <summary>
        /// Interface for PICMemory specifies that a pic has RAM and a PowerReset, since the PICFAMILY is highly versatile, because the
        /// PIC-Controllers exist in e.g 16-bit and 32-bit variations.
        /// </summary>
        public interface IPICMemory<T>
        {
            T RAM { get; set; }
            void PowerReset();
        }
}
