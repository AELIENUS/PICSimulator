using Applicator.Model;
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
            var sourceCollection = value as ObservableCollection<ItemNotifyByte>;
            var destinationCollection = new ObservableCollection<string>(new string[sourceCollection.Count]);
            for (int i = 0; i < sourceCollection.Count; i++)
            {
                destinationCollection[i] = sourceCollection[i].Value.ToString();
            }
            return destinationCollection;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var sourceCollection = value as ObservableCollection<string>;
            var destinationCollection = new ObservableCollection<ItemNotifyByte>(new ItemNotifyByte[sourceCollection.Count]);
            for (int i = 0; i < sourceCollection.Count; i++)
            {
                if (destinationCollection[i] == null)
                    destinationCollection[i] = new ItemNotifyByte();
                destinationCollection[i].Value = byte.Parse(sourceCollection[i]);
            }
            return destinationCollection;
        }
    }
}
