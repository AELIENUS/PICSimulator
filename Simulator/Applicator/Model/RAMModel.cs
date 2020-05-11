using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Applicator.Model
{
    class RAMModel
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

        #region RAM portions
        private ObservableCollection<byte> _RAM0;
        public ObservableCollection<byte> RAM0
        {
            get
            {
                return _RAM0;
            }
            set
            {
                _RAM0 = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<byte> _RAM1;
        public ObservableCollection<byte> RAM1
        {
            get
            {
                return _RAM1;
            }
            set
            {
                _RAM1 = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<byte> _RAM2;
        public ObservableCollection<byte> RAM2
        {
            get
            {
                return _RAM2;
            }
            set
            {
                _RAM2 = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<byte> _RAM3;
        public ObservableCollection<byte> RAM3
        {
            get
            {
                return _RAM3;
            }
            set
            {
                _RAM3 = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<byte> _RAM4;
        public ObservableCollection<byte> RAM4
        {
            get
            {
                return _RAM4;
            }
            set
            {
                _RAM4 = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<byte> _RAM5;
        public ObservableCollection<byte> RAM5
        {
            get
            {
                return _RAM5;
            }
            set
            {
                _RAM5 = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<byte> _RAM6;
        public ObservableCollection<byte> RAM6
        {
            get
            {
                return _RAM6;
            }
            set
            {
                _RAM6 = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<byte> _RAM7;
        public ObservableCollection<byte> RAM7
        {
            get
            {
                return _RAM7;
            }
            set
            {
                _RAM7 = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<byte> _RAM8;
        public ObservableCollection<byte> RAM8
        {
            get
            {
                return _RAM8;
            }
            set
            {
                _RAM8 = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<byte> _RAM9;
        public ObservableCollection<byte> RAM9
        {
            get
            {
                return _RAM9;
            }
            set
            {
                _RAM9 = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<byte> _RAM10;
        public ObservableCollection<byte> RAM10
        {
            get
            {
                return _RAM10;
            }
            set
            {
                _RAM10 = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<byte> _RAM11;
        public ObservableCollection<byte> RAM11
        {
            get
            {
                return _RAM11;
            }
            set
            {
                _RAM11 = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<byte> _RAM12;
        public ObservableCollection<byte> RAM12
        {
            get
            {
                return _RAM12;
            }
            set
            {
                _RAM12 = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<byte> _RAM13;
        public ObservableCollection<byte> RAM13
        {
            get
            {
                return _RAM13;
            }
            set
            {
                _RAM13 = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<byte> _RAM14;
        public ObservableCollection<byte> RAM14
        {
            get
            {
                return _RAM14;
            }
            set
            {
                _RAM14 = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<byte> _RAM15;
        public ObservableCollection<byte> RAM15
        {
            get
            {
                return _RAM15;
            }
            set
            {
                _RAM15 = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        public byte this[int index]
        {
            get
            {
                //umrechnung auf Rambereiche und zugriff
                return 0;
            }
            set
            {
                //umrechnung auf Rambereiche und zugriff
            }

        }

        public RAMModel()
        {
            RAM0 = new ObservableCollection<byte>(new byte[16]);
            RAM1 = new ObservableCollection<byte>(new byte[16]);
            RAM2 = new ObservableCollection<byte>(new byte[16]);
            RAM3 = new ObservableCollection<byte>(new byte[16]);
            RAM4 = new ObservableCollection<byte>(new byte[16]);
            RAM5 = new ObservableCollection<byte>(new byte[16]);
            RAM6 = new ObservableCollection<byte>(new byte[16]);
            RAM7 = new ObservableCollection<byte>(new byte[16]);
            RAM8 = new ObservableCollection<byte>(new byte[16]);
            RAM9 = new ObservableCollection<byte>(new byte[16]);
            RAM10 = new ObservableCollection<byte>(new byte[16]);
            RAM11 = new ObservableCollection<byte>(new byte[16]);
            RAM12 = new ObservableCollection<byte>(new byte[16]);
            RAM13 = new ObservableCollection<byte>(new byte[16]);
            RAM14 = new ObservableCollection<byte>(new byte[16]);
            RAM15 = new ObservableCollection<byte>(new byte[16]);
        }
    }
}