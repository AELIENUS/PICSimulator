using Application.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace Application.Model
{
    public class RAMModel : ObservableObject
    {
        #region RAM 
        private ObservableCollection<RAMPortion> _RAMList;
        public ObservableCollection<RAMPortion> RAMList 
        { 
            get
            {
                return _RAMList;
            }
            set
            {
                _RAMList = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        public int Timer0PrescaleRatio
        {
            get
            {
                int temp = RAMList[8].Byte1.Value & 0b0000_0111;
                switch(temp)
                {
                    case 0b000:
                        return 2;
                    case 0b001:
                        return 4;
                    case 0b010:
                        return 8;
                    case 0b011:
                        return 16;
                    case 0b100:
                        return 32;
                    case 0b101:
                        return 64;
                    case 0b110:
                        return 128;
                    case 0b111:
                        return 256;
                    default:
                        //hier sollte man nie rein kommen;
                        return 0;
                }
            }
        }

        public int WatchdogPrescaleRatios
        {
            get
            {
                int temp = RAMList[8].Byte1.Value & 0b0000_0111;
                switch (temp)
                {
                    case 0b000:
                        return 1;
                    case 0b001:
                        return 2;
                    case 0b010:
                        return 4;
                    case 0b011:
                        return 8;
                    case 0b100:
                        return 16;
                    case 0b101:
                        return 32;
                    case 0b110:
                        return 64;
                    case 0b111:
                        return 128;
                    default:
                        //hier sollte man nie rein kommen;
                        return 0;
                }
            }
        }

        private byte _PrescaleCounter;

        public byte PrescaleCounter
        {
            get
            {
                return _PrescaleCounter;
            }
            set
            {
                _PrescaleCounter = value;
            }
        }

        public byte this[int index]
        {
            get
            {
                //Indirekte Addressierung
                if(index == 0x00)
                {
                    index = RAMList[0].Byte4.Value;
                }
                double portionIndexDouble = index / 16;
                int portionIndex = (int)Math.Floor(portionIndexDouble);
                int position = index % 16;
                switch(position)
                {
                    case 0:
                        return RAMList[portionIndex].Byte0.Value;
                    case 1:
                        return RAMList[portionIndex].Byte1.Value;
                    case 2:
                        return RAMList[portionIndex].Byte2.Value;
                    case 3:
                        return RAMList[portionIndex].Byte3.Value;
                    case 4:
                        return RAMList[portionIndex].Byte4.Value;
                    case 5:
                        return RAMList[portionIndex].Byte5.Value;
                    case 6:
                        return RAMList[portionIndex].Byte6.Value;
                    case 7:
                        return RAMList[portionIndex].Byte7.Value;
                    case 8:
                        return RAMList[portionIndex].Byte8.Value;
                    case 9:
                        return RAMList[portionIndex].Byte9.Value;
                    case 10:
                        return RAMList[portionIndex].Byte10.Value;
                    case 11:
                        return RAMList[portionIndex].Byte11.Value;
                    case 12:
                        return RAMList[portionIndex].Byte12.Value;
                    case 13:
                        return RAMList[portionIndex].Byte13.Value;
                    case 14:
                        return RAMList[portionIndex].Byte14.Value;
                    case 15:
                        return RAMList[portionIndex].Byte15.Value;
                    default:
                        return 0;
                }
            }
            set
            {
                //Indirekte Adressierung
                if(index == 0x00)
                {
                    index = RAMList[0].Byte4.Value;
                }
                double portionIndexDouble = index / 16;
                int portionIndex = (int)Math.Floor(portionIndexDouble);
                int position = index % 16;
                switch (position)
                {
                    case 0:
                        RAMList[portionIndex].Byte0.Value = value;
                        break;
                    case 1:
                        RAMList[portionIndex].Byte1.Value = value;
                        break;
                    case 2:
                        RAMList[portionIndex].Byte2.Value = value;
                        break;
                    case 3:
                        RAMList[portionIndex].Byte3.Value = value;
                        break;
                    case 4:
                        RAMList[portionIndex].Byte4.Value = value;
                        break;
                    case 5:
                        if(portionIndex == 0)
                        {
                            SetPortA(value);
                        }
                        else
                        {
                            RAMList[portionIndex].Byte5.Value = value;
                        }
                        break;
                    case 6:
                        RAMList[portionIndex].Byte6.Value = value;
                        break;
                    case 7:
                        RAMList[portionIndex].Byte7.Value = value;
                        break;
                    case 8:
                        RAMList[portionIndex].Byte8.Value = value;
                        break;
                    case 9:
                        RAMList[portionIndex].Byte9.Value = value;
                        break;
                    case 10:
                        RAMList[portionIndex].Byte10.Value = value;
                        break;
                    case 11:
                        RAMList[portionIndex].Byte11.Value = value;
                        break;
                    case 12:
                        RAMList[portionIndex].Byte12.Value = value;
                        break;
                    case 13:
                        RAMList[portionIndex].Byte13.Value = value;
                        break;
                    case 14:
                        RAMList[portionIndex].Byte14.Value = value;
                        break;
                    case 15:
                        RAMList[portionIndex].Byte15.Value = value;
                        break;
                }
            }

        }

        public void IncTimer0()
        {
            if((RAMList[8].Byte1.Value & 0b0000_1000) == 0)
            {
                IncTimer0WithPrescaler();
            }
            else
            {
                IncTimer0WithoutPrescaler();
            }
        }

        private void IncTimer0WithPrescaler()
        {
            //ist Overflow?
            if (RAMList[0].Byte1.Value == 255
                && PrescaleCounter == Timer0PrescaleRatio)
            {
                PrescaleCounter = 0;
                Timer0Overflow();
            }
            else
            {
                if(PrescaleCounter == Timer0PrescaleRatio)
                {
                    RAMList[0].Byte1.Value++;
                    PrescaleCounter = 0;
                }
                else
                {
                    PrescaleCounter++;
                }
            }
        }

        private void IncTimer0WithoutPrescaler()
        {
            //ist Overflow?
            if (RAMList[0].Byte1.Value == 255)
            {
                Timer0Overflow();
            }
            else
            {
                RAMList[0].Byte1.Value++;
            }
        }

        private void Timer0Overflow()
        {
            RAMList[0].Byte1.Value = 0;
            //Ist GIE und T0IE an?
            if ((RAMList[0].Byte11.Value & 0b1010_0000) >= 0b1010_0000)
            {
                //T0IF setzen
                RAMList[0].Byte11.Value |= 0b0000_0100;
                RAMList[8].Byte11.Value |= 0b0000_0100;
            }
            //INTF an
            RAMList[0].Byte11.Value |= 0b0000_0010;
            RAMList[8].Byte11.Value |= 0b0000_0010;
        }

        private void SetPortA(object valueToSet)
        {
            byte val = (byte)valueToSet;
            //PortA PIN 4 ist source von Timer0 
            if ((RAMList[8].Byte1.Value & 0b0010_0000) > 0)
            {
                if((RAMList[8].Byte1.Value & 0b0100_0000) == 0)
                {
                    //steigende Flanke
                    if(((val&0b0001_0000)>0)
                        && ((RAMList[0].Byte5.Value & 0b0001_0000) == 0))
                    {
                        IncTimer0();
                    }
                }
                else
                {
                    //fallende Flanke
                    if (((val & 0b0001_0000) == 0)
                        && ((RAMList[0].Byte5.Value & 0b0001_0000) > 0))
                    {
                        IncTimer0();
                    }
                }
                RAMList[0].Byte5.Value = val;
            }
            else
            {
                RAMList[0].Byte5.Value = val;
            }
        }
            

        public RAMModel()
        {
            _RAMList = new ObservableCollection<RAMPortion>(new RAMPortion[16]);
            for (int i = 0; i < 16; i++)
            {
                _RAMList[i] = new RAMPortion();
            }
        }
    }
}