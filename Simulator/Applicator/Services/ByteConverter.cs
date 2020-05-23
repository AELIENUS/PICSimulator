<<<<<<< Updated upstream
﻿using System;
=======
﻿using Applicator.Model;
using Microsoft.SqlServer.Server;
using System;
using System.CodeDom.Compiler;
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
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
=======
            byte temp = (byte)value;
            string byteToString = Convert.ToString(temp, 2).PadLeft(8, '0');
            return byteToString;
>>>>>>> Stashed changes
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte b = byte.Parse((string)value);
            return b;
        }
    }
}
