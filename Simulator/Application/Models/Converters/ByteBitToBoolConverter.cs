using System;
using System.Globalization;
using System.Windows.Data;

namespace Application.Models.Converters
{
    class ByteBitToBoolConverter : IValueConverter
    {
        private static byte Backup
        {
            get;
            set;
        }
        

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Backup = (byte)value;
            byte temp = (byte)value;
            byte position = byte.Parse((string)parameter);
            bool returnValue;
            switch(parameter)
            {
                case 0:
                    temp &= 0b0000_0001;
                    break;
                case 1:
                    temp &= 0b0000_0010;
                    break;
                case 2:
                    temp &= 0b0000_0100;
                    break;
                case 3:
                    temp &= 0b0000_1000;
                    break;
                case 4:
                    temp &= 0b0001_0000;
                    break;
                case 5:
                    temp &= 0b0010_0000;
                    break;
                case 6:
                    temp &= 0b0100_0000;
                    break;
                case 7:
                    temp &= 0b1000_0000;
                    break;
            }
            if (Backup > 0)
                returnValue = true;
            else
                returnValue = false;
            return returnValue;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool inputBool = (bool)value;
            byte position = byte.Parse((string)parameter);
            switch (position)
            {
                case 0:
                    if (inputBool)
                        Backup |= 0b0000_0001;
                    else
                        Backup &= 0b1111_1110;
                    break;
                case 1:
                    if (inputBool)
                        Backup |= 0b0000_0010;
                    else
                        Backup &= 0b1111_1101;
                    break;
                case 2:
                    if (inputBool)
                        Backup |= 0b0000_0100;
                    else
                        Backup &= 0b1111_1011;
                    break;
                case 3:
                    if (inputBool)
                        Backup |= 0b0000_1000;
                    else
                        Backup &= 0b1111_0111;
                    break;
                case 4:
                    if (inputBool)
                        Backup |= 0b0001_0000;
                    else
                        Backup &= 0b1110_1111;
                    break;
                case 5:
                    if (inputBool)
                        Backup |= 0b0010_0000;
                    else
                        Backup &= 0b1101_1111;
                    break;
                case 6:
                    if (inputBool)
                        Backup |= 0b0100_0000;
                    else
                        Backup &= 0b1011_1111;
                    break;
                case 7:
                    if (inputBool)
                        Backup |= 0b1000_0000;
                    else
                        Backup &= 0b0111_1111;
                    break;
            }
            return Backup;
        }
    }
}
