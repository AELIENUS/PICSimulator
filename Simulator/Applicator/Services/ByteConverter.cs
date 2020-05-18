using Microsoft.SqlServer.Server;
using System;
using System.CodeDom.Compiler;
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
                byte temp = (byte)value;
                string byteToString = Convert.ToString(temp, 2).PadLeft(8, '0');
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
            string temp = (string)value;
            byte b = Convert.ToByte(temp);
            return b;
        }
    }
}
