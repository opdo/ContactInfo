using CONTACT_INFO.Viewmodel;
using System;
using System.Data;

namespace CONTACT_INFO.Model
{
    public class Phone: BaseViewModel
    {
        private int id;
        private string number;

        public string Number { get => number; set { number = value; OnPropertyChanged(); } }
        public int ID { get => id; set { id = value; OnPropertyChanged(); }}

        public Phone()
        { }

        public Phone(int id, string number)
        {
            this.id = id;
            this.number = number;
        }

        public Phone(Phone x)
        {
            CopyData(x);
        }

        public void CopyData(Phone x)
        {
            this.id = x.id;
            this.number = x.number;
        }

        public void Delete()
        {
            if (this.id < 1) return;
            SQLite db = new SQLite();
            db.ExecuteNonQuery("delete from PHONE where IdPhone=" + this.ID);
        }

        public void Save(int idCustomer)
        {
            string sql = "";
            SQLite db = new SQLite();
            if (this.id > 0)
            {
                // Update dữ liệu
                sql = String.Format("update PHONE set PhoneNumber={0}, IdCustomer={1} where IdPhone={2}",
                                    StringFormat.ToSQL(this.number.Replace(" ", "")),
                                    idCustomer,
                                    this.id);
            }
            else
            {
                // Thêm mới dữ liệu
                sql = String.Format("insert into PHONE (IdCustomer, PhoneNumber) values ({1}, {0})",
                                    StringFormat.ToSQL(this.number.Replace(" ", "")),
                                    idCustomer);
            }
            db.ExecuteNonQuery(sql);

            // Update lại mã khách hàng nếu là insert
            if (this.id < 1)
            {
                DataTable dataTable = db.Execute("select Max(idPhone) from PHONE");
                this.id = Convert.ToInt32(dataTable.Rows[0][0].ToString());
            }
        }
    }
}
