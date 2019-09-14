using CONTACT_INFO.Model;
using CONTACT_INFO.Windows;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;

namespace CONTACT_INFO.Viewmodel
{
    public class MainModel : BaseViewModel
    {
        private ListCustomer listData;
        private string searchString, inputCOMName;
        private Customer selectedCustomer;
        private bool isOpenDiaglog = true, isExcelExportDialog = false;
        private string numberCalling = "";
        private Thread threadSecurity;

        public ICommand DeleteCustomer { get; set; }
        public ICommand NewCustomer { get; set; }
        public ICommand InfoCustomer { get; set; }
        public ICommand CloseDialog { get; set; }
        public ICommand ExportExcel { get; set; }
        public ICommand BillCustomer { get; set; }

        public ObservableCollection<Customer> ListData { get => listData.List; set { listData.List = value; OnPropertyChanged(); } }
        public string SearchString { get => searchString; set { searchString = value; Search(); OnPropertyChanged(); } }
        public Customer SelectedCustomer { get => selectedCustomer; set { selectedCustomer = value; OnPropertyChanged(); } }
        public bool IsOpenDiaglog { get => isOpenDiaglog; set { isOpenDiaglog = value; OnPropertyChanged(); } }
        public bool IsExcelExportDialog { get => isExcelExportDialog; set { isExcelExportDialog = value; OnPropertyChanged(); } }
        public string InputCOMName { get => inputCOMName; set { inputCOMName = value; Properties.Settings.Default.LoginAccount = value; Properties.Settings.Default.Save(); OnPropertyChanged(); } }

        private SerialPort port;

        public MainModel()
        {
            StaticModel.MainVM = this;
            listData = new ListCustomer();
            searchString = "";
            DeleteCustomer = new RelayCommand<object>((p) => { return true; }, (p) => { Delete(new ObservableCollection<Customer>(((IList)p).Cast<Customer>().ToList())); });
            NewCustomer = new RelayCommand<object>((p) => { return true; }, (p) => { CustomerInfo(null); });
            InfoCustomer = new RelayCommand<object>((p) => { return true; }, (p) => { CustomerInfo(p as Customer); });
            CloseDialog = new RelayCommand<object>((p) => { return true; }, (p) => { OpenPort(); });
            ExportExcel = new RelayCommand<object>((p) => { return true; }, (p) => { _ExportExcel(); });
            BillCustomer = new RelayCommand<object>((p) => { return true; }, (p) => { _BillCustomer(p as Customer); });

            InputCOMName = Properties.Settings.Default.LoginAccount;
            OpenPort();

            threadSecurity = new Thread(_SecurityApp);
            threadSecurity.IsBackground = true;
            threadSecurity.Start();
        }

        private void _SecurityApp()
        {
            bool portIsOpen = false;
            while (true)
            {
                Thread.Sleep(2000);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    portIsOpen = port is null ? false : port.IsOpen;
                });
                if (!portIsOpen) OpenPort();
            }
        }

        private void _BillCustomer(Customer customer)
        {
            StaticModel.BillVM._InfoBill(customer.ID);
        }

        private async void _ExportExcel()
        {
            if (Properties.Settings.Default.Test)
            {
                ShowCustomerCalling("0707743501");
                return;
            }

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
            sheet.Name = "List Customer";

            int col = 0;
            sheet.Cells[2, ++col] = "STT";
            sheet.Cells[2, col].Font.Size = 12;
            sheet.Cells[2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;

            sheet.Cells[2, ++col] = "Họ tên";
            sheet.Cells[2, col].Font.Size = 12;
            sheet.Cells[2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;

            sheet.Cells[2, ++col] = "Địa chỉ";
            sheet.Cells[2, col].Font.Size = 12;
            sheet.Cells[2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;

            sheet.Cells[2, ++col] = "Điện thoại";
            sheet.Cells[2, col].Font.Size = 12;
            sheet.Cells[2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;

            sheet.Cells[2, ++col] = "Loại gas";
            sheet.Cells[2, col].Font.Size = 12;
            sheet.Cells[2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;

            sheet.Cells[2, ++col] = "Ghi chú";
            sheet.Cells[2, col].Font.Size = 12;
            sheet.Cells[2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;

            int num = 0;
            List<Customer> list = null;

            Application.Current.Dispatcher.Invoke(() =>
            {
                list = new ListCustomer().List.ToList();
            });
            foreach (Customer c in list)
            {
                col = 0;
                sheet.Cells[++num + 2, ++col] = num.ToString();
                sheet.Cells[num + 2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;
                sheet.Cells[num + 2, ++col] = c.Fullname;
                sheet.Cells[num + 2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;
                sheet.Cells[num + 2, ++col] = c.Address;
                sheet.Cells[num + 2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;
                sheet.Cells[num + 2, ++col] = c.PhoneText;
                sheet.Cells[num + 2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;
                sheet.Cells[num + 2, ++col] = c.Gastype;
                sheet.Cells[num + 2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;
                sheet.Cells[num + 2, ++col] = c.Note;
                sheet.Cells[num + 2, col].Borders.Weight = Excel.XlBorderWeight.xlThin;
            }
            sheet.UsedRange.Columns.AutoFit();
            sheet.UsedRange.Rows.AutoFit();
            wb.SaveAs(filepath);
            app.Quit();
            Application.Current.Dispatcher.Invoke(() =>
            {
                StaticModel.MainVM.IsExcelExportDialog = false;
                StaticModel.MainVM.IsOpenDiaglog = false;
                MessageBox.Show("Xuất dữ liệu khách hàng ra file excel thành công", "Xuất file excel", MessageBoxButton.OK, MessageBoxImage.Information);
            });

        }
        private void _AddCallingCustomer()
        {
            Customer c = new Customer();
            c.Phone.Add(new Phone(0, numberCalling));
            CustomerInfo(c);
        }

        private void OpenPort()
        {
            IsExcelExportDialog = true;
            try
            {
                // test
                if (Properties.Settings.Default.Test)
                {
                    IsOpenDiaglog = false;
                    return;
                }

                // test

                if (!String.IsNullOrEmpty(inputCOMName))
                {
                    port = new SerialPort(inputCOMName, Properties.Settings.Default.COMBaund, Parity.None, 8, StopBits.One);
                    port.Open();
                    port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
                    IsOpenDiaglog = false;
                }
            }
            catch
            {
                IsExcelExportDialog = false;
                MessageBox.Show("Không kết nối được với cổng COM.\nVui lòng kiểm tra lại thông tin bên dưới", "Kết nối tín hiệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                IsOpenDiaglog = true;
            }
            IsExcelExportDialog = false;
        }


        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (!port.IsOpen)
            {
                if (isOpenDiaglog) isOpenDiaglog = true;
                return;
            }
            string read = port.ReadExisting();
            read = read.Trim().Replace(" ", "");
            numberCalling += read;
            // tách chuỗi bằng regex
            string pattern = @"b([0-9]+)\/";
            // regex
            Regex regex = new Regex(pattern);
            if (regex.IsMatch(numberCalling))
            {
                // split number phone
                string[] arrResult = regex.Split(numberCalling);
                foreach (string phoneNumber in arrResult)
                {
                    ShowCustomerCalling(phoneNumber.Trim());
                }
                //reset data
                numberCalling = "";
            }

            //// Nếu chuỗi được truyền đầy đủ
            //if (read.IndexOf('b') >= 0 && read.IndexOf('/') >= 0)
            //{
            //    numberCalling = read.Substring(read.IndexOf('b') + 1, read.IndexOf('/') - read.IndexOf('b') - 1);
            //    ShowCustomerCalling(numberCalling);
            //    return;
            //}
            //// Nếu chuỗi bị truyền thiếu
            //if (read.IndexOf('b') >= 0)
            //{
            //    // Đọc chuỗi mới
            //    numberCalling = read.Substring(read.IndexOf('b') + 1);
            //}
            //else if (read.IndexOf('/') >= 0)
            //{
            //    numberCalling += read.Substring(0, read.IndexOf('/'));
            //    ShowCustomerCalling(numberCalling);
            //    return;
            //}
            //else
            //{
            //    numberCalling += read;
            //}
        }

        private void ShowCustomerCalling(string phone)
        {
            if (String.IsNullOrEmpty(phone)) return;
            if (phone.Length < 10 || phone.Length > 11) return;
            Application.Current.Dispatcher.Invoke(() =>
            {
                //foreach (Window w in Application.Current.Windows)
                //{
                //    if (w.GetType() == typeof(wCalling))
                //        w.Close();
                //}

                wCalling wCalling = new wCalling();
                phone = phone.Trim();
                phone = phone.Replace(" ", "");
                List<Customer> resultCustomer = ListData.Where(c => (c.Phone.Where(p => (p.Number == phone)).Count() > 0)).ToList();
                if (resultCustomer.Count < 1)
                {
                    wCalling.txtName.Text = phone;
                    wCalling.LastBill.Visibility = Visibility.Collapsed;
                }
                else
                {
                    wCalling.txtNumberCalling.Text = " " + phone;
                    Customer c = resultCustomer.First();
                    Bill b = new Bill();
                    b.IdCustomer = c.ID;
                    b.GetLastBillCustomer();
                    if (b.Id > 0)
                    {
                        wCalling.LastBillDate.Text = "Ngày mua hàng trước đó: " + ((DateTime)b.DateBuy).ToString("dd/MM/yyyy");
                    }
                    b.Id = 0;
                    b.DateBuy = DateTime.Now;
                    b.IdEmployee = -1;
                    wCalling.Bill = b;
                    foreach (Product p in wCalling.Bill.ListProduct)
                    {
                        p.Price = p.productPrice;
                    }
                    wCalling.txtName.Text = c.Fullname;
                    wCalling.txtPhone.Text = c.PhoneText;
                    wCalling.txtNote.Text = c.Note;
                    wCalling.txtAddress.Text = c.Address;
                    wCalling.txtGas.Text = c.Gastype;
                    wCalling.AddCustomer.Visibility = Visibility.Collapsed;
                }
                wCalling.numberCalling = phone;
                wCalling.Show();
            });
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType() == typeof(wCalling))
                    w.Close();
            }
            _AddCallingCustomer();
        }

        public void CustomerInfo(Customer c)
        {
            bool newCustomer = false;
            if (c is null) newCustomer = true;
            else if (c.ID == 0) newCustomer = true;

            wInfoCustomer w = new wInfoCustomer();
            CustomerInfoModel customerInfoModel = w.DataContext as CustomerInfoModel;
            if (newCustomer)
            {
                if (c is null) customerInfoModel.CustomerData = new Customer();
                else customerInfoModel.CustomerData = c;
                w.Show();
            }
            else
            {
                bool flag = false;
                foreach (Window oldw in Application.Current.Windows)
                {
                    if (oldw.GetType() == typeof(wInfoCustomer))
                    {
                        CustomerInfoModel info = (oldw as wInfoCustomer).DataContext as CustomerInfoModel;
                        if (info.CustomerData is null) continue;
                        if (info.CustomerData.ID == c.ID)
                        {
                            flag = true;
                            oldw.Show();
                            oldw.Focus();
                            break;
                        }
                    }
                }
                if (flag) return;
                customerInfoModel.CustomerData = c;
                w.Show();
            }
        }

        public void Search()
        {
            string searchString = this.searchString;

            // Hiển thị load lại toàn bộ danh sách khách hàng
            ListData = new ListCustomer().List;

            // Nếu ko có nội dung tìm kiếm
            if (String.IsNullOrEmpty(searchString)) return;

            // Nếu có nội dung tìm kiếm, parse lại string theo chuẩn: viết thường
            searchString = StaticModel.RemoveSign4VietnameseString(searchString.ToLower());

            ListData = new ObservableCollection<Customer>(ListData.Where(c => (StaticModel.RemoveSign4VietnameseString(c.Fullname.ToLower()).IndexOf(searchString) >= 0 || StaticModel.RemoveSign4VietnameseString(c.Address.ToLower()).IndexOf(searchString) >= 0 || c.PhoneText.ToLower().IndexOf(searchString) >= 0 || StaticModel.RemoveSign4VietnameseString(c.Note.ToLower()).IndexOf(searchString) >= 0 || StaticModel.RemoveSign4VietnameseString(c.Gastype.ToLower()).IndexOf(searchString) >= 0)).ToList<Customer>());
        }

        public void Delete(ObservableCollection<Customer> listDelete)
        {
            if (listDelete is null) return;
            if (listDelete.Count < 1) return;
            if (MessageBox.Show("Bạn có thực sự muốn xóa " + listDelete.Count + " khách hàng đang chọn hay không?\nToàn bộ dữ liệu liên quan tới khách hàng bị xóa cũng sẽ bị xóa theo.", "Xóa khách hàng", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No) return;
            foreach (Customer c in listDelete)
            {
                c.Delete();
                ListData.Remove(c);
            }
        }

    }
}
