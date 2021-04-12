using Application.Models.CustomDatastructures;
using GalaSoft.MvvmLight;

namespace Application.Models.Memory
{
    public class Port : ObservableObject
    {
        private byte _portLatch = 0;

        public byte PortLatch
        {
            get
            {
                return _portLatch;
            }
            set
            {
                _portLatch = value;
            }
        }

        private ObservableByte _TRISValue;

        public ObservableByte TRISValue
        {
            get
            {
                return _TRISValue;
            }
            set
            {
                SetTRIS(value.Value);
                RaisePropertyChanged();
            }
        }


        public ObservableByte _portValue;

        public ObservableByte PortValue
        {
            get
            {
                return _portValue;
            }
            set
            {
                SetPort(value.Value);
                RaisePropertyChanged();
                RaisePropertyChanged("Pin0");
                RaisePropertyChanged("Pin1");
                RaisePropertyChanged("Pin2");
                RaisePropertyChanged("Pin3");
                RaisePropertyChanged("Pin4");
                RaisePropertyChanged("Pin5");
                RaisePropertyChanged("Pin6");
                RaisePropertyChanged("Pin7");
            }
        }

        public bool Pin0
        {
            get
            {
                return GetPin(0);
            }
            set
            {
                SetPin(value, 0);
            }
        }

        public bool Pin1
        {
            get
            {
                return GetPin(1);
            }
            set
            {
                SetPin(value, 1);
            }
        }

        public bool Pin2
        {
            get
            {
                return GetPin(2);
            }
            set
            {
                SetPin(value, 2);
            }
        }

        public bool Pin3
        {
            get 
            {
                return GetPin(3);
            }
            set
            {
                SetPin(value, 3);
            }
        }

        public bool Pin4
        {
            get
            {
                return GetPin(4);
            }
            set
            {
                SetPin(value, 4);
                //SetPortAPin4(value);
            }
        }

        public bool Pin5
        {
            get
            {
                return GetPin(5);
            }
            set
            {
                SetPin(value, 5);
            }
        }

        public bool Pin6
        {
            get
            {
                return GetPin(6);
            }
            set
            {
                SetPin(value, 6);
            }
        }

        public bool Pin7
        {
            get
            {
                return GetPin(7);
            }
            set
            {
                SetPin(value, 7);
            }
        }

        private bool GetPin(int pinNumber)
        {
            byte mask = 0b0000_0001;
            mask = (byte)(mask << pinNumber);
            int pin = (PortValue.Value & mask);
            if (pin > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SetPin(bool value, int pinNumber)
        {
            byte mask = 0b0000_0001;
            mask = (byte)(mask << pinNumber);
            if ((TRISValue.Value & mask) > 0)
            {
                if (value == true)
                {
                    PortValue.Value |= mask;
                }
                else
                {
                    PortValue.Value &= (byte)~mask;
                }
                RaisePropertyChanged("PortValue");
                RaisePropertyChanged("Pin"+pinNumber);
            }
        }

        private void SetPort(byte value)
        {
            for (int i = 0; i < 8; i++)
            {
                byte mask = 0b0000_0001;
                mask = (byte)(mask << i);
                if ((_portValue.Value & mask) == 0)
                {
                    if ((value & mask) == 0)
                    {
                        _portValue.Value &= (byte)~mask;
                    }
                    else
                    {
                        _portValue.Value |= mask;
                    }
                }
                else
                {
                    if ((value & mask) == 0)
                    {
                        _portLatch &= (byte)~mask;
                    }
                    else
                    {
                        _portLatch |= mask;
                    }
                }
            }
        }

        private void SetTRIS(byte value)
        {
            for (int i = 0; i < 8; i++)
            {
                byte mask = 0b0000_0001;
                mask = (byte)(mask << i);
                if ((value & mask) == 0 && (_TRISValue.Value & mask) > 0)
                {
                    if ((PortLatch & mask) == 0)
                    {
                        _TRISValue.Value &= (byte)~mask;
                    }
                    else
                    {
                        _TRISValue.Value |= mask;
                    }
                }
                _TRISValue.Value = value;
            }
        }

        public Port()
        {
            _TRISValue = new ObservableByte();
            _portValue = new ObservableByte();
        }
        //public void SetPortAPin4(bool valueToSet)
        //{
        //    //PortA PIN 4 ist source von Timer0 
        //    if ((_RAMModel.RAMList[8].Byte1.Value & 0b0010_0000) > 0)
        //    {
        //        if((_RAMModel.RAMList[8].Byte1.Value & 0b0001_0000) == 0)
        //        {
        //            //steigende Flanke
        //            if((valueToSet == true)
        //                && ((_RAMModel.RAMList[0].Byte5.Value & 0b0001_0000) == 0))
        //            {
        //                _RAMModel.IncTimer0();
        //            }
        //        }
        //        else
        //        {
        //            //fallende Flanke
        //            if ((valueToSet == false)
        //                && ((_RAMModel.RAMList[0].Byte5.Value & 0b0001_0000) > 0))
        //            {
        //                _RAMModel.IncTimer0();
        //            }
        //        }
        //    }
        //    //Wert setzen
        //    if (valueToSet == true)
        //    {
        //        PortValue.Value |= 0b0001_0000;
        //    }
        //    else
        //    {
        //        PortValue.Value &= 0b1110_1111;
        //    }
        //}
    }
}
