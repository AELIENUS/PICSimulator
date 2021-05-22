using System.ComponentModel;

namespace Application.Models.Memory
{
    public interface IRAMModel: INotifyPropertyChanged, IPICRAM
    {
        IPort PortA { get; set; }
        IPort PortB { get; set; }
        bool PCWasJump { get; set; }
        bool PCLWasManipulated { get; set; }
        int PCWithClear { get; }
        byte PCL { get; }
        byte PCLATH { get; }
        int PCWithoutClear { get; }
        int ZFlag { get; }
        int CFlag { get; }
        int DCFlag { get; }
        int PCJumpAdress { get; set; }
        byte PrescaleCounter { get; set; }
        void IncTimer0();
    }
}