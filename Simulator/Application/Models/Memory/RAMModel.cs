
using Application.Models.CustomDatastructures;
using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;

namespace Application.Models.Memory
{
    public class RAMModel : ObservableObject
    {
        #region Fields
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

        private Port _PortA;
        public Port PortA
        {
            get
            {
                return _PortA;
            }
            set
            {
                if (_PortA.Equals(value))
                {
                    return;
                }
                _PortA = value;
                RaisePropertyChanged();
            }
        }

        private Port _PortB;

        public Port PortB
        {
            get
            {
                return _PortB;
            }
            set
            {
                if (_PortB.Equals(value))
                {
                    return;
                }
                _PortB = value;
                RaisePropertyChanged();
            }
        }

        private bool _PCWasJump;

        public bool PCWasJump
        {
            get
            {
                return _PCWasJump;
            }
            set
            {
                _PCWasJump = value;
                RaisePropertyChanged();
            }
        }

        private bool _PCLWasManipulated;

        public bool PCLWasManipulated
        {
            get
            {
                return _PCLWasManipulated;
            }
            set
            {
                _PCLWasManipulated = value;

            }
        }

        public int PCJumpAdress;

        public int PCWithClear
        {
            get
            {

                if (PCLWasManipulated == true)
                {
                    PCLWasManipulated = false;
                    return RAMList[0].Byte2.Value + (RAMList[0].Byte10.Value << 8);
                }
                else if (PCWasJump == true)
                {
                    PCWasJump = false;
                    return PCJumpAdress;
                }
                else
                {
                    return RAMList[0].Byte2.Value;
                }
            }
        }
        public byte PCL
        {
            get
            {
                return RAMList[0].Byte2.Value;
            }
        }

        public byte PCLATH
        {
            get
            {
                return RAMList[0].Byte10.Value;
            }
        }

        public int PCWithoutClear
        {
            get
            {
                if (PCLWasManipulated == true)
                {
                    return RAMList[0].Byte2.Value + (RAMList[0].Byte10.Value << 8);
                }
                else if (PCWasJump == true)
                {
                    return PCJumpAdress;
                }
                else
                {
                    return RAMList[0].Byte2.Value;
                }
            }
        }

        public int ZFlag
        {
            get
            {
                return (RAMList[0].Byte3.Value & 0b_0000_0100) >>2;
            }
        }

        public int CFlag
        {
            get
            {
                return (RAMList[0].Byte3.Value & 0b_0000_0001);
            }
        }

        public int DCFlag
        {
            get
            {
                return (RAMList[0].Byte3.Value & 0b_0000_0010) >> 1;
            }
        }
        #endregion

        public int Timer0PrescaleRatio
        {
            get
            {
                int bytesInQuestion = RAMList[8].Byte1.Value & 0b0000_0111;
                switch(bytesInQuestion)
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
                int bytesInQuestion = RAMList[8].Byte1.Value & 0b0000_0111;
                switch (bytesInQuestion)
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

        /// <summary>
        /// accessor for any location in the memory of the PIC
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public byte this[int index]
        {
            get
            {
                //wenn Status, rp0=1 -> Bank 1
                if ((RAMList[0].Byte3.Value & 0b0010_0000) >0)
                {
                    index += 0x80;
                } 
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
                //wenn Status, rp0=1 -> Bank 1
                if ((RAMList[0].Byte3.Value & 0b0010_0000) > 0)
                {
                    index += 0x80;
                }
                //Indirekte Adressierung
                if (index == 0x00)
                {
                    index = RAMList[0].Byte4.Value;
                }
                double portionIndexDouble = index / 16;
                int portionIndex = (int)Math.Floor(portionIndexDouble);
                int position = index % 16;
                switch (position)
                {
                    case 0:
                        //byte 0 portion 0 and 8 is available in both banks
                        if (portionIndex == 0)
                        {
                            RAMList[8].Byte0.Value = value;
                        }
                        if(portionIndex == 8)
                        {
                            RAMList[0].Byte0.Value = value;
                        }
                        RAMList[portionIndex].Byte0.Value = value;
                        break;
                    case 1:
                        if(portionIndex == 8)
                        {
                            if((RAMList[portionIndex].Byte1.Value & 0b0000_1111)!=(value & 0b0000_1111))
                            {
                                PrescaleCounter = 1;
                            }
                        }
                        RAMList[portionIndex].Byte1.Value = value;
                        break;
                    case 2:
                        if (portionIndex == 0)
                        {
                            RAMList[8].Byte2.Value = value;
                            RaisePropertyChanged("PCWithClear");
                            RaisePropertyChanged("PCWithoutClear");
                            RaisePropertyChanged("PCL");
                        }
                        if (portionIndex == 8)
                        {
                            RAMList[0].Byte2.Value = value;
                            RaisePropertyChanged("PCWithClear");
                            RaisePropertyChanged("PCWithoutClear");
                            RaisePropertyChanged("PCL");
                        }
                        RAMList[portionIndex].Byte2.Value = value;
                        break;
                    case 3:
                        if (portionIndex == 0)
                        {
                            RAMList[8].Byte3.Value = value;
                            RaisePropertyChanged("ZFlag");
                            RaisePropertyChanged("CFlag");
                            RaisePropertyChanged("DCFlag");
                        }
                        if (portionIndex == 8)
                        {
                            RAMList[0].Byte3.Value = value;
                            RaisePropertyChanged("ZFlag");
                            RaisePropertyChanged("CFlag");
                            RaisePropertyChanged("DCFlag");
                        }
                        RAMList[portionIndex].Byte3.Value = value;
                        break;
                    case 4:
                        //byte 4 portion 0 and 8 is the same in both banks
                        if (portionIndex == 0)
                        {
                            RAMList[8].Byte4.Value = value;
                        }
                        if (portionIndex == 8)
                        {
                            RAMList[0].Byte4.Value = value;
                        }
                        RAMList[portionIndex].Byte4.Value = value;
                        break;
                    case 5:
                        RAMList[portionIndex].Byte5.Value = value;
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
                        if (portionIndex == 0)
                        {
                            RAMList[8].Byte10.Value = value;
                            RaisePropertyChanged("PCWithClear");
                            RaisePropertyChanged("PCWithoutClear");
                            RaisePropertyChanged("PCLATH");
                        }
                        if (portionIndex == 8)
                        {
                            RAMList[0].Byte10.Value = value;
                            RaisePropertyChanged("PCWithClear");
                            RaisePropertyChanged("PCWithoutClear");
                            RaisePropertyChanged("PCLATH");
                        }
                        RAMList[portionIndex].Byte10.Value = value;
                        break;
                    case 11:
                        if (portionIndex == 0)
                        {
                            RAMList[8].Byte11.Value = value;
                        }
                        if (portionIndex == 8)
                        {
                            RAMList[0].Byte11.Value = value;
                        }
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

        #region Timer
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
                PrescaleCounter = 1;
                Timer0Overflow();
            }
            else
            {
                if(PrescaleCounter >= Timer0PrescaleRatio)
                {
                    RAMList[0].Byte1.Value++;
                    PrescaleCounter = 1;
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
            //T0IF setzen
            RAMList[0].Byte11.Value |= 0b0000_0100;
            RAMList[8].Byte11.Value |= 0b0000_0100;
        }
        #endregion

        public RAMModel(Port portA, Port portB)
        {
            _RAMList = new ObservableCollection<RAMPortion>(new RAMPortion[16]);
            for (int i = 0; i < 16; i++)
            {
                _RAMList[i] = new RAMPortion();
            }
            _PortA = new Port();
            _PortB = new Port();
            _RAMList[0].Byte5 = _PortA.PortValue;
            _RAMList[8].Byte5 = _PortA.TRISValue; 
            _RAMList[0].Byte6 = _PortB.PortValue;
            _RAMList[8].Byte6 = _PortB.TRISValue;
        }
    }
}