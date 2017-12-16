using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using BaseClasses;

namespace ConfigAndLogCollectorUI.Converter
{
    class EllipseColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            State state = value as State;

            if (state == null)
            {
                return Binding.DoNothing;
            }

            if (state == State.Error)
            {
                return new SolidColorBrush(Colors.Red);
            }
            if (state == State.Idle)
            {
                return new SolidColorBrush(Colors.Orange);
            }
            if (state == State.InProgress)
            {
                return new SolidColorBrush(Colors.Yellow);
            }
            if (state == State.Ready)
            {
                return new SolidColorBrush(Colors.Yellow);
            }

            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
