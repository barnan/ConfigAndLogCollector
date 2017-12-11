using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ConfigAndLogCollectorUI.Converter
{
    public class MessageOnScreenListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<MessageOnScreen> messageScreenList = value as List<MessageOnScreen>;

            if (messageScreenList == null)
            {
                return Binding.DoNothing;
            }

            StringBuilder sb = new StringBuilder(messageScreenList.Count);
            foreach (MessageOnScreen item in messageScreenList)
            {
                sb.Append($"{item.Type} | {item.Message}{Environment.NewLine}");
            }

            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
