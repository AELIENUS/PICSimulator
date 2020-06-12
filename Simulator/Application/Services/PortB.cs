﻿using Application.Model;
using Applicator.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PortB : ObservableObject
    {
        private RAMModel _RAMModel;

        public PortB(RAMModel model)
        {
            _RAMModel = model;
        }

        public byte Value
        {
            get
            {
                return _RAMModel.RAMList[0].Byte6.Value;
            }
            set
            {
                _RAMModel.RAMList[0].Byte6.Value = value;
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
                int temp = (Value & 0b0000_0001);
                if (temp > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                //INTF Interrupt
                SetPortBPin0(value);
                RaisePropertyChanged();
                RaisePropertyChanged("Value");
            }
        }

        public bool Pin1
        {
            get
            {
                int temp = (Value & 0b0000_0010);
                if (temp > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value == true)
                {
                    Value |= 0b0000_0010;
                }
                else
                {
                    Value &= 0b1111_1101;
                }
                RaisePropertyChanged();
                RaisePropertyChanged("Value");
            }
        }

        public bool Pin2
        {
            get
            {
                int temp = (Value & 0b0000_0100);
                if (temp > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value == true)
                {
                    Value |= 0b0000_0100;
                }
                else
                {
                    Value &= 0b1111_1011;
                }
                RaisePropertyChanged();
                RaisePropertyChanged("Value");
            }
        }

        public bool Pin3
        {
            get
            {
                int temp = (Value & 0b0000_1000);
                if (temp > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value == true)
                {
                    Value |= 0b000_1000;
                }
                else
                {
                    Value &= 0b1111_0111;
                }
                RaisePropertyChanged();
                RaisePropertyChanged("Value");
            }
        }

        public bool Pin4
        {
            get
            {
                int temp = (Value & 0b0001_0000);
                if (temp > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value == true)
                {
                    Value |= 0b0001_0000;
                }
                else
                {
                    Value &= 0b1110_1111;
                }
                RaisePropertyChanged();
                RaisePropertyChanged("Value");
            }
        }

        public bool Pin5
        {
            get
            {
                int temp = (Value & 0b0010_0000);
                if (temp > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value == true)
                {
                    Value |= 0b0010_0000;
                }
                else
                {
                    Value &= 0b1101_1111;
                }
                RaisePropertyChanged();
                RaisePropertyChanged("Value");
            }
        }

        public bool Pin6
        {
            get
            {
                int temp = (Value & 0b0100_0000);
                if (temp > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value == true)
                {
                    Value |= 0b0100_0000;
                }
                else
                {
                    Value &= 0b1011_1111;
                }
                RaisePropertyChanged();
                RaisePropertyChanged("Value");
            }
        }

        public bool Pin7
        {
            get
            {
                int temp = (Value & 0b1000_0000);
                if (temp > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value == true)
                {
                    Value |= 0b1000_0000;
                }
                else
                {
                    Value &= 0b0111_1111;
                }
                RaisePropertyChanged();
                RaisePropertyChanged("Value");
            }
        }

        void SetPortBPin0(bool valueToSet)
        {
            //INTEDG überprüfen
            if((_RAMModel.RAMList[8].Byte0.Value & 0b0100_0000)>0)
            {
                //Flag bei steigender Flanke
                if((valueToSet == true) 
                    && ((_RAMModel.RAMList[0].Byte6.Value & 0b0000_0001)==0))
                {
                    //Set INTF
                    _RAMModel.RAMList[0].Byte11.Value |= 0b0000_0010;
                }
            }
            else
            {
                //Flag bei fallender Flanke
                if ((valueToSet == false)
                    && ((_RAMModel.RAMList[0].Byte6.Value & 0b0000_0001) > 0))
                {
                    //Set INTF
                    _RAMModel.RAMList[0].Byte11.Value |= 0b0000_0010;
                }
            }
            //wert Setzen
            if (valueToSet == true)
            {
                Value |= 0b0000_0001;
            }
            else
            {
                Value &= 0b1111_1110;
            }
        }
    }
}
