using System.ComponentModel;

namespace Application.Models.Memory
{
    public interface IRAMModel: INotifyPropertyChanged
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

        /// <summary>
        /// accessor for any location in the memory of the PIC
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        byte this[int index] { get; set; }

        void IncTimer0();
    }
}