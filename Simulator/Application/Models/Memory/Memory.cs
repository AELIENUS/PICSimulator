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

        private bool _isIsr = false;
        public bool IsISR
        {
            get => _isIsr;
            set => _isIsr = value;
        }

        private double _cycleCounter;
        public double CycleCounter
        {
            get => _cycleCounter;
            set
            {
                _cycleCounter++;
                // Timer0 hochzählen, wenn Pin 4 von Port A nicht als ClockSource ausgewählt wurde
                // wenn T0CS gesetzt ist, dann dann wird PIN4 von Port A als ClockSource genommen.
                //ist T0CS = 0?
                if ((RAM.RAMList[8].Byte1.Value & 0b0010_0000) == 0)
                {
                    RAM.IncTimer0();
                }
                //Runtime neu berechnen
                Runtime ++;
                RaisePropertyChanged();
            }
        }

        private double _runtime;
        public double Runtime //in µs
        {
            get
            {
                _runtime = (CycleCounter*4) / (Quartz/1000000) ; //ein Cycle besteht aus 4 Quarztakten
                return _runtime;
            }
            set
            {
                _runtime = (CycleCounter*4) / (Quartz/1000000) ;
                RaisePropertyChanged();
            }
        }

        private double _quartz = 16000000;
        public double Quartz 
        { 
            get => _quartz;
            set
            {
                _quartz = value;
                RaisePropertyChanged();
            }
        }

        private ObservableStack<short> _pcStack;
        public ObservableStack<short> PCStack 
        { 
            get
            {
                return _pcStack ??
                       (_pcStack = new ObservableStack<short>(new Stack<short>(Constants.PC_STACK_CAPACITY)));
            }
            set
            {
                _pcStack = value;
                RaisePropertyChanged();
            }
        }

        private short _wReg;
        public short WReg
        {
            get => _wReg;
            set
            {
                if (value == _wReg)
                {
                    return;
                }
                _wReg = value;
                RaisePropertyChanged();
            }
        }

        private RAMModel _ram;
        public RAMModel RAM
        {
            get => _ram;
            set
            {
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
            WReg = 0x0000;

            PCStack = new ObservableStack<short>(new Stack<short>(Constants.PC_STACK_CAPACITY));

            RAM.PortA.Value = 0;
            RAM.PortB.Value = 0;

            RAM.PortA.PORTA_Latch = 0;
            RAM.PortB.PORTB_Latch = 0;

            RAM.PCL_was_Manipulated = false;
            RAM.PC_was_Jump = false;

            Reset_GPR();
            RAM.PrescaleCounter = 1;
            _cycleCounter = 0;
            _isIsr = false;
        }

       public void OtherReset()
        {
            //Bank 1
            RAM[Constants.INDF_B1] = 0x00;
            //TMR0 unchanged
            RAM[Constants.PCL_B1] = 0x00;
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
            WReg = 0x0000;

            PCStack = new ObservableStack<short>(new Stack<short>(Constants.PC_STACK_CAPACITY));

            RAM.PortA.Value = 0;
            RAM.PortB.Value = 0;

            RAM.PortA.PORTA_Latch = 0;
            RAM.PortB.PORTB_Latch = 0;

            RAM.PCL_was_Manipulated = false;
            RAM.PC_was_Jump = false;

            Reset_GPR();
            RAM.PrescaleCounter = 1;
            _cycleCounter = 0;
            _isIsr = false;
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
