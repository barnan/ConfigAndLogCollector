using System;
using System.Globalization;
using System.Windows.Data;

namespace ConfigAndLogCollectorUI.Converter
{
    public class ShareListItemConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
            {
                return Binding.DoNothing;
            }

            string name = values[0] as string;
            string serverName = values[1] as string;

            if (name == null || serverName == null)
            {
                return Binding.DoNothing;
            }

            return name + " - " + serverName;

        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
