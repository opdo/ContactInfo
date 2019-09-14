using CONTACT_INFO.Model;
using CONTACT_INFO.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System.Threading;

namespace CONTACT_INFO.Viewmodel
{
    public class BillViewModel : BaseViewModel
    {
        private int? idEmployee, idCustomer;
        private decimal totalMoney;
        private ObservableCollection<Bill> list, list2;
        private DateTime? fromDate, toDate;
        private Bill selectedBill;
        public ObservableCollection<Bill> List { get => list; set { list = value; OnPropertyChanged(); } }
        public Bill SelectedBill { get => selectedBill; set { selectedBill = value; OnPropertyChanged(); } }
        public DateTime? FromDate { get => fromDate; set { fromDate = value; GetListBill(); OnPropertyChanged(); } }
        public DateTime? ToDate { get => toDate; set { toDate = value; GetListBill(); OnPropertyChanged(); } }
        public int? IdEmployee { get => idEmployee; set { idEmployee = value; GetListBill(); OnPropertyChanged(); } }
        public int? IdCustomer { get => idCustomer; set { idCustomer = value; GetListBill(); OnPropertyChanged(); } }

        public decimal TotalMoney { get => totalMoney; set { totalMoney = value; OnPropertyChanged(); } }

        public ICommand DeleteBill { get; set; }
        public ICommand NewBill { get; set; }
        public ICommand InfoBill { get; set; }
        public ICommand ExportExcel { get; set; }



        public BillViewModel()
        {
            FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ToDate = ((DateTime)FromDate).AddMonths(1).AddDays(-1);

            List = new ObservableCollection<Bill>();
            list2 = new ObservableCollection<Bill>();
            StaticModel.BillVM = this;
            GetListBill();

            // command
            DeleteBill = new RelayCommand<object>((p) => { return true; }, (p) => { _DeleteBill(); });
            NewBill = new RelayCommand<object>((p) => { return true; }, (p) => { SelectedBill = null; _InfoBill(); });
            InfoBill = new RelayCommand<object>((p) => { return true; }, (p) => { _InfoBill(); });
            ExportExcel = new RelayCommand<object>((p) => { return true; }, (p) => { _ExportExcel(); });

        }

        private async void _ExportExcel()
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.DefaultExt = "xlsx";
            saveFile.Filter = "Excel xlsx (*.xlsx)|*.xlsx|Excel xls (*.xls)|*.xls";
            saveFile.CheckPathExists = true;
            if (saveFile.ShowDialog() == true)
            {
                try
                {
                    await Task.Run(() =>
                    {
                        _Export(saveFile.FileName);
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    StaticModel.MainVM.IsOpenDiaglog = false;
                    StaticModel.MainVM.IsExcelExportDialog = false;
                }
            }
        }

        private void _Export(string filepath)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                StaticModel.MainVM.IsOpenDiaglog = true;
                StaticModel.MainVM.IsExcelExportDialog = true;

            });

            Excel.Application app = new Excel.Application();
            Excel.Workbook wb = app.Workbooks.Add(Type.Missing);
            Excel._Worksheet sheet = null;
            sheet = wb.ActiveSheet;
            sheet.Name = "List Bill";
            int col = 0;
            sheet.Cells[2, ++col] = "STT";
            sheet.Cells[2, col].Font.Size = 12;
            sheet.Cells[2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;

            sheet.Cells[2, ++col] = "Mã HĐ";
            sheet.Cells[2, col].Font.Size = 12;
            sheet.Cells[2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;

            sheet.Cells[2, ++col] = "Khách hàng";
            sheet.Cells[2, col].Font.Size = 12;
            sheet.Cells[2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;

            sheet.Cells[2, ++col] = "Điện thoại";
            sheet.Cells[2, col].Font.Size = 12;
            sheet.Cells[2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;

            sheet.Cells[2, ++col] = "Nhân viên";
            sheet.Cells[2, col].Font.Size = 12;
            sheet.Cells[2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;

            sheet.Cells[2, ++col] = "Sản phẩm";
            sheet.Cells[2, col].Font.Size = 12;
            sheet.Cells[2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;

            sheet.Cells[2, ++col] = "Số lượng";
            sheet.Cells[2, col].Font.Size = 12;
            sheet.Cells[2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;

            sheet.Cells[2, ++col] = "Đơn giá";
            sheet.Cells[2, col].Font.Size = 12;
            sheet.Cells[2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;

            sheet.Cells[2, ++col] = "Thành tiền";
            sheet.Cells[2, col].Font.Size = 12;
            sheet.Cells[2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;

            int num = 0;

            ObservableCollection<Bill> list = null;
            Application.Current.Dispatcher.Invoke(() =>
            {
                list = new ObservableCollection<Bill>(List);
            });

            foreach (Bill b in list)
            {
                foreach (Product p in b.ListProduct)
                {
                    col = 0;
                    sheet.Cells[++num + 2, ++col] = num.ToString();
                    sheet.Cells[num + 2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;
                    sheet.Cells[num + 2, ++col] = b.Id.ToString();
                    Customer c = StaticModel.MainVM.ListData.Where(x => x.ID == b.IdCustomer).FirstOrDefault();
                    string name, phone;
                    if (c != null)
                    {
                        name = c.Fullname;
                        phone = c.PhoneText;
                    }
                    else
                    {
                        name = "<Không tìm thấy>";
                        phone = "<Không tìm thấy>";
                    }
                    sheet.Cells[num + 2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;
                    sheet.Cells[num + 2, ++col] = name;
                    sheet.Cells[num + 2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;
                    sheet.Cells[num + 2, ++col] = phone;
                    Employee e = StaticModel.EmployeeVM.List.Where(x => x.Id == b.IdEmployee).FirstOrDefault();
                    if (e is null)
                    {
                        name = "<Không tìm thấy>";
                    }
                    else
                    {
                        name = e.Name;
                    }
                    sheet.Cells[num + 2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;
                    sheet.Cells[num + 2, ++col] = name;

                    sheet.Cells[num + 2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;
                    sheet.Cells[num + 2, ++col] = p.Name;
                    sheet.Cells[num + 2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;
                    sheet.Cells[num + 2, ++col] = p.Count;
                    sheet.Cells[num + 2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;
                    sheet.Cells[num + 2, ++col] = p.Price;
                    sheet.Cells[num + 2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;
                    sheet.Cells[num + 2, ++col] = (p.Count * p.Price);

                    sheet.Cells[num + 2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;
                }
            }
            sheet.UsedRange.Columns.AutoFit();
            sheet.UsedRange.Rows.AutoFit();
            wb.SaveAs(filepath);
            app.Quit();

            Application.Current.Dispatcher.Invoke(() =>
            {
                StaticModel.MainVM.IsOpenDiaglog = false;
                StaticModel.MainVM.IsExcelExportDialog = false;
                MessageBox.Show("Xuất dữ liệu hóa đơn ra file excel thành công", "Xuất file excel", MessageBoxButton.OK, MessageBoxImage.Information);

            });


        }

        private void _DeleteBill()
        {
            if (SelectedBill.Id < 1) return;
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa hóa đơn này hay không?", "Xóa phiếu nhập", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                SelectedBill.Delete();
                GetListBill();
            }
        }

        public void _InfoBill(int idCustomer = 0)
        {
            wInfoBill w = new wInfoBill();
            w.Bill = new Bill(SelectedBill);
            if (idCustomer > 0) w.Bill.IdCustomer = idCustomer;
            w.LoadDataProduct();
            w.Show();
        }

        public void GetListBill()
        {
            string from = "1500-1-1 00:00:00", to = "2999-12-30 23:59:59";
            if (FromDate != null) from = ((DateTime)FromDate).ToString("yyyy-MM-dd") + " 00:00:00";
            if (ToDate != null) to = ((DateTime)ToDate).ToString("yyyy-MM-dd") + " 23:59:59";
            from = "'" + from + "'";
            to = "'" + to + "'";


            int count = 0;
            try
            {
                List.Clear();
                list2.Clear();
            }
            catch
            {
                return;
            }
            SQLite sQLite = new SQLite();
            string sql = "select a.IdBill, a.IdCustomer, a.IdEmployee, a.DateBuy, b.IdProduct, c.ProductName, c.ProductUnit, b.Count as [TotalCount], b.Price, (b.Price*b.Count) as [TotalMoney]  from [BILL] a, [BILL_INFO] b, [PRODUCT] c where a.IdBill = b.IdBill and b.IdProduct = c.IdProduct and a.DateBuy >= " + from + " and a.DateBuy <= " + to;
            if (IdEmployee != null) sql += " and a.IdEmployee = " + IdEmployee;
            if (IdCustomer != null) sql += " and a.IdCustomer = " + IdCustomer;
            sql += " order by a.DateBuy DESC";
            DataTable data = sQLite.Execute(sql);
            foreach (DataRow r in data.Rows)
            {
                int idBill = int.Parse(r["IdBill"].ToString());
                Bill e = List.Where(x => x.Id == idBill).FirstOrDefault();
                if (e is null)
                {
                    e = new Bill(r);
                    e.STT = ++count;
                    List.Add(e);
                    Product p = new Product(r);
                    p.STT = e.ListProduct.Count + 1;
                    e.ListProduct.Add(p);
                }
                else
                {
                    Product p = new Product(r);
                    p.STT = e.ListProduct.Count + 1;
                    e.ListProduct.Add(p);
                }

            }

            TotalMoney = 0;
            foreach (Bill b in List)
            {
                b.CalTotalMoney();
                TotalMoney += b.TotalMoney;
            }
            List = List;
        }

    }
}
