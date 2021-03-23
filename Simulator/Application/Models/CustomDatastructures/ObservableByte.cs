using GalaSoft.MvvmLight;

namespace Application.Models.CustomDatastructures
{
    public class ObservableByte : AbstractByte
    {
        private byte _value;

        public override byte Value 
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value == value)
                {
                    return;
                }
                _value = value;
                RaisePropertyChanged();
            }
        }

        public ObservableByte()
        {
            Value = 0;
        }
    }
}
