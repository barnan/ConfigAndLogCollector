using BaseClasses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ConfigAndLogCollectorUI.Converter
{
    class OptionListItemConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length > 2)
            {
                return Binding.DoNothing;
            }

            string name = values[0] as string;
            List<ArchPath> pathList = values[1] as List<ArchPath>;

            if (name == null || pathList == null)
            {
                return Binding.DoNothing;
            }

            StringBuilder sb = new StringBuilder(name, pathList.Count);

            foreach (ArchPath item in pathList)
            {
                sb.Append($" | {item.Path}");
            }

            return sb.ToString();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
