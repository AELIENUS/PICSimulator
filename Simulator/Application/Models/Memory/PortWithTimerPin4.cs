using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.CustomDatastructures;

namespace Application.Models.Memory
{
    class PortWithTimerPin : IPort
    {
        public byte PortLatch { get; set; }
        public ObservableByte TRISValue { get; set; }
        public ObservableByte PortValue { get; set; }
        public bool Pin0 { get; set; }
        public bool Pin1 { get; set; }
        public bool Pin2 { get; set; }
        public bool Pin3 { get; set; }
        public bool Pin4 { get; set; }
        public bool Pin5 { get; set; }
        public bool Pin6 { get; set; }
        public bool Pin7 { get; set; }

        private void SetPortAPin4(bool valueToSet)
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
