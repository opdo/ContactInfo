using CONTACT_INFO.Viewmodel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CONTACT_INFO.Model
{
    public class Bill : BaseViewModel
    {
        private int id, sTT;
        private decimal totalMoney;
        private int idCustomer, idEmployee;
        private DateTime? dateBuy;
        private ObservableCollection<Product> listProduct = new ObservableCollection<Product>();

        public ObservableCollection<Product> ListProduct { get => listProduct; set { listProduct = value; CheckListUnique(); CalTotalMoney(); OnPropertyChanged(); } }



        public int IdCustomer { get => idCustomer; set { idCustomer = value; OnPropertyChanged(); } }
        public int IdEmployee { get => idEmployee; set { idEmployee = value; OnPropertyChanged(); } }
        public decimal TotalMoney { get => totalMoney; set { totalMoney = value; OnPropertyChanged(); } }
        public int Id { get => id; set { id = value; OnPropertyChanged(); } }
        public DateTime? DateBuy { get => dateBuy; set { dateBuy = value; OnPropertyChanged(); } }
        public int STT { get => sTT; set { sTT = value; OnPropertyChanged(); } }

        public void GetLastBillCustomer()
        {
            if (idCustomer < 1) return;
            SQLite sQLite = new SQLite();
            string sql = "select a.IdBill, a.IdCustomer, a.IdEmployee, a.DateBuy, b.IdProduct, c.ProductName, c.ProductUnit, b.Count as [TotalCount], b.Price, (b.Price*b.Count) as [TotalMoney]  from [BILL] a, [BILL_INFO] b, [PRODUCT] c where a.IdBill = b.IdBill and b.IdProduct = c.IdProduct and a.IdBill in (select idBill from Bill where IdCustomer = " + idCustomer + " order by DateBuy DESC limit 1)";
            DataTable data = sQLite.Execute(sql);
            foreach (DataRow r in data.Rows)
            {
               
                if (Id < 1)
                {
                    Copy(r);
                    Product p = new Product(r);
                    p.STT = ListProduct.Count + 1;
                    ListProduct.Add(p);
                }
                else
                {
                    Product p = new Product(r);
                    p.STT = ListProduct.Count + 1;
                    ListProduct.Add(p);
                }

            }

            CalTotalMoney();
        }
        public Bill()
        {
            DateBuy = DateTime.Now;
        }
        public Bill(Bill x)
        {
            if (x is null)
            {
                DateBuy = DateTime.Now;
                return;
            }
            this.Id = x.Id;
            this.IdCustomer = x.IdCustomer;
            this.IdEmployee = x.IdEmployee;
            this.DateBuy = x.DateBuy;
            this.STT = x.STT;
            this.TotalMoney = x.TotalMoney;
            this.ListProduct = new ObservableCollection<Product>(x.ListProduct);
        }
        public Bill(DataRow x)
        {
            Copy(x);
        }

        public void CalTotalMoney()
        {
            TotalMoney = 0;
            foreach (Product p in ListProduct)
            {
                TotalMoney += p.TotalMoney;
            }
        }

        private void Copy(DataRow r)
        {
            this.Id = Convert.ToInt32(r["idBill"].ToString());
            this.DateBuy = Convert.ToDateTime(r["DateBuy"].ToString());

            if (r.Table.Columns.Contains("IdCustomer")) this.IdCustomer = String.IsNullOrEmpty(r["IdCustomer"].ToString()) ? 0 : Convert.ToInt32(r["IdCustomer"].ToString());
            if (r.Table.Columns.Contains("IdEmployee")) this.IdEmployee = String.IsNullOrEmpty(r["IdEmployee"].ToString()) ? 0 : Convert.ToInt32(r["IdEmployee"].ToString());
            if (r.Table.Columns.Contains("TotalMoney")) this.TotalMoney = String.IsNullOrEmpty(r["TotalMoney"].ToString()) ? 0 : Convert.ToInt32(r["TotalMoney"].ToString());
        }

        public void Delete()
        {
            if (this.id < 1) return;
            SQLite db = new SQLite();
            db.ExecuteNonQuery("delete from [BILL] where IdBill=" + this.id);
        }

        internal void Print()
        {
            if (this.id < 1)
            {
                MessageBox.Show("Vui lòng lưu lại hóa đơn trước khi in", "In hóa đơn", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            PrinterControl printerControl = new PrinterControl(Properties.Settings.Default.PrinterName);
            try
            {
                printerControl.CheckPrinterStatus();
                printerControl.PrintTicket(this);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "In hóa đơn", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void CheckListUnique()
        {
            int idRemove = -1;
            for (int i = 0; i < ListProduct.Count; i++)
            {
                for (int j = i+1; j < ListProduct.Count; j++)
                {
                    if (ListProduct[i].Id == ListProduct[j].Id)
                    {
                        idRemove = j;
                        break;
                    }
                }
                if (idRemove > 0) break;
            }
            if (idRemove > 0)
            {
                ListProduct.RemoveAt(idRemove);
                CheckListUnique();
            }
            CalTotalMoney();
        }

        public void Save()
        {
            string sql = "";
            SQLite db = new SQLite();
            if (this.id > 0)
            {
                // Update dữ liệu
                sql = String.Format("update BILL set IdCustomer={0}, IdEmployee={1} where IdBill={2}",
                                    IdCustomer == 0 ? "NULL" : idCustomer.ToString(),
                                    IdEmployee == 0 ? "NULL" : IdEmployee.ToString(),
                                    this.id);
            }
            else
            {
                // Thêm mới dữ liệu
                sql = String.Format("insert into BILL (IdCustomer, IdEmployee, DateBuy) values ({0}, {1}, '{2}')",
                                    IdCustomer == 0 ? "NULL" : idCustomer.ToString(),
                                    IdEmployee == 0 ? "NULL" : IdEmployee.ToString(),
                                    ((DateTime)DateBuy).ToString("yyyy-MM-dd HH:mm:ss"));
            }
            db.ExecuteNonQuery(sql);

            // Update lại mã khách hàng nếu là insert
            if (this.id < 1)
            {
                DataTable dataTable = db.Execute("select Max(IdBill) from BILL");
                this.id = Convert.ToInt32(dataTable.Rows[0][0].ToString());
            }


            // delete all and add product to bill
            sql = String.Format("delete from BILL_INFO where IdBill = " + this.id);
            db.ExecuteNonQuery(sql);
            foreach (Product x in ListProduct)
            {
                sql = String.Format("insert into BILL_INFO (IdBill, IdProduct, Count, Price) values ({0}, {1}, {2}, {3})",
                                    this.id,
                                    x.Id,
                                    x.Count,
                                    x.Price);
                db.ExecuteNonQuery(sql);
            }
            StaticModel.BillVM.GetListBill();
            StaticModel.WarehouseVM.GetListProduct();
        }
    }
}
