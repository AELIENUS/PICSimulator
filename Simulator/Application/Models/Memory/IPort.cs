using System.ComponentModel;
using Application.Models.CustomDatastructures;

namespace Application.Models.Memory
{
    public interface IPort : INotifyPropertyChanged
    {
        byte PortLatch { get; set; }
        ObservableByte TRISValue { get; set; }
        ObservableByte PortValue { get; set; }
        bool Pin0 { get; set; }
        bool Pin1 { get; set; }
        bool Pin2 { get; set; }
        bool Pin3 { get; set; }
        bool Pin4 { get; set; }
        bool Pin5 { get; set; }
        bool Pin6 { get; set; }
        bool Pin7 { get; set; }
    }
}