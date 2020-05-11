using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace Application.Model
{
    public class RAMModel
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
                //Indizes bis 256 kommen rein 0 ist auf RAM 0 auf index 0
                //index auf Bereich ist SourceIndex modulo durch 16
                //Bank wird ermittelt durch das ganzzahlige abgerundete Ergebnis von SourceIndex durch 16
                int sliceIndex;
                int slice;
                if (index < 16)
                {
                    sliceIndex = 0;
                    slice = index;
                }
                else
                {
                    slice = index % 16;
                    sliceIndex = (int)Math.Floor(((double)index / 16));
                }
                //umrechnung auf Rambereiche und zugriff

                switch (slice)
                {
                    case 0:
                        return RAM0[sliceIndex];
                    case 1:
                        return RAM1[sliceIndex];
                    case 2:
                        return RAM2[sliceIndex];
                    case 3:
                        return RAM3[sliceIndex];
                    case 4:
                        return RAM4[sliceIndex];
                    case 5:
                        return RAM5[sliceIndex];
                    case 6:
                        return RAM6[sliceIndex];
                    case 7:
                        return RAM7[sliceIndex];
                    case 8:
                        return RAM8[sliceIndex];
                    case 9:
                        return RAM9[sliceIndex];
                    case 10:
                        return RAM10[sliceIndex];
                    case 11:
                        return RAM11[sliceIndex];
                    case 12:
                        return RAM12[sliceIndex];
                    case 13:
                        return RAM13[sliceIndex];
                    case 14:
                        return RAM14[sliceIndex];
                    case 15:
                        return RAM15[sliceIndex];
                    default:
                        throw new Exception("Dieser Speicher existiert nicht");
                }
            }
            set
            {
                //Indizes bis 256 kommen rein 0 ist auf RAM 0 auf index 0
                //index auf Bereich ist SourceIndex modulo durch 16
                //Bank wird ermittelt durch das ganzzahlige abgerundete Ergebnis von SourceIndex durch 16
                //Ausnahme: der mitgegebene Index ist kleiner als 16
                int sliceIndex;
                int slice;
                if (index<16)
                {
                    sliceIndex = 0;
                    slice = index; 
                }
                else
                {
                    slice = index % 16;
                    sliceIndex = (int)Math.Floor(((double)index / 16));
                }    
                //umrechnung auf Rambereiche und zugriff

                switch (slice)
                {
                    case 0:
                        RAM0[sliceIndex] = value;
                        break;
                    case 1:
                        RAM1[sliceIndex] = value;
                        break;
                    case 2:
                        RAM2[sliceIndex] = value;
                        break;
                    case 3:
                        RAM3[sliceIndex] = value;
                        break;
                    case 4:
                        RAM4[sliceIndex] = value;
                        break;
                    case 5:
                        RAM5[sliceIndex] = value;
                        break;
                    case 6:
                        RAM6[sliceIndex] = value;
                        break;
                    case 7:
                        RAM7[sliceIndex] = value;
                        break;
                    case 8:
                        RAM8[sliceIndex] = value;
                        break;
                    case 9:
                        RAM9[sliceIndex] = value;
                        break;
                    case 10:
                        RAM10[sliceIndex] = value;
                        break;
                    case 11:
                        RAM11[sliceIndex] = value;
                        break;
                    case 12:
                        RAM12[sliceIndex] = value;
                        break;
                    case 13:
                        RAM13[sliceIndex] = value;
                        break;
                    case 14:
                        RAM14[sliceIndex] = value;
                        break;
                    case 15:
                        RAM15[sliceIndex] = value;
                        break;
                    default:
                        throw new Exception("Dieser Speicher existiert nicht");
                }
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