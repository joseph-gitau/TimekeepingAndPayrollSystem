using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace TimekeepingAndPayrollSystem
{
    public class ClientNameToInverseVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string clientName = value as string;
            if (clientName == "Weekend/Holiday/Vacation")
                return Visibility.Collapsed; // Hide for special row
            return Visibility.Visible; // Show for all other rows
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
