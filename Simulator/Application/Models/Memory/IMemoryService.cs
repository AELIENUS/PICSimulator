using System.Collections.Generic;

namespace Application.Models.Memory
{
    public interface IMemoryService
    {
        bool IsISR { get; set; }
        double CycleCounter { get; set; }

        double Runtime //in µs
        {
            get;
            set;
        }

        double Quartz { get; set; }
        Stack<short> PCStack { get; set; }
        short WReg { get; set; }
        RAMModel RAM { get; set; }
        void PowerReset();
        void OtherReset();
        void Reset_GPR();
    }
}