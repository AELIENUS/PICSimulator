using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public abstract class AbstractByte : ObservableObject
    {
        public abstract byte Value 
        { 
            get;
            set; 
        }
    }
}
