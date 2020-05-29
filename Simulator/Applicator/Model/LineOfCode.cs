using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applicator.Model
{
    public class LineOfCode : ObservableObject
    {
        private string _Line;

        public string Line
        {
            get
            {
                return _Line;
            }
            set
            {
                _Line = value;
                RaisePropertyChanged();
            }
        }

        private bool _IsDebug;

        public bool IsDebug
        {
            get
            {
                return _IsDebug;
            }
            set
            {
                _IsDebug = value;
                RaisePropertyChanged();
            }
        }

        private int _CommandIndex = 0x10000;

        public int CommandIndex
        {
            get
            {
                return _CommandIndex;
            }
            set
            {
                _CommandIndex = value;
                RaisePropertyChanged();
            }
        }

        private short _ProgramCode;

        public short ProgramCode
        {
            get
            {
                return _ProgramCode;
            }
            set
            {
                _ProgramCode = value;
                RaisePropertyChanged();
            }
        }

        private bool _IsExecuted = false;

        public bool IsExecuted
        {
            get
            {
                return _IsExecuted;
            }
            set
            {
                _IsExecuted = value;
                RaisePropertyChanged();
            }
        }

        public LineOfCode()
        {

        }

        public LineOfCode(string code)
        {
            _Line = code;
        }
    }
}
