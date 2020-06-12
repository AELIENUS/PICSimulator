using Application.Services;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Application.Model
{
    public class Memory : ObservableObject
    {
        #region properties

        private short _CycleCounter;

        public short CycleCounter
        {
            get
            {
                return _CycleCounter;
            }
            set
            {
                _CycleCounter++;
                // Timer0 hochzählen, wenn Pin 4 von Port A nicht als ClockSource ausgewählt wurde
                // wenn T0CS gesetzt ist, dann dann wird PIN4 von Port A als ClockSource genommen.
                //ist T0CS = 0?
                if ((RAM.RAMList[8].Byte1.Value & 0b0010_0000) == 0)
                {
                    RAM.IncTimer0();
                }
                //Laufzeit neu berechnen
                Laufzeit ++;
                RaisePropertyChanged();
            }
        }

        private float _Laufzeit;
        public float Laufzeit //in µs
        {
            get
            {
                _Laufzeit = (CycleCounter*4) / (Quartzfrequenz/1000000); //ein Cycle besteht aus 4 Quarztakten
                return _Laufzeit;
            }
            set
            {
                _Laufzeit = (CycleCounter*4) / (Quartzfrequenz/1000000);
                RaisePropertyChanged();
            }
        }

        private int _Quartzfrequenz = 16000000;
        public int Quartzfrequenz 
        { 
            get
            {
                return _Quartzfrequenz;
            }
            set
            {
                _Quartzfrequenz = value;
                RaisePropertyChanged();
            }
        }

        public int PC
        {
            get
            {
                int temp;
                temp = RAM[Constants.PCL_B1] + (RAM[Constants.PCLATH_B1] << 8);
                return temp;
            }
        }


        private ObservableStack<short> _PCStack;

        public ObservableStack<short> PCStack 
        { 
            get
            {
                if(_PCStack == null)
                {
                    _PCStack = new ObservableStack<short>(new Stack<short>(Constants.PC_STACK_CAPACITY));
                }
                return _PCStack;
            }
            set
            {
                _PCStack = value;
                RaisePropertyChanged();
            }
        }

        private short _W_Reg;

        public short W_Reg
        {
            get
            {
                return _W_Reg;
            }
            set
            {
                if (value == _W_Reg)
                {
                    return;
                }
                _W_Reg = value;
                RaisePropertyChanged();
            }
        }

        private short _Z_Flag;
        public short Z_Flag
        {
            get
            {
                return _Z_Flag;
            }
            set
            {
                if (value == _Z_Flag)
                {
                    return;
                }
                _Z_Flag = value;
                RaisePropertyChanged();
            }
        }

        private short _C_Flag;

        public short C_Flag
        {
            get
            {
                return _C_Flag;
            }
            set
            {
                if (value == _C_Flag)
                {
                    return;
                }
                _C_Flag = value;
                RaisePropertyChanged();
            }
        }

        private short _DC_Flag;
        public short DC_Flag
        {
            get
            {
                return _DC_Flag;
            }
            set
            {
                if (value == _DC_Flag)
                {
                    return;
                }
                _DC_Flag = value;
                RaisePropertyChanged();
            }
        }

        private RAMModel _ram;
        
        public RAMModel RAM
        {
            get
            {
                return _ram;
            }
            set
            {
                if (value.Equals(_ram))
                {
                    return;
                }
                _ram = value;
                RaisePropertyChanged();
            }
        }

        private byte[] _dataEEPROM;

        public byte[] DataEEPROM
        {
            get
            {
                return _dataEEPROM;
            }
            set
            {
                if (value.Equals(_dataEEPROM))
                {
                    return;
                }
                _dataEEPROM = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public Memory()
        {
            RAM = new RAMModel();
            PowerReset();
        }

        public void PowerReset()
        {
            //Initialzustand von RAM wiederherstellen
            //Bank 1
            RAM[Constants.INDF_B1] = 0x00;
            RAM[Constants.TMR0] = 0x00;
            RAM[Constants.PCL_B1] = 0x00;
            RAM[Constants.STATUS_B1] = 0b0001_1000;
            RAM[Constants.FSR_B1] = 0x00;
            RAM[Constants.PORTA] = 0x00;
            RAM[Constants.PORTB] = 0x00;
            RAM[Constants.EEDATA] = 0x00;
            RAM[Constants.EEADR] = 0x00;
            RAM[Constants.PCLATH_B1] = 0x00;
            RAM[Constants.INTCON_B1] = 0x00;
            //Bank2
            RAM[Constants.INDF_B2] = 0x00;
            RAM[Constants.OPTION_REG] = 0xFF;
            RAM[Constants.PCL_B2] = 0x00;
            RAM[Constants.STATUS_B2] = 0b0001_1000;
            RAM[Constants.FSR_B2] = 0x00;
            RAM[Constants.TRISA] = 0xFF;
            RAM[Constants.TRISB] = 0xFF;
            RAM[Constants.EECON1] = 0x00;
            RAM[Constants.EECON2] = 0x00;
            RAM[Constants.PCLATH_B2] = 0x00;
            RAM[Constants.INTCON_B2] = 0x00;
            W_Reg = 0x0000;

            PCStack = new ObservableStack<short>(new Stack<short>(Constants.PC_STACK_CAPACITY));

            Reset_GPR();
            RAM.PrescaleCounter = 1;
            _CycleCounter = 0;
        }

       public void OtherReset()
        {
            //Bank 1
            RAM[Constants.INDF_B1] = 0x00;
            //TMR0 unchanged
            RAM[Constants.PCL_B1] = 0x00;
            //hier gibt es konditionale bits mal noch tiefer nachschauen wie das wann gemacht wird
            RAM[Constants.STATUS_B1] &= 0b0001_1111;
            //FSR unchanged
            //PORTA unchanged
            //PORTB unchanged
            //EEDATA unchanged
            //EEADR unchanged
            RAM[Constants.PCLATH_B1] = 0x00;
            RAM[Constants.INTCON_B1] &= 0x01;
            //Bank2
            RAM[Constants.INDF_B2] = 0x00;
            RAM[Constants.OPTION_REG] = 0xFF;
            RAM[Constants.PCL_B2] = 0x00;
            RAM[Constants.STATUS_B2] &= 0b0001_1111;
            //FSR unchanged
            RAM[Constants.TRISA] = 0xFF;
            RAM[Constants.TRISB] = 0xFF;
            //Bit 4 konditional
            RAM[Constants.EECON1] = 0x00;
            RAM[Constants.EECON2]= 0x00;
            RAM[Constants.PCLATH_B2] = 0x00;
            RAM[Constants.INTCON_B2] &= 0x001;

            //RESETS VON OBEN ÜBERPRÜFEN

            //GPR
            Reset_GPR();
        }

        private void Reset_GPR()
        {
            //GPR 1 zurücksetzen
            for (int i = Constants.GPR_START_B1; i <= Constants.GPR_END_B1; i++)
            {
                RAM[i] = 0x00;
            }

            //GPR 2 zurücksetzen
            for (int i = Constants.GPR_START_B2; i <= Constants.GPR_END_B2; i++)
            {
                RAM[i] = 0x00;
            }
        }
    }
}
