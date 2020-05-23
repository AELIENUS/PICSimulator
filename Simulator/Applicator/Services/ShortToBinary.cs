using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Application.Services
{
    class ShortToBinary : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            short temp = (short)value;
            string shortToString = Convert.ToString(temp, 2).PadLeft(8, '0');
            return shortToString;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string temp = (string)value;
            short s = (short)Convert.ToByte(temp);
            return s;
        }
    }
}
