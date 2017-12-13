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
            try
            {
                State state = (State)Enum.Parse(typeof(State), value.ToString());

                switch (state)
                {
                    case State.Error:
                        return new SolidColorBrush(Colors.Red);
                    case State.InProgress:
                        return new SolidColorBrush(Colors.Yellow);
                    case State.Idle:
                        return new SolidColorBrush(Colors.Green);
                    default:
                        return new SolidColorBrush(Colors.Gray);
                }
            }
            catch (Exception)
            {
                return Binding.DoNothing;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
