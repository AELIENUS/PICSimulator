using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Applicator.Model
{
    public class ItemNotifyByte: ObservableObject
    {
        private byte _value;

        public byte Value 
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value == value)
                {
                    return;
                }
                _value = value;
                RaisePropertyChanged("Value");
            }
        }

        public ItemNotifyByte()
        {
            Value = 0;
        }
    }
}
