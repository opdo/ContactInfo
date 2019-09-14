using CONTACT_INFO.Model;
using CONTACT_INFO.Viewmodel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for wCalling.xaml
    /// </summary>
    public partial class wCalling : Window, INotifyPropertyChanged
    {
        public string numberCalling;
        private Bill bill;
        public Bill Bill { get => bill; set { bill = value; OnPropertyChanged(); } }
        public ICommand DeleteProduct { get; set; }
        public ICommand CurrentCellChanged { get; set; }


        public wCalling()
        {
            InitializeComponent();
            DeleteProduct = new RelayCommand<object>((p) => { return true; }, (p) => { _DeleteProduct(p as Product); });
            CurrentCellChanged = new RelayCommand<object>((p) => { return true; }, (p) => { Bill.CheckListUnique(); });

        }
        private void _AddCallingCustomer()
        {
            Customer c = new Customer();
            c.Phone.Add(new Phone(0, numberCalling));
            StaticModel.MainVM.CustomerInfo(c);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cardInfo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
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
                this.Bill.Print();
                MessageBox.Show("Lưu hóa đơn thành công\nMã hóa đơn là " + this.Bill.Id, "Lưu hóa đơn", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu dữ liệu\n" + ex.Message, "Lưu hóa đơn", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            _AddCallingCustomer();
            this.Close();
        }
    }
}
