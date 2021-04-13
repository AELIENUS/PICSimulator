using GalaSoft.MvvmLight;
using System.Collections.Generic;
using Application.Constants;
using Application.Models.CustomDatastructures;

namespace Application.Models.Memory
{
    public class MemoryService : ObservableObject, IMemoryService
    {
        #region properties

        private bool _isIsr = false;
        public bool IsISR
        {
            get => _isIsr;
            set => _isIsr = value;
        }

        public byte TRISAValue
        {
            get
            {
                return _ram.PortA.TRISValue.Value;
            }
            set
            {
                _ram.PortA.TRISValue.Value = value;
                RaisePropertyChanged();
            }
        }

        public byte PortAValue
        {
            get
            {
                return _ram.PortA.PortValue.Value;
            }
            set
            {
                _ram.PortA.PortValue.Value = value;
                RaisePropertyChanged();
            }
        }

        public bool PortAPin0
        {
            get
            {
                return _ram.PortA.Pin0;
            }
            set
            {
                _ram.PortA.Pin0 = value;
                RaisePropertyChanged();
            }
        }

        public bool PortAPin1
        {
            get
            {
                return _ram.PortA.Pin1;
            }
            set
            {
                _ram.PortA.Pin1 = value;
                RaisePropertyChanged();
            }
        }

        public bool PortAPin2
        {
            get
            {
                return _ram.PortA.Pin2;
            }
            set
            {
                _ram.PortA.Pin2 = value;
                RaisePropertyChanged();
            }
        }

        public bool PortAPin3
        {
            get
            {
                return _ram.PortA.Pin3;
            }
            set
            {
                _ram.PortA.Pin3 = value;
                RaisePropertyChanged();
            }
        }

        public bool PortAPin4
        {
            get
            {
                return _ram.PortA.Pin4;
            }
            set
            {
                _ram.PortA.Pin4 = value;
                RaisePropertyChanged();
            }
        }

        public bool PortAPin5
        {
            get
            {
                return _ram.PortA.Pin5;
            }
            set
            {
                _ram.PortA.Pin5 = value;
                RaisePropertyChanged();
            }
        }

        public bool PortAPin6
        {
            get
            {
                return _ram.PortA.Pin6;
            }
            set
            {
                _ram.PortA.Pin6 = value;
                RaisePropertyChanged();
            }
        }

        public bool PortAPin7
        {
            get
            {
                return _ram.PortA.Pin7;
            }
            set
            {
                _ram.PortA.Pin7 = value;
                RaisePropertyChanged();
            }
        }

        public byte TRISBValue
        {
            get
            {
                return _ram.PortB.TRISValue.Value;
            }
            set
            {
                _ram.PortB.TRISValue.Value = value;
                RaisePropertyChanged();
            }
        }

        public byte PortBValue
        {
            get
            {
                return _ram.PortB.PortValue.Value;
            }
            set
            {
                _ram.PortB.PortValue.Value = value;
                RaisePropertyChanged();
            }
        }

        public bool PortBPin0
        {
            get
            {
                return _ram.PortB.Pin0;
            }
            set
            {
                _ram.PortB.Pin0 = value;
                RaisePropertyChanged();
            }
        }

        public bool PortBPin1
        {
            get
            {
                return _ram.PortB.Pin1;
            }
            set
            {
                _ram.PortB.Pin1 = value;
                RaisePropertyChanged();
            }
        }

        public bool PortBPin2
        {
            get
            {
                return _ram.PortB.Pin2;
            }
            set
            {
                _ram.PortB.Pin2 = value;
                RaisePropertyChanged();
            }
        }

        public bool PortBPin3
        {
            get
            {
                return _ram.PortB.Pin3;
            }
            set
            {
                _ram.PortB.Pin3 = value;
                RaisePropertyChanged();
            }
        }

        public bool PortBPin4
        {
            get
            {
                return _ram.PortB.Pin4;
            }
            set
            {
                _ram.PortB.Pin4 = value;
                RaisePropertyChanged();
            }
        }

        public bool PortBPin5
        {
            get
            {
                return _ram.PortB.Pin5;
            }
            set
            {
                _ram.PortB.Pin5 = value;
                RaisePropertyChanged();
            }
        }

        public bool PortBPin6
        {
            get
            {
                return _ram.PortB.Pin6;
            }
            set
            {
                _ram.PortB.Pin6 = value;
                RaisePropertyChanged();
            }
        }

        public bool PortBPin7
        {
            get
            {
                return _ram.PortB.Pin7;
            }
            set
            {
                _ram.PortB.Pin7 = value;
                RaisePropertyChanged();
            }
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
                if ((RAM[MemoryConstants.OPTION_REG] & 0b0010_0000) == 0)
                {
                    RAM.IncTimer0();
                }
                //Runtime neu berechnen
                Runtime ++;
                RaisePropertyChanged();
            }
        }

        public int ZFlag
        {
            get
            {
                return _ram.ZFlag;
            }
        }

        public int DCFlag
        {
            get
            {
                return _ram.DCFlag;
            }
        }

        public int CFlag
        {
            get
            {
                return _ram.CFlag;
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

        private Stack<short> _pcStack;
        public Stack<short> PCStack 
        { 
            get
            {
                return _pcStack;
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

        private IRAMModel _ram;
        public IRAMModel RAM
        {
            get => _ram;
            set
            {
                _ram = value;
                RaisePropertyChanged();
            }
        }

        public int PCWithoutClear
        {
            get
            {
                return _ram.PCWithoutClear;
            }
        }

        public byte PCL
        {
            get
            {
                return _ram.PCL;
            }
        }

        public byte PCLATH
        {
            get
            {
                return _ram.PCLATH;
            }
        }
        #endregion

        public MemoryService(IRAMModel ram, Stack<short> pcStack)
        {
            _ram = ram;
            _pcStack = pcStack;
            _ram.PropertyChanged += RAM_PropertyChanged;
            _ram.PortA.PropertyChanged += PortA_PropertyChanged;
            _ram.PortB.PropertyChanged += PortB_PropertyChanged;
            PowerReset();
        }

        private void PortB_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "TRISValue":
                    RaisePropertyChanged("TRISBValue");
                    break;
                case "PortValue":
                    RaisePropertyChanged("PortBValue");
                    break;
                case "Pin0":
                    RaisePropertyChanged("PortBPin0");
                    break;
                case "Pin1":
                    RaisePropertyChanged("PortBPin1");
                    break;
                case "Pin2":
                    RaisePropertyChanged("PortBPin2");
                    break;
                case "Pin3":
                    RaisePropertyChanged("PortBPin3");
                    break;
                case "Pin4":
                    RaisePropertyChanged("PortBPin4");
                    break;
                case "Pin5":
                    RaisePropertyChanged("PortBPin5");
                    break;
                case "Pin6":
                    RaisePropertyChanged("PortBPin6");
                    break;
                case "Pin7":
                    RaisePropertyChanged("PortBPin7");
                    break;
                default:
                    break;
            }
        }

        private void PortA_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "TRISValue":
                    RaisePropertyChanged("TRISAValue");
                    break;
                case "PortValue":
                    RaisePropertyChanged("PortAValue");
                    break;
                case "Pin0":
                    RaisePropertyChanged("PortAPin0");
                    break;
                case "Pin1":
                    RaisePropertyChanged("PortAPin1");
                    break;
                case "Pin2":
                    RaisePropertyChanged("PortAPin2");
                    break;
                case "Pin3":
                    RaisePropertyChanged("PortAPin3");
                    break;
                case "Pin4":
                    RaisePropertyChanged("PortAPin4");
                    break;
                case "Pin5":
                    RaisePropertyChanged("PortAPin5");
                    break;
                case "Pin6":
                    RaisePropertyChanged("PortAPin6");
                    break;
                case "Pin7":
                    RaisePropertyChanged("PortAPin7");
                    break;
                default:
                    break;
            }
        }

        private void RAM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(e.PropertyName);
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

            PCStack.Clear();

            RAM.PortA.PortValue.Value = 0;
            RAM.PortB.PortValue.Value = 0;

            RAM.PortA.PortLatch = 0;
            RAM.PortB.PortLatch = 0;

            RAM.PCLWasManipulated = false;
            RAM.PCWasJump = false;

            ResetGPR();
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

            PCStack.Clear();

            RAM.PortA.PortValue.Value = 0;
            RAM.PortB.PortValue.Value = 0;

            RAM.PortA.PortLatch = 0;
            RAM.PortB.PortLatch = 0;

            RAM.PCLWasManipulated = false;
            RAM.PCWasJump = false;

            ResetGPR();
            RAM.PrescaleCounter = 1;
            _cycleCounter = 0;
            _isIsr = false;
        }

        public void ResetGPR()
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
