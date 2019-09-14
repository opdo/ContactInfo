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
    public class Product : BaseViewModel
    {
        private int id, count, sTT;
        private decimal price, totalMoney;
        private string name;
        private string unit;
        public decimal productPrice;
        private string note;
        public bool AutoSave;
        private ObservableCollection<Warehouse> list;
        public int Id { get => id; set { id = value; OnPropertyChanged(); } }
        public int Count { get => count; set { count = value; if (AutoSave) Save(); TotalMoney = Count * Price; OnPropertyChanged(); } }
        public decimal Price { get => price; set { price = value; if (AutoSave) Save(); TotalMoney = Count * Price; OnPropertyChanged(); } }
        public string Name { get => name; set { name =  StaticModel.ToUpperTitle(value); if (AutoSave) Save(); OnPropertyChanged(); } }
        public int STT { get => sTT; set { sTT = value; OnPropertyChanged(); } }
        public string Unit { get => unit; set { unit = StaticModel.ToUpperTitle(value); if (AutoSave) Save(); OnPropertyChanged(); } }
        public ObservableCollection<Warehouse> List { get => list; set { list = value; OnPropertyChanged(); } }
        public string Note { get => note; set { note = value; if (AutoSave) Save(); OnPropertyChanged(); } }

        //private void SortList()
        //{
        //    List = new ObservableCollection<Warehouse>(List.OrderBy(x => x.DateInput));
        //    int count = 0;
        //    foreach (Warehouse w in List)
        //    {
        //        w.STT = ++count;
        //    }
        //}

        public decimal TotalMoney { get => totalMoney; set { totalMoney = value; OnPropertyChanged(); } }


        public Product()
        {
            List = new ObservableCollection<Warehouse>();
        }
        public Product(Product x)
        {
            if (x is null) return;
            this.Id = x.Id;
            this.Name = x.Name;
            this.Price = x.Price;
            this.STT = x.STT;
            this.Count = x.Count;
            this.Unit = x.Unit;
        }
        public Product(DataRow x)
        {
            List = new ObservableCollection<Warehouse>();
            Copy(x);
        }

        private void Copy(DataRow r)
        {
            this.Id = Convert.ToInt32(r["idProduct"].ToString());
            this.Name = r["ProductName"].ToString().Trim();
            this.Unit = r["ProductUnit"].ToString().Trim();
            if (r.Table.Columns.Contains("ProductNote")) this.Note = r["ProductNote"].ToString().Trim();
            if (r.Table.Columns.Contains("ProductPrice"))
            {
                this.Price = Convert.ToInt32(String.IsNullOrEmpty(r["ProductPrice"].ToString()) ? "0" : r["ProductPrice"].ToString());
                productPrice = Price;
            }
            if (r.Table.Columns.Contains("Price"))
            {
                decimal _price = 0;
                decimal.TryParse(r["Price"].ToString(), out _price);
                this.Price = _price;
            }
            if (r.Table.Columns.Contains("TotalCount"))
            {

                int _count = 0;
                int.TryParse(r["TotalCount"].ToString(), out _count);
                this.Count = _count;
            }


        }


        public void Save()
        {
            string sql = "";
            SQLite db = new SQLite();
            if (this.id > 0)
            {
                // Update dữ liệu
                sql = String.Format("update PRODUCT set ProductName={0}, ProductUnit={1}, ProductNote={2}, ProductPrice={3} where IdProduct={4}",
                                    StringFormat.ToSQL(this.name, "N"),
                                    StringFormat.ToSQL(this.Unit, "N"),
                                    StringFormat.ToSQL(this.Note, "N"),
                                    this.Price,
                                    this.id);
            }
            else
            {
                // Thêm mới dữ liệu
                sql = String.Format("insert into PRODUCT (ProductName, ProductUnit, ProductNote, ProductPrice) values ({0}, {1}, {2}, {3})",
                                    StringFormat.ToSQL(this.name, "N"),
                                    StringFormat.ToSQL(this.Unit, "N"), 
                                    StringFormat.ToSQL(this.Note, "N"),
                                    this.Price);
            }
            db.ExecuteNonQuery(sql);

            // Update lại mã khách hàng nếu là insert
            if (this.id < 1)
            {
                DataTable dataTable = db.Execute("select Max(IdProduct) from PRODUCT");
                this.id = Convert.ToInt32(dataTable.Rows[0][0].ToString());
            }
        }

        public void SaveSort()
        {
            SQLite db = new SQLite();
            int index = this.STT;
            if (this.id < 1) return;
            string sql = String.Format("update PRODUCT set IndexSort = " + index + " where idProduct = " + this.Id);
            db.ExecuteNonQuery(sql);

        }

        public void Delete()
        {
            if (this.id < 1) return;
            SQLite db = new SQLite();
            db.ExecuteNonQuery("update PRODUCT set Hidden=1 where IdProduct=" + this.Id);
        }

    }
    
}
