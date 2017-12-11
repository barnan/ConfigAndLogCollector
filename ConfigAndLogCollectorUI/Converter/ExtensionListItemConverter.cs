using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ConfigAndLogCollectorUI.Converter
{
    class ExtensionListItemConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string path = values[0] as string;
            if (path == null)
            {
                return Binding.DoNothing;
            }

            int dayNumber = 0;
            try
            {
                dayNumber = (int) values[1];
            }
            catch (Exception)
            {
                return Binding.DoNothing;
            }

            string placeholder = dayNumber < 10 ? " " : string.Empty;

            return $"{dayNumber}{placeholder} | {path}";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
