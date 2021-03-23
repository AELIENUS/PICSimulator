using Application.Models.CustomDatastructures;

namespace Application.Models.Memory
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
