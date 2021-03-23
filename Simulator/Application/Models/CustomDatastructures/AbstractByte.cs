using GalaSoft.MvvmLight;

namespace Application.Models.CustomDatastructures
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
