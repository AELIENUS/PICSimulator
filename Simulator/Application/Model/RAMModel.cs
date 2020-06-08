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
                        return RAMList[portionIndex].Byte0.Value;
                    case 1:
                        return RAMList[portionIndex].Byte1.Value;
                    case 2:
                        return RAMList[portionIndex].Byte2.Value;
                    case 3:
                        return RAMList[portionIndex].Byte3.Value;
                    case 4:
                        return RAMList[portionIndex].Byte4.Value;
                    case 5:
                        return RAMList[portionIndex].Byte5.Value;
                    case 6:
                        return RAMList[portionIndex].Byte6.Value;
                    case 7:
                        return RAMList[portionIndex].Byte7.Value;
                    case 8:
                        return RAMList[portionIndex].Byte8.Value;
                    case 9:
                        return RAMList[portionIndex].Byte9.Value;
                    case 10:
                        return RAMList[portionIndex].Byte10.Value;
                    case 11:
                        return RAMList[portionIndex].Byte11.Value;
                    case 12:
                        return RAMList[portionIndex].Byte12.Value;
                    case 13:
                        return RAMList[portionIndex].Byte13.Value;
                    case 14:
                        return RAMList[portionIndex].Byte14.Value;
                    case 15:
                        return RAMList[portionIndex].Byte15.Value;
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
                        RAMList[portionIndex].Byte0.Value = value;
                        break;
                    case 1:
                        RAMList[portionIndex].Byte1.Value = value;
                        break;
                    case 2:
                        RAMList[portionIndex].Byte2.Value = value;
                        break;
                    case 3:
                        RAMList[portionIndex].Byte3.Value = value;
                        break;
                    case 4:
                        RAMList[portionIndex].Byte4.Value = value;
                        break;
                    case 5:
                        RAMList[portionIndex].Byte5.Value = value;
                        //ist Port A gemeint?
                        if((portionIndex == 0)
                            //T0CS gesetzt
                            && ((RAMList[8].Byte1.Value & 0b0010_0000) > 0)                            
                            //PORTA Pin4 ist aktuell high
                            && ((RAMList[portionIndex].Byte5.Value & 0b0001_0000) > 0))
                        {
                            //wenn alles stimmt dann inkrementiere Timer 0
                            RAMList[0].Byte1.Value++;
                        }
                        break;
                    case 6:
                        RAMList[portionIndex].Byte6.Value = value;
                        break;
                    case 7:
                        RAMList[portionIndex].Byte7.Value = value;
                        break;
                    case 8:
                        RAMList[portionIndex].Byte8.Value = value;
                        break;
                    case 9:
                        RAMList[portionIndex].Byte9.Value = value;
                        break;
                    case 10:
                        RAMList[portionIndex].Byte10.Value = value;
                        break;
                    case 11:
                        RAMList[portionIndex].Byte11.Value = value;
                        break;
                    case 12:
                        RAMList[portionIndex].Byte12.Value = value;
                        break;
                    case 13:
                        RAMList[portionIndex].Byte13.Value = value;
                        break;
                    case 14:
                        RAMList[portionIndex].Byte14.Value = value;
                        break;
                    case 15:
                        RAMList[portionIndex].Byte15.Value = value;
                        break;
                }
            }

        }

        public void IncTimer0 (int prescaler)
        {
            //ist T0CS = 0?
            if ((RAMList[8].Byte1.Value & 0b0010_0000) == 0)
            {
                //ist Overflow?
                if(RAMList[0].Byte1.Value==255)
                {
                    RAMList[0].Byte1.Value = 0;
                    //Ist GIE und T0IE an?
                    if ((RAMList[0].Byte11.Value & 0b1010_0000) >= 0b1010_0000)
                    {
                        //T0IF setzen
                        RAMList[0].Byte11.Value |= 0b0000_0100;
                        RAMList[8].Byte11.Value |= 0b0000_0100;
                    }
                    //INTF an
                    RAMList[0].Byte11.Value |= 0b0000_0010;
                    RAMList[8].Byte11.Value |= 0b0000_0010;
                }
                else
                {
                    RAMList[0].Byte1.Value++;
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