using Applicator.Model;
using Applicator.Services;
using GalaSoft.MvvmLight;
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
    public class RAMModel : ObservableObject
    {
        #region RAM portions
        private ObservableCollection<ItemNotifyByte> _RAM0;
        public ObservableCollection<ItemNotifyByte> RAM0
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

        private ObservableCollection<ItemNotifyByte> _RAM1;
        public ObservableCollection<ItemNotifyByte> RAM1
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

        private ObservableCollection<ItemNotifyByte> _RAM2;
        public ObservableCollection<ItemNotifyByte> RAM2
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

        private ObservableCollection<ItemNotifyByte> _RAM3;
        public ObservableCollection<ItemNotifyByte> RAM3
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

        private ObservableCollection<ItemNotifyByte> _RAM4;
        public ObservableCollection<ItemNotifyByte> RAM4
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

        private ObservableCollection<ItemNotifyByte> _RAM5;
        public ObservableCollection<ItemNotifyByte> RAM5
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

        private ObservableCollection<ItemNotifyByte> _RAM6;
        public ObservableCollection<ItemNotifyByte> RAM6
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

        private ObservableCollection<ItemNotifyByte> _RAM7;
        public ObservableCollection<ItemNotifyByte> RAM7
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

        private ObservableCollection<ItemNotifyByte> _RAM8;
        public ObservableCollection<ItemNotifyByte> RAM8
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

        private ObservableCollection<ItemNotifyByte> _RAM9;
        public ObservableCollection<ItemNotifyByte> RAM9
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

        private ObservableCollection<ItemNotifyByte> _RAM10;
        public ObservableCollection<ItemNotifyByte> RAM10
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

        private ObservableCollection<ItemNotifyByte> _RAM11;
        public ObservableCollection<ItemNotifyByte> RAM11
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

        private ObservableCollection<ItemNotifyByte> _RAM12;
        public ObservableCollection<ItemNotifyByte> RAM12
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

        private ObservableCollection<ItemNotifyByte> _RAM13;
        public ObservableCollection<ItemNotifyByte> RAM13
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

        private ObservableCollection<ItemNotifyByte> _RAM14;
        public ObservableCollection<ItemNotifyByte> RAM14
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

        private ObservableCollection<ItemNotifyByte> _RAM15;
        public ObservableCollection<ItemNotifyByte> RAM15
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
                double tempSlice;
                if (index < 16)
                {
                    sliceIndex = 0;
                    tempSlice = index;
                }
                else
                {
                    sliceIndex = index % 16;
                    tempSlice = index / 16;
                }
                int slice = (int)Math.Floor(tempSlice);
                //umrechnung auf Rambereiche und zugriff

                switch (slice)
                {
                    case 0:
                        return RAM0[sliceIndex].Value;
                    case 1:
                        return RAM1[sliceIndex].Value;
                    case 2:
                        return RAM2[sliceIndex].Value;
                    case 3:
                        return RAM3[sliceIndex].Value;
                    case 4:
                        return RAM4[sliceIndex].Value;
                    case 5:
                        return RAM5[sliceIndex].Value;
                    case 6:
                        return RAM6[sliceIndex].Value;
                    case 7:
                        return RAM7[sliceIndex].Value;
                    case 8:
                        return RAM8[sliceIndex].Value;
                    case 9:
                        return RAM9[sliceIndex].Value;
                    case 10:
                        return RAM10[sliceIndex].Value;
                    case 11:
                        return RAM11[sliceIndex].Value;
                    case 12:
                        return RAM12[sliceIndex].Value;
                    case 13:
                        return RAM13[sliceIndex].Value;
                    case 14:
                        return RAM14[sliceIndex].Value;
                    case 15:
                        return RAM15[sliceIndex].Value;
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
<<<<<<< Updated upstream
                double tempSlice;
                if (index<16)
                {
                    sliceIndex = 0;
                    tempSlice = index; 
                }
                else
                {
                    sliceIndex = index % 16;
                    tempSlice = index / 16;
                }    
                int slice = (int)Math.Floor(tempSlice);
=======
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
>>>>>>> Stashed changes
                //umrechnung auf Rambereiche und zugriff
                
                switch (slice)
                {
                    case 0:
                        RAM0[sliceIndex].Value = value;
                        break;
                    case 1:
                        RAM1[sliceIndex].Value = value;
                        break;
                    case 2:
                        RAM2[sliceIndex].Value = value;
                        break;
                    case 3:
                        RAM3[sliceIndex].Value = value;
                        break;
                    case 4:
                        RAM4[sliceIndex].Value = value;
                        break;
                    case 5:
                        RAM5[sliceIndex].Value = value;
                        break;
                    case 6:
                        RAM6[sliceIndex].Value = value;
                        break;
                    case 7:
                        RAM7[sliceIndex].Value = value;
                        break;
                    case 8:
                        RAM8[sliceIndex].Value = value;
                        break;
                    case 9:
                        RAM9[sliceIndex].Value = value;
                        break;
                    case 10:
                        RAM10[sliceIndex].Value = value;
                        break;
                    case 11:
                        RAM11[sliceIndex].Value = value;
                        break;
                    case 12:
                        RAM12[sliceIndex].Value = value;
                        break;
                    case 13:
                        RAM13[sliceIndex].Value = value;
                        break;
                    case 14:
                        RAM14[sliceIndex].Value = value;
                        break;
                    case 15:
                        RAM15[sliceIndex].Value = value;
                        break;
                    default:
                        throw new Exception("Dieser Speicher existiert nicht");
                }
            }

        }

        public RAMModel()
        {
            RAM0 = new ObservableCollection<ItemNotifyByte>(new ItemNotifyByte[16]);
            RAM1 = new ObservableCollection<ItemNotifyByte>(new ItemNotifyByte[16]);
            RAM2 = new ObservableCollection<ItemNotifyByte>(new ItemNotifyByte[16]);
            RAM3 = new ObservableCollection<ItemNotifyByte>(new ItemNotifyByte[16]);
            RAM4 = new ObservableCollection<ItemNotifyByte>(new ItemNotifyByte[16]);
            RAM5 = new ObservableCollection<ItemNotifyByte>(new ItemNotifyByte[16]);
            RAM6 = new ObservableCollection<ItemNotifyByte>(new ItemNotifyByte[16]);
            RAM7 = new ObservableCollection<ItemNotifyByte>(new ItemNotifyByte[16]);
            RAM8 = new ObservableCollection<ItemNotifyByte>(new ItemNotifyByte[16]);
            RAM9 = new ObservableCollection<ItemNotifyByte>(new ItemNotifyByte[16]);
            RAM10 = new ObservableCollection<ItemNotifyByte>(new ItemNotifyByte[16]);
            RAM11 = new ObservableCollection<ItemNotifyByte>(new ItemNotifyByte[16]);
            RAM12 = new ObservableCollection<ItemNotifyByte>(new ItemNotifyByte[16]);
            RAM13 = new ObservableCollection<ItemNotifyByte>(new ItemNotifyByte[16]);
            RAM14 = new ObservableCollection<ItemNotifyByte>(new ItemNotifyByte[16]);
            RAM15 = new ObservableCollection<ItemNotifyByte>(new ItemNotifyByte[16]);
            //create Objects in the RAM class
            for (int i = 0; i < 16; i++)
            {
                RAM0[i] = new ItemNotifyByte();
            }
            for (int i = 0; i < 16; i++)
            {
                RAM1[i] = new ItemNotifyByte();
            }
            for (int i = 0; i < 16; i++)
            {
                RAM2[i] = new ItemNotifyByte();
            }
            for (int i = 0; i < 16; i++)
            {
                RAM3[i] = new ItemNotifyByte();
            }
            for (int i = 0; i < 16; i++)
            {
                RAM4[i] = new ItemNotifyByte();
            }
            for (int i = 0; i < 16; i++)
            {
                RAM5[i] = new ItemNotifyByte();
            }
            for (int i = 0; i < 16; i++)
            {
                RAM6[i] = new ItemNotifyByte();
            }
            for (int i = 0; i < 16; i++)
            {
                RAM7[i] = new ItemNotifyByte();
            }
            for (int i = 0; i < 16; i++)
            {
                RAM8[i] = new ItemNotifyByte();
            }
            for (int i = 0; i < 16; i++)
            {
                RAM9[i] = new ItemNotifyByte();
            }
            for (int i = 0; i < 16; i++)
            {
                RAM10[i] = new ItemNotifyByte();
            }
            for (int i = 0; i < 16; i++)
            {
                RAM11[i] = new ItemNotifyByte();
            }
            for (int i = 0; i < 16; i++)
            {
                RAM12[i] = new ItemNotifyByte();
            }
            for (int i = 0; i < 16; i++)
            {
                RAM13[i] = new ItemNotifyByte();
            }
            for (int i = 0; i < 16; i++)
            {
                RAM14[i] = new ItemNotifyByte();
            }
            for (int i = 0; i < 16; i++)
            {
                RAM15[i] = new ItemNotifyByte();
            }
        }
    }
}