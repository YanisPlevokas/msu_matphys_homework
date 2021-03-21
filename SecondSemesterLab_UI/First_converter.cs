using System;
using FirstSemesterLib;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;

namespace SecondSemesterLab_UI
{
    class First_converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value as DataItem? == null)
                return "object error";
            return ((DataItem)value).electromagnet_field.ToString() + "\nMagnitude: " + ((DataItem)value).electromagnet_field.Magnitude.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
