using System;
using System.Windows.Data;

namespace GazeMonitoring {
    public class BooleanAndConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if (values == null) {
                return false;
            }

            foreach (object value in values) {
                if ((value is bool b) && b == false) {
                    return false;
                }
            }
            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture) {
            return null;
        }
    }
}