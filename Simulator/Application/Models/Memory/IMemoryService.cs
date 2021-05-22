using System.Collections.Generic;
using System.ComponentModel;

namespace Application.Models.Memory
{
    public interface IMemoryService : INotifyPropertyChanged
    {
        bool IsISR { get; set; }
        byte TRISAValue { get; set; }
        byte PortAValue { get; set; }
        bool PortAPin0 { get; set; }
        bool PortAPin1 { get; set; }
        bool PortAPin2 { get; set; }
        bool PortAPin3 { get; set; }
        bool PortAPin4 { get; set; }
        bool PortAPin5 { get; set; }
        bool PortAPin6 { get; set; }
        bool PortAPin7 { get; set; }
        byte TRISBValue { get; set; }
        byte PortBValue { get; set; }
        bool PortBPin0 { get; set; }
        bool PortBPin1 { get; set; }
        bool PortBPin2 { get; set; }
        bool PortBPin3 { get; set; }
        bool PortBPin4 { get; set; }
        bool PortBPin5 { get; set; }
        bool PortBPin6 { get; set; }
        bool PortBPin7 { get; set; }
        double CycleCounter { get; set; }
        int ZFlag { get; }
        int DCFlag { get; }
        int CFlag { get; }

        double Runtime //in µs
        {
            get;
            set;
        }

        double Quartz { get; set; }
        Stack<short> PCStack { get; set; }
        short WReg { get; set; }
        IRAMModel RAM { get; set; }
        int PCWithoutClear { get; }
        byte PCL { get; }
        byte PCLATH { get; }
        void PowerReset();
        void OtherReset();
        void ResetGPR();
    }
}