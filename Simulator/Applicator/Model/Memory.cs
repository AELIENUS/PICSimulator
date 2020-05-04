using Applicator.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace Applicator.Model
{
    public class Memory
    {
        #region PropertyChanged Teil TODO: gibt es das in MVVM light?
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        private byte[] _ram = new byte[Constants.RAM_SIZE];


        ObservableCollection<byte> RAM
        {
            get;
            set;
        }
        /*public byte[] RAM
        {
            get
            {
                return _ram;
            }
            set
            {
                if(value.Equals(_ram))
                {
                    return;
                }
                _ram = value;
                RaisePropertyChanged();
            }
        }*/

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

        private short[] _program = new short[Constants.PROGRAM_MEMORY_SIZE];
        public short[] Program
        {
            get
            {
                return _program;
            }
            set
            {
                if (value.Equals(_program))
                {
                    return;
                }
                _program = value;
                RaisePropertyChanged();
            }
        }
        public Memory()
        {
            Program = new short[Constants.PROGRAM_MEMORY_SIZE];
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

            Reset_GPR();
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
            RAM[Constants.EECON2] = 0x00;
            RAM[Constants.PCLATH_B2] = 0x00;
            RAM[Constants.INTCON_B2] &= 0x001;

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
