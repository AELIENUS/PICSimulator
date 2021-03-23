﻿
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

        private PortA _PortA;
        public PortA PortA
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

        private PortB _PortB;

        public PortB PortB
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

        private bool _PC_was_Jump;

        public bool PC_was_Jump
        {
            get
            {
                return _PC_was_Jump;
            }
            set
            {
                _PC_was_Jump = value;
                RaisePropertyChanged();
            }
        }

        private bool _PCL_was_Manipulated;

        public bool PCL_was_Manipulated
        {
            get
            {
                return _PCL_was_Manipulated;
            }
            set
            {
                _PCL_was_Manipulated = value;

            }
        }

        public int PC_JumpAdress;

        public int PC_With_Clear
        {
            get
            {

                if (PCL_was_Manipulated == true)
                {
                    PCL_was_Manipulated = false;
                    return RAMList[0].Byte2.Value + (RAMList[0].Byte10.Value << 8);
                }
                else if (PC_was_Jump == true)
                {
                    PC_was_Jump = false;
                    return PC_JumpAdress;
                }
                else
                {
                    return RAMList[0].Byte2.Value;
                }
            }
        }

        public int PC_Without_Clear
        {
            get
            {
                if (PCL_was_Manipulated == true)
                {
                    return RAMList[0].Byte2.Value + (RAMList[0].Byte10.Value << 8);
                }
                else if (PC_was_Jump == true)
                {
                    return PC_JumpAdress;
                }
                else
                {
                    return RAMList[0].Byte2.Value;
                }
            }
        }

        public int Z_Flag
        {
            get
            {
                return (RAMList[0].Byte3.Value & 0b_0000_0100) >>2;
            }
        }

        public int C_Flag
        {
            get
            {
                return (RAMList[0].Byte3.Value & 0b_0000_0001);
            }
        }

        public int DC_Flag
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
                        if(portionIndex == 0)
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
                            RaisePropertyChanged("PC_With_Clear");
                            RaisePropertyChanged("PC_Without_Clear");
                        }
                        if (portionIndex == 8)
                        {
                            RAMList[0].Byte2.Value = value;
                            RaisePropertyChanged("PC_With_Clear");
                            RaisePropertyChanged("PC_Without_Clear");
                        }
                        RAMList[portionIndex].Byte2.Value = value;
                        break;
                    case 3:
                        if (portionIndex == 0)
                        {
                            RAMList[8].Byte3.Value = value;
                            RaisePropertyChanged("Z_Flag");
                            RaisePropertyChanged("C_Flag");
                            RaisePropertyChanged("DC_Flag");
                        }
                        if (portionIndex == 8)
                        {
                            RAMList[0].Byte3.Value = value;
                            RaisePropertyChanged("Z_Flag");
                            RaisePropertyChanged("C_Flag");
                            RaisePropertyChanged("DC_Flag");
                        }
                        RAMList[portionIndex].Byte3.Value = value;
                        break;
                    case 4:
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
                        if (portionIndex == 0)
                        {
                            SetPortA(value);
                        }
                        else if (portionIndex == 8)
                        {
                            SetTrisA(value);
                        }
                        else
                        {
                            RAMList[portionIndex].Byte5.Value = value;
                        }
                        break;
                    case 6:
                        if(portionIndex == 0)
                        {
                            SetPortB(value);
                        }
                        else if(portionIndex == 8)
                        {
                            SetTrisB(value);
                        }
                        else
                        {
                            RAMList[portionIndex].Byte6.Value = value;
                        }
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
                            RaisePropertyChanged("PC_With_Clear");
                            RaisePropertyChanged("PC_Without_Clear");
                        }
                        if (portionIndex == 8)
                        {
                            RAMList[0].Byte10.Value = value;
                            RaisePropertyChanged("PC_With_Clear");
                            RaisePropertyChanged("PC_Without_Clear");
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
        #region SetPort
        private void SetPortA(byte valueToSet)
        {
            //TRISA[0] auf Ausgang?
            if ((RAMList[8].Byte5.Value & 0b0000_0001) == 0)
            {
                if((valueToSet&0b0000_0001)==0)
                {
                    RAMList[0].Byte5.Value &= 0b1111_1110;
                }
                else
                {
                    RAMList[0].Byte5.Value |= 0b0000_0001;
                }
            }
            else
            {
                if ((valueToSet & 0b0000_0001) == 0)
                {
                    PortA.PORTA_Latch &= 0b1111_1110;
                }
                else
                {
                    PortA.PORTA_Latch |= 0b0000_0001;
                }
            }
            //TRISA[1] auf Ausgang
            if ((RAMList[8].Byte5.Value & 0b0000_0010) == 0)
            {
                if ((valueToSet & 0b0000_0010) == 0)
                {
                    RAMList[0].Byte5.Value &= 0b1111_1101;
                }
                else
                {
                    RAMList[0].Byte5.Value |= 0b0000_0010;
                }
            }
            else
            {
                if ((valueToSet & 0b0000_0010) == 0)
                {
                    PortA.PORTA_Latch &= 0b1111_1101;
                }
                else
                {
                    PortA.PORTA_Latch |= 0b0000_0010;
                }
            }
            //TRISA[2] auf Ausgang
            if ((RAMList[8].Byte5.Value & 0b0000_0100) == 0)
            {
                if ((valueToSet & 0b0000_0100) == 0)
                {
                    RAMList[0].Byte5.Value &= 0b1111_1011;
                }
                else
                {
                    RAMList[0].Byte5.Value |= 0b0000_0100;
                }
            }
            else
            {
                if ((valueToSet & 0b0000_0100) == 0)
                {
                    PortA.PORTA_Latch &= 0b1111_1011;
                }
                else
                {
                    PortA.PORTA_Latch |= 0b0000_0100;
                }
            }
            //TRISA[3] auf Ausgang
            if ((RAMList[8].Byte5.Value & 0b0000_1000) == 0)
            {
                if ((valueToSet & 0b0000_1000) == 0)
                {
                    RAMList[0].Byte5.Value &= 0b1111_0111;
                }
                else
                {
                    RAMList[0].Byte5.Value |= 0b0000_1000;
                }
            }
            else
            {
                if ((valueToSet & 0b0000_1000) == 0)
                {
                    PortA.PORTA_Latch &= 0b1111_0111;
                }
                else
                {
                    PortA.PORTA_Latch |= 0b0000_1000;
                }
            }
            //TRISA[4] auf Ausgang
            if ((RAMList[8].Byte5.Value & 0b0001_0000) == 0)
            {
                if ((valueToSet & 0b0001_0000) == 0)
                {
                    RAMList[0].Byte5.Value &= 0b1110_1111;
                }
                else
                {
                    RAMList[0].Byte5.Value |= 0b0001_0000;
                }
            }
            else
            {
                if ((valueToSet & 0b0001_0000) == 0)
                {
                    PortA.PORTA_Latch &= 0b1110_1111;
                }
                else
                {
                    PortA.PORTA_Latch |= 0b0001_0000;
                }
            }
            //TRISA[5] auf Ausgang
            if ((RAMList[8].Byte5.Value & 0b0010_0000) == 0)
            {
                if ((valueToSet & 0b0010_0000) == 0)
                {
                    RAMList[0].Byte5.Value &= 0b1101_1111;
                }
                else
                {
                    RAMList[0].Byte5.Value |= 0b0010_0000;
                }
            }
            else
            {
                if ((valueToSet & 0b0010_0000) == 0)
                {
                    PortA.PORTA_Latch &= 0b1101_1111;
                }
                else
                {
                    PortA.PORTA_Latch |= 0b0010_0000;
                }
            }
            //TRISA[6] auf Ausgang
            if ((RAMList[8].Byte5.Value & 0b0100_0000) == 0)
            {
                if ((valueToSet & 0b0100_0000) == 0)
                {
                    RAMList[0].Byte5.Value &= 0b1011_1111;
                }
                else
                {
                    RAMList[0].Byte5.Value |= 0b0100_0000;
                }
            }
            else
            {
                if ((valueToSet & 0b0100_0000) == 0)
                {
                    PortA.PORTA_Latch &= 0b1011_1111;
                }
                else
                {
                    PortA.PORTA_Latch |= 0b0100_0000;
                }
            }
            //TRISA[7] auf Ausgang
            if ((RAMList[8].Byte5.Value & 0b1000_0000) == 0)
            {
                if ((valueToSet & 0b1000_0000) == 0)
                {
                    RAMList[0].Byte5.Value &= 0b0111_1111;
                }
                else
                {
                    RAMList[0].Byte5.Value |= 0b1000_0000;
                }
            }
            else
            {
                if ((valueToSet & 0b1000_0000) == 0)
                {
                    PortA.PORTA_Latch &= 0b0111_1111;
                }
                else
                {
                    PortA.PORTA_Latch |= 0b1000_0000;
                }
            }
            //RaisePropertyChanged
            PortA.RaisePropertyChanged("Value");
            PortA.RaisePropertyChanged("Pin0");
            PortA.RaisePropertyChanged("Pin1");
            PortA.RaisePropertyChanged("Pin2");
            PortA.RaisePropertyChanged("Pin3");
            PortA.RaisePropertyChanged("Pin4");
            PortA.RaisePropertyChanged("Pin5");
            PortA.RaisePropertyChanged("Pin6");
            PortA.RaisePropertyChanged("Pin7");
        }

        private void SetPortB(byte valueToSet)
        {
            //TRISB[0] auf Ausgang?
            if ((RAMList[8].Byte6.Value & 0b0000_0001) == 0)
            {
                if ((valueToSet & 0b0000_0001) == 0)
                {
                    RAMList[0].Byte6.Value &= 0b1111_1110;
                }
                else
                {
                    RAMList[0].Byte6.Value |= 0b0000_0001;
                }
            }
            else
            {
                if ((valueToSet & 0b0000_0001) == 0)
                {
                    PortB.PORTB_Latch &= 0b1111_1110;
                }
                else
                {
                    PortB.PORTB_Latch |= 0b0000_0001;
                }
            }
            //TRISB[1] auf Ausgang
            if ((RAMList[8].Byte6.Value & 0b0000_0010) == 0)
            {
                if ((valueToSet & 0b0000_0010) == 0)
                {
                    RAMList[0].Byte6.Value &= 0b1111_1101;
                }
                else
                {
                    RAMList[0].Byte6.Value |= 0b0000_0010;
                }
            }
            else
            {
                if ((valueToSet & 0b0000_0010) == 0)
                {
                    PortB.PORTB_Latch &= 0b1111_1101;
                }
                else
                {
                    PortB.PORTB_Latch |= 0b0000_0010;
                }
            }

            //TRISB[2] auf Ausgang
            if ((RAMList[8].Byte6.Value & 0b0000_0100) == 0)
            {
                if ((valueToSet & 0b0000_0100) == 0)
                {
                    RAMList[0].Byte6.Value &= 0b1111_1011;
                }
                else
                {
                    RAMList[0].Byte6.Value |= 0b0000_0100;
                }
            }
            else
            {
                if ((valueToSet & 0b0000_0100) == 0)
                {
                    PortB.PORTB_Latch &= 0b1111_1011;
                }
                else
                {
                    PortB.PORTB_Latch |= 0b0000_0100;
                }
            }

            //TRISB[3] auf Ausgang
            if ((RAMList[8].Byte6.Value & 0b0000_1000) == 0)
            {
                if ((valueToSet & 0b0000_1000) == 0)
                {
                    RAMList[0].Byte6.Value &= 0b1111_0111;
                }
                else
                {
                    RAMList[0].Byte6.Value |= 0b0000_1000;
                }
            }
            else
            {
                if ((valueToSet & 0b0000_1000) == 0)
                {
                    PortB.PORTB_Latch &= 0b1111_0111;
                }
                else
                {
                    PortB.PORTB_Latch |= 0b0000_1000;
                }
            }

            //TRISB[4] auf Ausgang
            if ((RAMList[8].Byte6.Value & 0b0001_0000) == 0)
            {
                if ((valueToSet & 0b0001_0000) == 0)
                {
                    RAMList[0].Byte6.Value &= 0b1110_1111;
                }
                else
                {
                    RAMList[0].Byte6.Value |= 0b0001_0000;
                }
            }
            else
            {
                if ((valueToSet & 0b0001_0000) == 0)
                {
                    PortB.PORTB_Latch &= 0b1110_1111;
                }
                else
                {
                    PortB.PORTB_Latch |= 0b0001_0000;
                }
            }

            //TRISB[5] auf Ausgang
            if ((RAMList[8].Byte6.Value & 0b0010_0000) == 0)
            {
                if ((valueToSet & 0b0010_0000) == 0)
                {
                    RAMList[0].Byte6.Value &= 0b1101_1111;
                }
                else
                {
                    RAMList[0].Byte6.Value |= 0b0010_0000;
                }
            }
            else
            {
                if ((valueToSet & 0b0010_0000) == 0)
                {
                    PortB.PORTB_Latch &= 0b1101_1111;
                }
                else
                {
                    PortB.PORTB_Latch |= 0b0010_0000;
                }
            }

            //TRISB[6] auf Ausgang
            if ((RAMList[8].Byte6.Value & 0b0100_0000) == 0)
            {
                if ((valueToSet & 0b0100_0000) == 0)
                {
                    RAMList[0].Byte6.Value &= 0b1011_1111;
                }
                else
                {
                    RAMList[0].Byte6.Value |= 0b0100_0000;
                }
            }
            else
            {
                if ((valueToSet & 0b0100_0000) == 0)
                {
                    PortB.PORTB_Latch &= 0b1011_1111;
                }
                else
                {
                    PortB.PORTB_Latch |= 0b0100_0000;
                }
            }

            //TRISB[7] auf Ausgang
            if ((RAMList[8].Byte6.Value & 0b1000_0000) == 0)
            {
                if ((valueToSet & 0b1000_0000) == 0)
                {
                    RAMList[0].Byte6.Value &= 0b0111_1111;
                }
                else
                {
                    RAMList[0].Byte6.Value |= 0b1000_0000;
                }
            }
            else
            {
                if ((valueToSet & 0b1000_0000) == 0)
                {
                    PortB.PORTB_Latch &= 0b0111_1111;
                }
                else
                {
                    PortB.PORTB_Latch |= 0b1000_0000;
                }
            }

            //RaisePropertyChanged
            PortB.RaisePropertyChanged("Value");
            PortB.RaisePropertyChanged("Pin0");
            PortB.RaisePropertyChanged("Pin1");
            PortB.RaisePropertyChanged("Pin2");
            PortB.RaisePropertyChanged("Pin3");
            PortB.RaisePropertyChanged("Pin4");
            PortB.RaisePropertyChanged("Pin5");
            PortB.RaisePropertyChanged("Pin6");
            PortB.RaisePropertyChanged("Pin7");
        }
        #endregion
        #region TRIS
        private void SetTrisB(byte valueToSet)
        {
            //Pin0
            if((valueToSet & 0b0000_0001) == 0 && (RAMList[8].Byte6.Value & 0b0000_0001) > 0)
            {
                if ((PortB.PORTB_Latch & 0b0000_0001) == 0)
                {
                    RAMList[0].Byte6.Value &= 0b1111_1110;
                }
                else
                {
                    RAMList[0].Byte6.Value |= 0b0000_0001;
                }
            }
            //Pin1
            if ((valueToSet & 0b0000_0010) == 0 && (RAMList[8].Byte6.Value & 0b0000_0010) > 0)
            {
                if ((PortB.PORTB_Latch & 0b0000_0010) == 0)
                {
                    RAMList[0].Byte6.Value &= 0b1111_1101;
                }
                else
                {
                    RAMList[0].Byte6.Value |= 0b0000_0010;
                }
            }
            //Pin2
            if ((valueToSet & 0b0000_0100) == 0 && (RAMList[8].Byte6.Value & 0b0000_0100) > 0)
            {
                if ((PortB.PORTB_Latch & 0b0000_0100) == 0)
                {
                    RAMList[0].Byte6.Value &= 0b1111_1011;
                }
                else
                {
                    RAMList[0].Byte6.Value |= 0b0000_0100;
                }
            }
            //Pin3
            if ((valueToSet & 0b0000_1000) == 0 && (RAMList[8].Byte6.Value & 0b0000_1000) > 0)
            {
                if ((PortB.PORTB_Latch & 0b0000_1000) == 0)
                {
                    RAMList[0].Byte6.Value &= 0b1111_0111;
                }
                else
                {
                    RAMList[0].Byte6.Value |= 0b0000_1000;
                }
            }
            //Pin4
            if ((valueToSet & 0b0001_0000) == 0 && (RAMList[8].Byte6.Value & 0b0001_0000) > 0)
            {
                if ((PortB.PORTB_Latch & 0b0001_0000) == 0)
                {
                    RAMList[0].Byte6.Value &= 0b1110_1111;
                }
                else
                {
                    RAMList[0].Byte6.Value |= 0b0001_0000;
                }
            }
            //Pin5
            if ((valueToSet & 0b0010_0000) == 0 && (RAMList[8].Byte6.Value & 0b0010_0000) > 0)
            {
                if ((PortB.PORTB_Latch & 0b0010_0000) == 0)
                {
                    RAMList[0].Byte6.Value &= 0b1101_1111;
                }
                else
                {
                    RAMList[0].Byte6.Value |= 0b0010_0000;
                }
            }
            //Pin6
            if ((valueToSet & 0b0100_0000) == 0 && (RAMList[8].Byte6.Value & 0b0100_0000) > 0)
            {
                if ((PortB.PORTB_Latch & 0b0100_0000) == 0)
                {
                    RAMList[0].Byte6.Value &= 0b1011_1111;
                }
                else
                {
                    RAMList[0].Byte6.Value |= 0b0100_0000;
                }
            }
            //Pin7
            if ((valueToSet & 0b1000_0000) == 0 && (RAMList[8].Byte6.Value & 0b1000_0000) > 0)
            {
                if ((PortB.PORTB_Latch & 0b1000_0000) == 0)
                {
                    RAMList[0].Byte6.Value &= 0b0111_1111;
                }
                else
                {
                    RAMList[0].Byte6.Value |= 0b1000_0000;
                }
            }

            RAMList[8].Byte6.Value = valueToSet;
        }

        private void SetTrisA(byte valueToSet)
        {
            //Pin0
            if ((valueToSet & 0b0000_0001) == 0 && (RAMList[8].Byte5.Value & 0b0000_0001) > 0)
            {
                if ((PortA.PORTA_Latch & 0b0000_0001) == 0)
                {
                    RAMList[0].Byte5.Value &= 0b1111_1110;
                }
                else
                {
                    RAMList[0].Byte5.Value |= 0b0000_0001;
                }
            }
            //Pin1
            if ((valueToSet & 0b0000_0010) == 0 && (RAMList[8].Byte5.Value & 0b0000_0010) > 0)
            {
                if ((PortA.PORTA_Latch & 0b0000_0010) == 0)
                {
                    RAMList[0].Byte5.Value &= 0b1111_1101;
                }
                else
                {
                    RAMList[0].Byte5.Value |= 0b0000_0010;
                }
            }
            //Pin2
            if ((valueToSet & 0b0000_0100) == 0 && (RAMList[8].Byte5.Value & 0b0000_0100) > 0)
            {
                if ((PortA.PORTA_Latch & 0b0000_0100) == 0)
                {
                     RAMList[0].Byte5.Value &= 0b1111_1011;
                }
                else
                {
                    RAMList[0].Byte5.Value |= 0b0000_0100;
                }
            }
            //Pin3
            if ((valueToSet & 0b0000_1000) == 0 && (RAMList[8].Byte5.Value & 0b0000_1000) > 0)
            {
                if ((PortA.PORTA_Latch & 0b0000_1000) == 0)
                {
                    RAMList[0].Byte5.Value &= 0b1111_0111;
                }
                else
                {
                    RAMList[0].Byte5.Value |= 0b0000_1000;
                }
            }
            //Pin4
            if ((valueToSet & 0b0001_0000) == 0 && (RAMList[8].Byte5.Value & 0b0001_0000) > 0)
            {
                if ((PortA.PORTA_Latch & 0b0001_0000) == 0)
                {
                    RAMList[0].Byte5.Value &= 0b1110_1111;
                }
                else
                {
                    RAMList[0].Byte5.Value |= 0b0001_0000;
                }
            }
            //Pin5
            if ((valueToSet & 0b0010_0000) == 0 && (RAMList[8].Byte5.Value & 0b0010_0000) > 0)
            {
                if ((PortA.PORTA_Latch & 0b0010_0000) == 0)
                {
                    RAMList[0].Byte5.Value &= 0b1101_1111;
                }
                else
                {
                    RAMList[0].Byte5.Value |= 0b0010_0000;
                }
            }
            //Pin6
            if ((valueToSet & 0b0100_0000) == 0 && (RAMList[8].Byte5.Value & 0b0100_0000) > 0)
            {
                if ((PortA.PORTA_Latch & 0b0100_0000) == 0)
                {
                    RAMList[0].Byte5.Value &= 0b1011_1111;
                }
                else
                {
                    RAMList[0].Byte5.Value |= 0b0100_0000;
                }
            }
            //Pin7
            if ((valueToSet & 0b1000_0000) == 0 && (RAMList[8].Byte5.Value & 0b1000_0000) > 0)
            {
                if ((PortA.PORTA_Latch & 0b1000_0000) == 0)
                {
                    RAMList[0].Byte5.Value &= 0b0111_1111;
                }
                else
                {
                    RAMList[0].Byte5.Value |= 0b1000_0000;
                }
            }

            RAMList[8].Byte5.Value = valueToSet;
        }
        #endregion

        public RAMModel()
        {
            _RAMList = new ObservableCollection<RAMPortion>(new RAMPortion[16]);
            for (int i = 0; i < 16; i++)
            {
                _RAMList[i] = new RAMPortion();
            }
            _PortA = new PortA(this);
            _PortB = new PortB(this);
            _RAMList[0].Byte5 = new PortADummy(_PortA);
            _RAMList[0].Byte6 = new PortBDummy(_PortB);
        }
    }
}