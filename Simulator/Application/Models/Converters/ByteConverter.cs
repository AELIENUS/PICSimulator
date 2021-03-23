using System;
using System.Globalization;
using System.Windows.Data;

namespace Application.Models.Converters
{
    class ByteConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte temp = (byte)value;
            string byteToString = Convert.ToString(temp, 2).PadLeft(8, '0');
            return byteToString;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string temp = (string)value;
            byte b = Convert.ToByte(temp);
            return b;
        }
    }
}
