using CONTACT_INFO.Viewmodel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CONTACT_INFO.Model
{
    public class Warehouse : BaseViewModel
    {
        private int id, sTT;
        private int idProduct;
        private int count;
        private string note;
        private DateTime? dateInput;
        private int idBill { get; set; }

        public int Id { get => id; set { id = value; OnPropertyChanged(); } }
        public int IdProduct { get => idProduct; set { idProduct = value; OnPropertyChanged(); } }
        public int Count { get => count; set { count = value; OnPropertyChanged(); } }
        public DateTime? DateInput { get => dateInput; set { dateInput = value; OnPropertyChanged(); } }
        public string Note { get => note; set { note = value; OnPropertyChanged(); } }
        public int IdBill { get => idBill; set { idBill = value; OnPropertyChanged(); } }

        public bool AutoSave { get; internal set; }
        public int STT { get => sTT; set { sTT = value; OnPropertyChanged(); } }


        public Warehouse()
        {
        }

        public Warehouse(Warehouse x)
        {
            if (x is null) return;
            this.Id = x.Id;
            this.IdProduct = x.IdProduct;
            this.Count = x.Count;
            this.DateInput = x.DateInput;
            this.Note = x.Note;
            this.IdBill = x.IdBill;

        }

        public Warehouse(DataRow x)
        {
            AutoSave = true;
            Copy(x);
        }

        private void Copy(DataRow r)
        {
            try
            {
                this.Id = Convert.ToInt32(r["IdInput"].ToString());
            }
            catch
            {
                return;
            }
            this.IdProduct = Convert.ToInt32(r["IdProduct"].ToString());
            this.Count = Convert.ToInt32(r["InputCount"].ToString());
            this.DateInput = Convert.ToDateTime(r["InputDate"].ToString());
            this.Note = r["InputNote"].ToString().Trim();

        }

        public void Save()
        {
            string sql = "";
            SQLite db = new SQLite();
            if (this.id > 0)
            {
                // Update dữ liệu
                sql = String.Format("update WAREHOUSE_INPUT set IdProduct={0}, InputCount={1}, InputDate = {2}, InputNote = {3} where IdInput={4}",
                                    this.IdProduct,
                                    this.Count,
                                    DateInput is null ? "NULL" : "'" + ((DateTime)DateInput).ToString("yyyy-MM-dd HH:mm:ss") + "'",
                                    StringFormat.ToSQL(this.Note, "N"),
                                    this.id);
            }
            else
            {
                // Thêm mới dữ liệu
                sql = String.Format("insert into WAREHOUSE_INPUT (IdProduct, InputCount, InputNote, InputDate) values ({0}, {1}, {2}, {3})",
                                    this.IdProduct,
                                    this.Count,
                                    StringFormat.ToSQL(this.Note, "N"),
                                    DateInput is null ? "NULL" : "'" + ((DateTime)DateInput).ToString("yyyy-MM-dd HH:mm:ss") + "'");
            }
            db.ExecuteNonQuery(sql);

            // Update lại mã khách hàng nếu là insert
            if (this.id < 1)
            {
                DataTable dataTable = db.Execute("select Max(IdInput) from WAREHOUSE_INPUT");
                this.id = Convert.ToInt32(dataTable.Rows[0][0].ToString());
            }

        }

        public void Delete()
        {
            if (this.id < 1) return;
            SQLite db = new SQLite();
            db.ExecuteNonQuery("delete from WAREHOUSE_INPUT where IdInput=" + this.id);
        }
    }
}
