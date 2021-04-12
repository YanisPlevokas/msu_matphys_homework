using System;
using FirstSemesterLib;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;

namespace SecondSemesterLab_UI
{
    class Third_converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value as V4DataOnGrid == null)
                return value?.ToString();
            if (parameter as int? == null)
                return "parameter error : " + parameter?.ToString() + " (" + parameter?.GetType() + ")";
            int param = (int)parameter;
            V4DataOnGrid data = (V4DataOnGrid)value;
            double returned;
            if (param < 0)
            // Output min
            {
                returned = data.Aggregate((max, v) => v.electromagnet_field.Magnitude > max.electromagnet_field.Magnitude ? v : max).electromagnet_field.Magnitude;
                return returned.ToString() + " - max magn";
            }
            else
            // Output max
            {
                returned = data.Aggregate((min, v) => v.electromagnet_field.Magnitude < min.electromagnet_field.Magnitude ? v : min).electromagnet_field.Magnitude;
                return returned.ToString() + " - min magn";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
