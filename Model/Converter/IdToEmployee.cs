using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CONTACT_INFO.Model.Converter
{
    public class IdToEmployee : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int id = 0;
            if (!int.TryParse(value.ToString().Trim(), out id)) return "<Không tìm thấy>";
            Employee e = StaticModel.EmployeeVM.List.Where(x => x.Id == id).FirstOrDefault();
            if (e is null) return "<Không tìm thấy>";
            return e.Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
