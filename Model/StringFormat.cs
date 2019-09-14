using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CONTACT_INFO.Model
{
    static class StringFormat
    {
        // Kiểm tra có phải định dạng của số điện thoại hay ko?
        // Một số điện thoại hợp lệ phải từ 6 đến 11 ký tự là số, không chưa các ký tự khác
        static public bool isPhone(string phone)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(phone, "^[0-9]{7,11}$");
        }

        // Định dạng lại tên (full name) của user input vào thành dạng chuẩn
        // Dạng chuẩn là dạng đã cắt hết các ký tự khoảng cách dư thừa chỉ để lại chuỗi in hoa ở mỗi ký tự đầu tiên
        static public string ToName(string name)
        {
            if (name is null) return name;
            // cắt ký tự khoảng cách dư thừa ở hai bên trái, phải chuỗi
            name = name.Trim();
            // cắt các ký tự khoảng cách ở giữa mỗi từ nếu dư
            name = System.Text.RegularExpressions.Regex.Replace(name, @"\s+", " ");
            // kiểm tra string có bị null hay emply hay ko
            if (String.IsNullOrEmpty(name)) return String.Empty;
            // tạo chuỗi mới chứa kết quả là tên chuẩn
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());
        }

        static public string ToSQL(string s, string tiento = "")
        {
            if (s is null) return "NULL";
            s = s.Replace("'", "''");
            if (String.IsNullOrEmpty(s)) return "NULL";
            return "'" + s + "'";
        }
    }
}
