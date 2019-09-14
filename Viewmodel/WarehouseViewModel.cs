using CONTACT_INFO.Model;
using CONTACT_INFO.Windows;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System.Threading.Tasks;
using System.Threading;

namespace CONTACT_INFO.Viewmodel
{
    public class WarehouseViewModel : BaseViewModel
    {
        private ObservableCollection<Product> list, list2;
        private DateTime? fromDate, toDate;
        private Product selectedProduct;
        private Warehouse selectedWarehouse;
        private int totalCount, totalCountOutput, totalCountRemain;
        public ObservableCollection<Product> List { get => list; set { list = value; OnPropertyChanged(); } }
        public Product SelectedProduct { get => selectedProduct; set { selectedProduct = value; if (value != null) UpdateTotalCount(); OnPropertyChanged(); } }


        public Warehouse SelectedWarehouse { get => selectedWarehouse; set { selectedWarehouse = value; OnPropertyChanged(); } }
        public DateTime? FromDate { get => fromDate; set { fromDate = value; GetListProduct(); OnPropertyChanged(); } }
        public DateTime? ToDate { get => toDate; set { toDate = value; GetListProduct(); OnPropertyChanged(); } }
        public int TotalCount { get => totalCount; set { totalCount = value; OnPropertyChanged(); } }
        public int TotalCountOutput { get => totalCountOutput; set { totalCountOutput = value; OnPropertyChanged(); } }
        public int TotalCountRemain { get => totalCountRemain; set { totalCountRemain = value; OnPropertyChanged(); } }

        public ICommand DeleteWarehouse { get; set; }
        public ICommand NewWarehouse { get; set; }
        public ICommand InfoWarehouse { get; set; }
        public ICommand ExportExcel { get; set; }

        public WarehouseViewModel()
        {
            FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ToDate = ((DateTime)FromDate).AddMonths(1).AddDays(-1);
            list = new ObservableCollection<Product>();
            list2 = new ObservableCollection<Product>();
            StaticModel.WarehouseVM = this;
            GetListProduct();

            // command
            DeleteWarehouse = new RelayCommand<object>((p) => { return true; }, (p) => { _DeleteWarehouse(); });
            NewWarehouse = new RelayCommand<object>((p) => { return true; }, (p) => {   SelectedWarehouse = null; _InfoWarehouse(); });
            InfoWarehouse = new RelayCommand<object>((p) => { return true; }, (p) => { _InfoWarehouse(); });
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
                StaticModel.MainVM.IsExcelExportDialog = true;
                StaticModel.MainVM.IsOpenDiaglog = true;
            });

            Excel.Application app = new Excel.Application();
            Excel.Workbook wb = app.Workbooks.Add(Type.Missing);
            Excel._Worksheet sheet = null;
            sheet = wb.ActiveSheet;
            sheet.Name = "List Warehouse";
            int col = 0;
            sheet.Cells[2, ++col] = "STT";
            sheet.Cells[2, col].Font.Size = 12;
            sheet.Cells[2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;

            sheet.Cells[2, ++col] = "Ngày";
            sheet.Cells[2, col].Font.Size = 12;
            sheet.Cells[2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;

            sheet.Cells[2, ++col] = "Sản phẩm";
            sheet.Cells[2, col].Font.Size = 12;
            sheet.Cells[2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;

            sheet.Cells[2, ++col] = "Số lượng";
            sheet.Cells[2, col].Font.Size = 12;
            sheet.Cells[2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;

            sheet.Cells[2, ++col] = "Ghi chú";
            sheet.Cells[2, col].Font.Size = 12;
            sheet.Cells[2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;

            int num = 0;

            ObservableCollection<Product> list = null;
            Application.Current.Dispatcher.Invoke(() =>
            {
                list = new ObservableCollection<Product>(List);
            });
            foreach (Product p in list)
            {
                foreach (Warehouse w in p.List)
                {
                    col = 0;
                    sheet.Cells[++num + 2, ++col] = num.ToString();
                    sheet.Cells[num + 2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;
                    sheet.Cells[num + 2, ++col] = ((DateTime)w.DateInput).ToString("dd/MM/yyyy");
                    sheet.Cells[num + 2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;
                    sheet.Cells[num + 2, ++col] = p.Name;
                    sheet.Cells[num + 2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;
                    sheet.Cells[num + 2, ++col] = w.Count;
                    sheet.Cells[num + 2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;
                    sheet.Cells[num + 2, ++col] = w.Note;
                    sheet.Cells[num + 2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;
                }
            }
            sheet.UsedRange.Columns.AutoFit();
            sheet.UsedRange.Rows.AutoFit();
            wb.SaveAs(filepath);
            app.Quit();

            Application.Current.Dispatcher.Invoke(() =>
            {
                StaticModel.MainVM.IsExcelExportDialog = false;
                StaticModel.MainVM.IsOpenDiaglog = false;
                MessageBox.Show("Xuất dữ liệu nhập xuất kho ra file excel thành công", "Xuất file excel", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        private void _DeleteWarehouse()
        {
            if (SelectedWarehouse.Id < 1) return;
            if (SelectedWarehouse.Count < 1) return;
            if (SelectedWarehouse.IdBill > 0) return;
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa phiếu nhập này hay không?", "Xóa phiếu nhập", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                SelectedWarehouse.Delete();
                GetListProduct();
            }
        }

        private void _InfoWarehouse()
        {
            wInfoWarehouse w = new wInfoWarehouse();
            w.Warehouse = new Warehouse(SelectedWarehouse);
            w.Warehouse.AutoSave = false;
            if (w.Warehouse.Id < 1) w.Warehouse.DateInput = DateTime.Now;
            w.ShowDialog();
            GetListProduct();
        }

        private void UpdateTotalCount()
        {
            if (SelectedProduct is null) return;
            TotalCount = 0;
            TotalCountOutput = 0;
            foreach (Warehouse w in SelectedProduct.List)
            {
                if (w.Count < 0) TotalCountOutput += -w.Count;
                else TotalCount += w.Count;
            }
            TotalCountRemain = TotalCount - TotalCountOutput;
        }

        public void GetListProduct()
        {
            // date
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
            string sql = "select *, Ifnull(ifnull((select sum(InputCount) from [WAREHOUSE_INPUT] t0 where t0.IdProduct = a.IdProduct and t0.InputDate < " + from + "), 0) - ifnull((select sum([Count]) from [BILL] t1, [BILL_INFO] t2 where t1.IdBill = t2.IdBill and t2.IdProduct = a.IdProduct and  DateBuy < " + from + "), 0), 0) as [LastTotalCount], Ifnull(ifnull((select sum(InputCount) from [WAREHOUSE_INPUT] t0 where t0.IdProduct = a.IdProduct and t0.InputDate >= " + from + " and t0.InputDate <= " + to + "), 0) - ifnull((select sum([Count]) from [BILL] t1, [BILL_INFO] t2 where t1.IdBill = t2.IdBill and t2.IdProduct = a.IdProduct and DateBuy >= "+from+" and DateBuy <= " + to + "), 0), 0) as [TotalCount] from [PRODUCT] a left join (select * from [WAREHOUSE_INPUT] where InputDate >= " + from + " and InputDate <= " + to + ") b on a.IdProduct = b.IdProduct left join (select t1.DateBuy, t2.* from [BILL] t1, [BILL_INFO] t2 where t1.IdBill = t2.IdBill and t1.DateBuy >= " + from + " and t1.DateBuy <= " + to + ") c on a.IdProduct = c.IdProduct where a.Hidden = 0 order by a.IndexSort";
            SQLite sQLite = new SQLite();
            DataTable data = sQLite.Execute(sql);
            foreach (DataRow r in data.Rows)
            {
                int id = int.Parse(r["IdProduct"].ToString().Trim());
                Product e = list2.Where(x => x.Id == id).FirstOrDefault();
                if (e is null)
                {
                    e = new Product(r);
                    e.Count += int.Parse(r["LastTotalCount"].ToString().Trim());
                    e.STT = ++count;
                    List.Add(e);
                    list2.Add(e);
                }

                // Add tồn trước nếu có
                if (e.List.Count < 1)
                {
                    try
                    {
                        int lastCount = 0;
                        lastCount = int.Parse(r["LastTotalCount"].ToString().Trim());
                        if (lastCount > 0)
                        {
                            Warehouse tontruoc = new Warehouse(r);
                            tontruoc.Id = 0;
                            tontruoc.DateInput = null;
                            tontruoc.STT = 1;
                            tontruoc.Note = "Tồn trước đó";
                            tontruoc.Count = lastCount;
                            e.List.Add(tontruoc);
                        }
                    }
                    catch { }
                }
                // Add nhập hàng
                try
                {
                    Warehouse nhaphang = new Warehouse(r);
                    
                    if (nhaphang.Id > 0)
                    {
                        if (!e.List.Any(x => x.Id == nhaphang.Id))
                        {
                            nhaphang.STT = e.List.Count + 1;
                            e.List.Add(nhaphang);
  
                        }
                    }
                }
                catch { }

                // Add hóa đơn
                try
                {
                    Warehouse hoadon = new Warehouse(r);
                    int mahoadon = 0;
                    mahoadon = int.Parse(r["IdBill"].ToString().Trim());
                    if (mahoadon > 0)
                    {
                        if (!e.List.Any(x => x.IdBill == mahoadon))
                        {
                            hoadon.Id = 0;
                            hoadon.DateInput = Convert.ToDateTime(r["DateBuy"].ToString().Trim());
                            hoadon.Count = -int.Parse(r["Count"].ToString().Trim());
                            hoadon.Note = "Hóa đơn số " + mahoadon;
                            hoadon.IdBill = mahoadon;
                            hoadon.STT = e.List.Count + 1;
                            e.List.Add(hoadon);
    
                        }    
                    }
                }
                catch { }
                


            }

            foreach (Product p in List)
            {
                p.List = new ObservableCollection<Warehouse>(p.List.OrderBy(x => x.DateInput));
                count = 0;
                foreach (Warehouse w in p.List)
                {
                    w.STT = ++count;
                }
            }
            UpdateTotalCount();
        }

    }
}
