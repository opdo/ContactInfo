using CONTACT_INFO.Model;
using CONTACT_INFO.Viewmodel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CONTACT_INFO.Windows
{
    /// <summary>
    /// Interaction logic for wInfoBill.xaml
    /// </summary>
    public partial class wInfoBill : Window, INotifyPropertyChanged
    {
        private Bill bill;
        public Bill Bill { get => bill; set { bill = value; OnPropertyChanged(); } }
        public ICommand DeleteProduct { get; set; }
        public ICommand CurrentCellChanged { get; set; }

        
        public wInfoBill()
        {
            InitializeComponent();

          
            DeleteProduct = new RelayCommand<object>((p) => { return true; }, (p) => { _DeleteProduct(p as Product); });
            CurrentCellChanged = new RelayCommand<object>((p) => { return true; }, (p) => { Bill.CheckListUnique(); });

        }

        private void _DeleteProduct(Product product)
        {
            if (product.Id > 0)
            {
                if (MessageBox.Show("Bạn có thực sự muốn xóa sản phẩm " + product.Name + " ra khỏi hóa đơn này hay không?", "Xóa chi tiết hóa đơn", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No) return;
            }
            Bill.ListProduct.Remove(product);
            Bill.CalTotalMoney();
        }

        public void LoadDataProduct()
        {
            //if (Bill is null) return;
            //if (Bill.Id < 1) return;
            //Bill.ListProduct = new ObservableCollection<Product>();
            //SQLite sql = new SQLite();
            //DataTable data = sql.Execute("select a.*, a.Count as [TotalCount], b.ProductName, b.ProductUnit, ([Price]*[Count]) as [TotalMoney] from [BILL_INFO] a, [PRODUCT] b where a.IdProduct = b.IdProduct and IdBill = " + Bill.Id);
            //foreach (DataRow r in data.Rows)
            //{
            //    Product p = new Product(r);
            //    p.AutoSave = false;
            //    p.STT = Bill.ListProduct.Count + 1;
            //    Bill.ListProduct.Add(p);
            //}
            //Bill.ListProduct = Bill.ListProduct;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            //if (Bill.IdCustomer < 1)
            //{
            //    MessageBox.Show("Vui lòng chọn khách hàng", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            //    return;
            //}

            //if (Bill.IdEmployee < 1)
            //{
            //    MessageBox.Show("Vui lòng chọn nhân viên giao hàng", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            //    return;
            //}

            if (Bill.ListProduct.Count < 1)
            {
                MessageBox.Show("Vui lòng chọn ít nhất 1 sản phẩm hoặc dịch vụ", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }

            foreach (Product p in Bill.ListProduct)
            {
                if (p is null)
                {
                    MessageBox.Show("Một sản phẩm hoặc dịch vụ trong hóa đơn không hợp lệ", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    return;
                }
                if (p.Id < 1)
                {
                    MessageBox.Show("Một sản phẩm hoặc dịch vụ trong hóa đơn không hợp lệ", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    return;
                }
            }
            try
            {
                this.Bill.Save();
                MessageBox.Show("Lưu hóa đơn thành công\nMã hóa đơn là " + this.Bill.Id, "Lưu hóa đơn", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu dữ liệu\n" + ex.Message, "Lưu hóa đơn", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Close();
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void ColorZone_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Product p = new Product();
            p.AutoSave = false;
            p.STT = Bill.ListProduct.Count + 1;
            p.Count = 1;
            Bill.ListProduct.Add(p);
            Bill.ListProduct = Bill.ListProduct;
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            this.Bill.Print();
        }
    }
}
