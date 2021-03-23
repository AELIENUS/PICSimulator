using GalaSoft.MvvmLight;
using System.Collections.Generic;
using Application.Constants;
using Application.Models.CustomDatastructures;

namespace Application.Models.Memory
{
    public class MemoryService : ObservableObject
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
                       (_pcStack = new ObservableStack<short>(new Stack<short>(MemoryConstants.PC_STACK_CAPACITY)));
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

        public MemoryService()
        {
            RAM = new RAMModel();
            PowerReset();
        }

        public void PowerReset()
        {
            //Initialzustand von RAM wiederherstellen
            //Bank 1
            RAM[MemoryConstants.INDF_B1] = 0x00;
            RAM[MemoryConstants.TMR0] = 0x00;
            RAM[MemoryConstants.PCL_B1] = 0x00;
            RAM[MemoryConstants.STATUS_B1] = 0b0001_1000;
            RAM[MemoryConstants.FSR_B1] = 0x00;
            RAM[MemoryConstants.PORTA] = 0x00;
            RAM[MemoryConstants.PORTB] = 0x00;
            RAM[MemoryConstants.EEDATA] = 0x00;
            RAM[MemoryConstants.EEADR] = 0x00;
            RAM[MemoryConstants.PCLATH_B1] = 0x00;
            RAM[MemoryConstants.INTCON_B1] = 0x00;
            //Bank2
            RAM[MemoryConstants.INDF_B2] = 0x00;
            RAM[MemoryConstants.OPTION_REG] = 0xFF;
            RAM[MemoryConstants.PCL_B2] = 0x00;
            RAM[MemoryConstants.STATUS_B2] = 0b0001_1000;
            RAM[MemoryConstants.FSR_B2] = 0x00;
            RAM[MemoryConstants.TRISA] = 0xFF;
            RAM[MemoryConstants.TRISB] = 0xFF;
            RAM[MemoryConstants.EECON1] = 0x00;
            RAM[MemoryConstants.EECON2] = 0x00;
            RAM[MemoryConstants.PCLATH_B2] = 0x00;
            RAM[MemoryConstants.INTCON_B2] = 0x00;
            WReg = 0x0000;

            PCStack = new ObservableStack<short>(new Stack<short>(MemoryConstants.PC_STACK_CAPACITY));

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
            RAM[MemoryConstants.INDF_B1] = 0x00;
            //TMR0 unchanged
            RAM[MemoryConstants.PCL_B1] = 0x00;
            RAM[MemoryConstants.STATUS_B1] &= 0b0001_1111;
            //FSR unchanged
            //PORTA unchanged
            //PORTB unchanged
            //EEDATA unchanged
            //EEADR unchanged
            RAM[MemoryConstants.PCLATH_B1] = 0x00;
            RAM[MemoryConstants.INTCON_B1] &= 0x01;
            //Bank2
            RAM[MemoryConstants.INDF_B2] = 0x00;
            RAM[MemoryConstants.OPTION_REG] = 0xFF;
            RAM[MemoryConstants.PCL_B2] = 0x00;
            RAM[MemoryConstants.STATUS_B2] &= 0b0001_1111;
            //FSR unchanged
            RAM[MemoryConstants.TRISA] = 0xFF;
            RAM[MemoryConstants.TRISB] = 0xFF;
            //Bit 4 konditional
            RAM[MemoryConstants.EECON1] = 0x00;
            RAM[MemoryConstants.EECON2]= 0x00;
            RAM[MemoryConstants.PCLATH_B2] = 0x00;
            RAM[MemoryConstants.INTCON_B2] &= 0x001;
            WReg = 0x0000;

            PCStack = new ObservableStack<short>(new Stack<short>(MemoryConstants.PC_STACK_CAPACITY));

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
            for (int i = MemoryConstants.GPR_START_B1; i <= MemoryConstants.GPR_END_B1; i++)
            {
                RAM[i] = 0x00;
            }

            //GPR 2 zurücksetzen
            for (int i = MemoryConstants.GPR_START_B2; i <= MemoryConstants.GPR_END_B2; i++)
            {
                RAM[i] = 0x00;
            }
        }
    }
}
