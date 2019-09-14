using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CONTACT_INFO.Model.Converter
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int type = 0;
            if (!(parameter is null))
            {
                int.TryParse(parameter.ToString(), out type);
            }

            bool? b = value as bool?;
            if (b is null) return type == 0 ? Visibility.Collapsed : Visibility.Visible;

            if ((bool)b) return type == 0 ? Visibility.Visible : Visibility.Collapsed;
            return type == 0 ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
