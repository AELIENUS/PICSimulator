using Application.Model;
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
        #region RAM 
        private ObservableCollection<RAMPortion> _RAMList;
        public ObservableCollection<RAMPortion> RAMList 
        { 
            get
            {
                return _RAMList;
            }
            set
            {
                _RAMList = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        public byte this[int index]
        {
            get
            {
                double portionIndexDouble = index / 16;
                int portionIndex = (int)Math.Floor(portionIndexDouble);
                int position = index % 16;
                switch(position)
                {
                    case 0:
                        return RAMList[portionIndex].Byte0;
                    case 1:
                        return RAMList[portionIndex].Byte0;
                    case 2:
                        return RAMList[portionIndex].Byte0;
                    case 3:
                        return RAMList[portionIndex].Byte0;
                    case 4:
                        return RAMList[portionIndex].Byte0;
                    case 5:
                        return RAMList[portionIndex].Byte0;
                    case 6:
                        return RAMList[portionIndex].Byte0;
                    case 7:
                        return RAMList[portionIndex].Byte0;
                    case 8:
                        return RAMList[portionIndex].Byte0;
                    case 9:
                        return RAMList[portionIndex].Byte0;
                    case 10:
                        return RAMList[portionIndex].Byte0;
                    case 11:
                        return RAMList[portionIndex].Byte0;
                    case 12:
                        return RAMList[portionIndex].Byte0;
                    case 13:
                        return RAMList[portionIndex].Byte0;
                    case 14:
                        return RAMList[portionIndex].Byte0;
                    case 15:
                        return RAMList[portionIndex].Byte0;
                    default:
                        return 0;
                }
            }
            set
            {
                double portionIndexDouble = index / 16;
                int portionIndex = (int)Math.Floor(portionIndexDouble);
                int position = index % 16;
                switch (position)
                {
                    case 0:
                        RAMList[portionIndex].Byte0 = value;
                        break;
                    case 1:
                        RAMList[portionIndex].Byte1 = value;
                        break;
                    case 2:
                        RAMList[portionIndex].Byte2 = value;
                        break;
                    case 3:
                        RAMList[portionIndex].Byte3 = value;
                        break;
                    case 4:
                        RAMList[portionIndex].Byte4 = value;
                        break;
                    case 5:
                        RAMList[portionIndex].Byte5 = value;
                        break;
                    case 6:
                        RAMList[portionIndex].Byte6 = value;
                        break;
                    case 7:
                        RAMList[portionIndex].Byte7 = value;
                        break;
                    case 8:
                        RAMList[portionIndex].Byte8 = value;
                        break;
                    case 9:
                        RAMList[portionIndex].Byte9 = value;
                        break;
                    case 10:
                        RAMList[portionIndex].Byte10 = value;
                        break;
                    case 11:
                        RAMList[portionIndex].Byte11 = value;
                        break;
                    case 12:
                        RAMList[portionIndex].Byte12 = value;
                        break;
                    case 13:
                        RAMList[portionIndex].Byte13 = value;
                        break;
                    case 14:
                        RAMList[portionIndex].Byte14 = value;
                        break;
                    case 15:
                        RAMList[portionIndex].Byte15 = value;
                        break;
                }
            }

        }

        public RAMModel()
        {
            _RAMList = new ObservableCollection<RAMPortion>(new RAMPortion[16]);
            for (int i = 0; i < 16; i++)
            {
                _RAMList[i] = new RAMPortion();
            }
        }
    }
}