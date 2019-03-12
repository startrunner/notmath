using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Mathematica
{
    public class HasValueToVisibilityConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture )
        {
            return HasValue() ? Visibility.Visible : Visibility.Hidden;

            bool HasValue()
            {
                if (value is string str)
                    return !string.IsNullOrWhiteSpace(str);
                return value != null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new HasValueToVisibilityConverter();
        }
    }
}
