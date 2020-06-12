using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Applicator.Model
{
    public class ItemNotifyByte: ObservableObject
    {
        private byte _value;

        public byte Value 
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
                RaisePropertyChanged("Value");
            }
        }

        /*[IndexerName("Bit")]
        public bool this[int index]
        {
            get
            {
                int val;
                switch(index)
                {
                    case 7:
                        //7 Stellen nach rechts
                        val = Value & 0b1000_0000;
                        val = (val >> 7);
                        break;
                    case 6:
                        //6 Stellen nach rechts
                        val = Value & 0b0100_0000;
                        val = (val >> 6);
                        break;
                    case 5:
                        //5 Stellen nach rechts
                        val = Value & 0b0010_0000;
                        val = (val >> 5);
                        break;
                    case 4:
                        //4 Stellen nach rechts
                        val = Value & 0b0001_0000;
                        val = (val >> 4);
                        break;
                    case 3:
                        //3 Stellen nach rechts
                        val = Value & 0b0000_1000;
                        val = (val >> 3);
                        break;
                    case 2:
                        //2 Stellen nach rechts
                        val = Value & 0b0000_0100;
                        val = (val >> 2);
                        break;
                    case 1:
                        //1 Stellen nach rechts
                        val = Value & 0b0000_0010;
                        val = (val >> 1);
                        break;
                    case 0:
                        //0 Stellen nach rechts
                        val = Value & 0b0000_0001;
                        val = (val >> 0);
                        break;
                    default:
                        val = 0xFFFF;
                        break;
                }
                if(val>0)
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
                switch (index)
                {
                    case 7:
                        if(value)
                        {
                            Value |= 0b1000_0000;
                        }
                        else
                        {
                            Value &= 0b0111_1111;
                        }
                        break;
                    case 6:
                        if (value)
                        {
                            Value |= 0b0100_0000;
                        }
                        else
                        {
                            Value &= 0b1011_1111;
                        }
                        break;
                    case 5:
                        if (value)
                        {
                            Value |= 0b0010_0000;
                        }
                        else
                        {
                            Value &= 0b1101_1111;
                        }
                        break;
                    case 4:
                        if (value)
                        {
                            Value |= 0b0001_0000;
                        }
                        else
                        {
                            Value &= 0b1110_1111;
                        }
                        break;
                    case 3:
                        if (value)
                        {
                            Value |= 0b0000_1000;
                        }
                        else
                        {
                            Value &= 0b1111_0111;
                        }
                        break;
                    case 2:
                        if (value)
                        {
                            Value |= 0b0000_0100;
                        }
                        else
                        {
                            Value &= 0b1111_1011;
                        }
                        break;
                    case 1:
                        if (value)
                        {
                            Value |= 0b0000_0010;
                        }
                        else
                        {
                            Value &= 0b1111_1101;
                        }
                        break;
                    case 0:
                        if (value)
                        {
                            Value |= 0b0000_0001;
                        }
                        else
                        {
                            Value &= 0b1111_1110;
                        }
                        break;
                    default:
                        break;
                }
                RaisePropertyChanged("Bit");
            }
        }*/

        public ItemNotifyByte()
        {
            Value = 0;
        }
    }
}
