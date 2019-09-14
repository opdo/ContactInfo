using CONTACT_INFO.Viewmodel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CONTACT_INFO.Model
{
    public class Customer: BaseViewModel
    {
        private int id, sTT;
        private string fullname, address, gastype, note, firstPhone;
        private ObservableCollection<Phone> phone = new ObservableCollection<Phone>();
        public bool AutoSave = false;

        public int STT { get => sTT; set { sTT = value; OnPropertyChanged(); } }
        public int ID { get => id; set => id = value; }
        public string Fullname { get => fullname; set { fullname = StringFormat.ToName(value); if (AutoSave) Save(); OnPropertyChanged(); } }
        public string Address { get => address; set { address = StaticModel.ToUpperTitle(value); if (AutoSave) Save(); OnPropertyChanged(); } }
        public string Gastype { get => gastype; set { gastype = StaticModel.ToUpperTitle(value); if (AutoSave) Save(); OnPropertyChanged(); } }
        public string Note { get => note; set { note = StaticModel.ToUpperTitle(value); if (AutoSave) Save(); OnPropertyChanged(); } }
        public string PhoneText { get => GetPhoneText(); }
        public ObservableCollection<Phone> Phone { get => phone; set { phone = value; GetPhoneText(); OnPropertyChanged(); OnPropertyChanged("PhoneText"); } }

        public string FirstPhone { get => firstPhone; set { firstPhone = value; OnPropertyChanged(); } }

        public Customer()
        {

        }

        public Customer(DataRow r)
        {
            this.id = Convert.ToInt32(r["idCustomer"].ToString());
            this.fullname = r["FullnameCustomer"].ToString().Trim();
            this.address = r["AddressCustomer"] is null ? "" : r["AddressCustomer"].ToString().Trim();
            this.gastype = r["GasType"] is null ? "" : r["GasType"].ToString().Trim();
            this.note = r["NoteCustomer"] is null ? "" : r["NoteCustomer"].ToString().Trim();
        }

        public Customer(int id, string fullname, string address, string gastype, string note)
        {
            this.id = id;
            this.fullname = fullname;
            this.address = address;
            this.gastype = gastype;
            this.note = note;
        }

        public Customer(Customer x)
        {
            CopyData(x);
        }

        public void Delete()
        {
            if (this.id  < 1) return;
            SQLite db = new SQLite();
            db.ExecuteNonQuery("delete from CUSTOMER where IdCustomer=" + this.ID);
        }

        public void Save()
        {
            string sql = "";
            SQLite db = new SQLite();
            if (this.id > 0)
            {
                // Update dữ liệu
                sql = String.Format("update CUSTOMER set FullnameCustomer={0}, AddressCustomer={1}, GasType={2}, NoteCustomer={3} where IdCustomer={4}",
                                    StringFormat.ToSQL(this.fullname, "N"),
                                    StringFormat.ToSQL(this.address, "N"),
                                    StringFormat.ToSQL(this.gastype, "N"),
                                    StringFormat.ToSQL(this.note, "N"),
                                    this.id);
            }
            else
            {
                // Thêm mới dữ liệu
                sql = String.Format("insert into CUSTOMER (FullnameCustomer, AddressCustomer, Gastype, NoteCustomer) values ({0}, {1}, {2}, {3})",
                                    StringFormat.ToSQL(this.fullname, "N"),
                                    StringFormat.ToSQL(this.address, "N"),
                                    StringFormat.ToSQL(this.gastype, "N"),
                                    StringFormat.ToSQL(this.note, "N"));
            }
            db.ExecuteNonQuery(sql);

            // Update lại mã khách hàng nếu là insert
            if (this.id < 1)
            {
                DataTable dataTable = db.Execute("select Max(idCustomer) from CUSTOMER");
                this.id = Convert.ToInt32(dataTable.Rows[0][0].ToString());
            }

            // Save lại toàn bộ các số điện thoại
            foreach (Phone p in this.Phone)
            {
                p.Save(this.id);
            }
        }



        public void CopyData(Customer x)
        {
            if (x is null) return;
            this.ID = x.id;
            this.Fullname = x.fullname;
            this.Address = x.address;
            this.Gastype = x.gastype;
            this.Note = x.note;
            this.Phone.Clear();
            foreach (Phone p in x.Phone)
            {
                this.Phone.Add(new Phone(p));
            }
            Phone = Phone;
        }

        public string GetPhoneText()
        {
            FirstPhone = "";
            string phoneText = "";
            foreach (Phone p in this.phone)
            {
                if (String.IsNullOrEmpty(FirstPhone)) FirstPhone = p.Number;
                phoneText += p.Number + "\n";
            }
            
            return phoneText.Trim();
        }
    }
}
