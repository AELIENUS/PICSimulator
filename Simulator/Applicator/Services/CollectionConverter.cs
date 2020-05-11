using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Application.Services
{
    class CollectionConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var sourceCollection = value as ObservableCollection<byte>;
            var destinationCollection = new ObservableCollection<string>(new string[sourceCollection.Count]);
            for (int i = 0; i < sourceCollection.Count; i++)
            {
                destinationCollection[i] = sourceCollection[i].ToString();
            }
            return destinationCollection;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var sourceCollection = value as ObservableCollection<string>;
            var destinationCollection = new ObservableCollection<byte>(new byte[sourceCollection.Count]);
            for (int i = 0; i < sourceCollection.Count; i++)
            {
                destinationCollection[i] = byte.Parse(sourceCollection[i]);
            }
            return destinationCollection;
        }
    }
}
