using System.ComponentModel;

namespace Application.Models.Memory
{
    public interface IRAMModel
    {
        Port PortA { get; set; }
        Port PortB { get; set; }
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

        event PropertyChangedEventHandler PropertyChanged;
    }
}