using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Memory
{
    class PortWithTimerPin : Port
    {
        public override bool Pin4
        {
            get
            {
                return base.GetPin(4);
            }
            set
            {
                SetPortAPin4(value);
                RaisePropertyChanged();
            }
        }

        public void SetPortAPin4(bool valueToSet)
        {
            ////PortA PIN 4 ist source von Timer0 
            //if ((_RAMModel.RAMList[8].Byte1.Value & 0b0010_0000) > 0)
            //{
            //    if ((_RAMModel.RAMList[8].Byte1.Value & 0b0001_0000) == 0)
            //    {
            //        //steigende Flanke
            //        if ((valueToSet == true)
            //            && ((_RAMModel.RAMList[0].Byte5.Value & 0b0001_0000) == 0))
            //        {
            //            _RAMModel.IncTimer0();
            //        }
            //    }
            //    else
            //    {
            //        //fallende Flanke
            //        if ((valueToSet == false)
            //            && ((_RAMModel.RAMList[0].Byte5.Value & 0b0001_0000) > 0))
            //        {
            //            _RAMModel.IncTimer0();
            //        }
            //    }
            //}
            ////Wert setzen
            //if (valueToSet == true)
            //{
            //    PortValue.Value |= 0b0001_0000;
            //}
            //else
            //{
            //    PortValue.Value &= 0b1110_1111;
            //}
        }
    }
}
