using Application.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public class PortADummy : AbstractByte
    {
        private PortA _PortA;

        public override byte Value
        {
            get
            {
                return _PortA._value;
            }
            set
            {
                _PortA._value = value;
                RaisePropertyChanged();
                _PortA.RaisePropertyChanged("Value");
                _PortA.RaisePropertyChanged("Pin0");
                _PortA.RaisePropertyChanged("Pin1");
                _PortA.RaisePropertyChanged("Pin2");
                _PortA.RaisePropertyChanged("Pin3");
                _PortA.RaisePropertyChanged("Pin4");
                _PortA.RaisePropertyChanged("Pin5");
                _PortA.RaisePropertyChanged("Pin6");
                _PortA.RaisePropertyChanged("Pin7");
            }
        }

        public PortADummy(PortA port)
        {
            _PortA = port;
        }
    }
}
