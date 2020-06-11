using Application.Model;
using Applicator.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PortB : ItemNotifyByte
    {
        private Memory memory;


        public PortB(Memory mem)
        {
            memory = mem;
        }

        public bool Pin0
        {
            get
            {
                int temp = (memory.RAM[Constants.PORTB] & 0b0000_0001);
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
                    memory.RAM[Constants.PORTB] |= 0b0000_0001;
                }
                else
                {
                    memory.RAM[Constants.PORTB] &= 0b1111_1110;
                }
                RaisePropertyChanged();
            }
        }

        public bool Pin1
        {
            get
            {
                int temp = (memory.RAM[Constants.PORTB] & 0b0000_0010);
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
                    memory.RAM[Constants.PORTB] |= 0b0000_0010;
                }
                else
                {
                    memory.RAM[Constants.PORTB] &= 0b1111_1101;
                }
                RaisePropertyChanged();
            }
        }

        public bool Pin2
        {
            get
            {
                int temp = (memory.RAM[Constants.PORTB] & 0b0000_0100);
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
                    memory.RAM[Constants.PORTB] |= 0b0000_0100;
                }
                else
                {
                    memory.RAM[Constants.PORTB] &= 0b1111_1011;
                }
                RaisePropertyChanged();
            }
        }

        public bool Pin3
        {
            get
            {
                int temp = (memory.RAM[Constants.PORTB] & 0b0000_1000);
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
                    memory.RAM[Constants.PORTB] |= 0b000_1000;
                }
                else
                {
                    memory.RAM[Constants.PORTB] &= 0b1111_0111;
                }
                RaisePropertyChanged();
            }
        }

        public bool Pin4
        {
            get
            {
                int temp = (memory.RAM[Constants.PORTB] & 0b0001_0000);
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
                    memory.RAM[Constants.PORTB] |= 0b0001_0000;
                }
                else
                {
                    memory.RAM[Constants.PORTB] &= 0b1110_1111;
                }
                RaisePropertyChanged();
            }
        }

        public bool Pin5
        {
            get
            {
                int temp = (memory.RAM[Constants.PORTB] & 0b0010_0000);
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
                    memory.RAM[Constants.PORTB] |= 0b0010_0000;
                }
                else
                {
                    memory.RAM[Constants.PORTB] &= 0b1101_1111;
                }
                RaisePropertyChanged();
            }
        }

        public bool Pin6
        {
            get
            {
                int temp = (memory.RAM[Constants.PORTB] & 0b0100_0000);
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
                    memory.RAM[Constants.PORTB] |= 0b0100_0000;
                }
                else
                {
                    memory.RAM[Constants.PORTB] &= 0b1011_1111;
                }
                RaisePropertyChanged();
            }
        }

        public bool Pin7
        {
            get
            {
                int temp = (memory.RAM[Constants.PORTB] & 0b1000_0000);
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
                    memory.RAM[Constants.PORTB] |= 0b1000_0000;
                }
                else
                {
                    memory.RAM[Constants.PORTB] &= 0b0111_1111;
                }
                RaisePropertyChanged();
            }
        }

    }
}
