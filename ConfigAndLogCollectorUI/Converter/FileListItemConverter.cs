using System;
using System.Globalization;
using System.Windows.Data;

namespace ConfigAndLogCollectorUI.Converter
{
    public class FileListItemConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 3)
            {
                return Binding.DoNothing;
            }

            try
            {
                string netName = (string)values[0];
                string version = (string)values[1];
                string path = (string)values[2];

                if (version == null)
                {
                    version = "0.0.0.0";
                }

                return $"{netName} {version} {path}";
            }
            catch (Exception)
            {
                return Binding.DoNothing;
            }


        }



        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
