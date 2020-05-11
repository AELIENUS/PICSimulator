using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Application.Services
{
    class ByteConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value!= null)
            {
                string byteToString = value.ToString();
                return byteToString;
            }
            else
            {
                string noValue = "NULL";
                return noValue;
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte b = byte.Parse((string)value);
            return b;
        }
    }
}
