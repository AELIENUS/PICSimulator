using Applicator.Services;
using System;
using System.Collections.Generic;
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

        private byte[] _dataMemory;

        public byte[] DataMemory
        {
            get
            {
                return _dataMemory;
            }
            set
            {
                if(value.Equals(_dataMemory))
                {
                    return;
                }
                _dataMemory = value;
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

        private short[] _program;
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
        }
    }
}
