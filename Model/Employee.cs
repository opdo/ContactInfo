using CONTACT_INFO.Viewmodel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CONTACT_INFO.Model
{
    public class Employee : BaseViewModel
    {
        private int id, sTT;
        private string name, phone;
        public bool AutoSave;
        public string Name { get => name; set { name = StringFormat.ToName(value); if (AutoSave) Save(); OnPropertyChanged(); } }
        public string Phone { get => phone; set { phone = value; if (AutoSave) Save(); OnPropertyChanged(); } }
        public int Id { get => id; set { id = value; OnPropertyChanged(); } }
        public int STT { get => sTT; set { sTT = value; OnPropertyChanged(); } }

        public Employee()
        {
        }

        public Employee(Employee x)
        {
            if (x is null) return;
            this.Id = x.Id;
            this.Name = x.Name;
            this.Phone = x.Phone;
            this.AutoSave = x.AutoSave;
            this.STT = x.STT;
        }

        public Employee(DataRow x)
        {
            Copy(x);
        }

        private void Copy(DataRow r)
        {
            this.Id = Convert.ToInt32(r["idEmployee"].ToString());
            this.Name = r["FullnameEmployee"].ToString().Trim();
            this.Phone = r["PhoneEmployee"] is null ? "" : r["PhoneEmployee"].ToString().Trim();
        }

        public void Save()
        {
            string sql = "";
            SQLite db = new SQLite();
            if (this.id > 0)
            {
                // Update dữ liệu
                sql = String.Format("update EMPLOYEE set FullnameEmployee={0}, PhoneEmployee={1} where IdEmployee={2}",
                                    StringFormat.ToSQL(this.name, "N"),
                                    StringFormat.ToSQL(this.phone),
                                    this.id);
            }
            else
            {
                // Thêm mới dữ liệu
                sql = String.Format("insert into EMPLOYEE (FullnameEmployee, PhoneEmployee) values ({0}, {1})",
                                    StringFormat.ToSQL(this.name, "N"),
                                    StringFormat.ToSQL(this.phone));
            }
            db.ExecuteNonQuery(sql);

            // Update lại mã khách hàng nếu là insert
            if (this.id < 1)
            {
                DataTable dataTable = db.Execute("select Max(IdEmployee) from EMPLOYEE");
                this.id = Convert.ToInt32(dataTable.Rows[0][0].ToString());
            }

        }

        public void Delete()
        {
            if (this.id < 1) return;
            SQLite db = new SQLite();
            db.ExecuteNonQuery("delete from [EMPLOYEE] where IdEmployee=" + this.id);
        }
    }
}
