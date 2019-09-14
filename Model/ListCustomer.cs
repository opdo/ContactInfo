using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CONTACT_INFO.Model
{
    public class ListCustomer
    {
        private ObservableCollection<Customer> list = new ObservableCollection<Customer>();

        public ObservableCollection<Customer> List { get => list; set => list = value; }

        public ListCustomer()
        {
            // Số thứ tự
            int stt = 0;

            //Database db = new Database();
            SQLite db = new SQLite();
            DataTable data = db.Execute("select CUSTOMER.*, PHONE.IdPhone, PhoneNumber from CUSTOMER left join PHONE on CUSTOMER.IdCustomer = PHONE.IdCustomer order by CUSTOMER.IdCustomer DESC");
            for (int i = 0; i < data.Rows.Count; i++)
            {
                Customer c = new Customer(data.Rows[i]);
                c.AutoSave = true;
                List<Customer> resultList = List.Where(x => (x.ID == c.ID)).ToList();
                if (resultList.Count < 1)
                {
                    // Không tồn tại, add member
                    List.Add(c);
                    c = List.Last();
                    c.STT = ++stt;
                }
                else
                {
                    c = resultList[0];
                }

                // Kiểm tra string phone có tồn tại
                if (data.Rows[i]["PhoneNumber"] is null) continue;
                if (String.IsNullOrEmpty(data.Rows[i]["PhoneNumber"].ToString())) continue;
                Phone p = new Phone(Convert.ToInt32(data.Rows[i]["IdPhone"].ToString()), data.Rows[i]["PhoneNumber"].ToString().Replace(" ", ""));
                c.Phone.Add(p);
                if (String.IsNullOrEmpty(c.FirstPhone)) c.FirstPhone = p.Number;
            }
        }

        public void Clone(ListCustomer listnew)
        {
            List.Clear();
            foreach (Customer c in listnew.List)
            {
                List.Add(c);
            }
        }
    }
}
