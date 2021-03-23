using Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public class PortBDummy : AbstractByte
    {
        private PortB _PortB;

        public override byte Value
        {
            get
            {
                return _PortB._value;
            }
            set
            {
                _PortB._value = value;
                RaisePropertyChanged();
                _PortB.RaisePropertyChanged("Value");
                _PortB.RaisePropertyChanged("Pin0");
                _PortB.RaisePropertyChanged("Pin1");
                _PortB.RaisePropertyChanged("Pin2");
                _PortB.RaisePropertyChanged("Pin3");
                _PortB.RaisePropertyChanged("Pin4");
                _PortB.RaisePropertyChanged("Pin5");
                _PortB.RaisePropertyChanged("Pin6");
                _PortB.RaisePropertyChanged("Pin7");
            }
        }

        public PortBDummy(PortB port)
        {
            _PortB = port;
        }
    }
}
