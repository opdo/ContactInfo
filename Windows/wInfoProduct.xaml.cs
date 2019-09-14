using CONTACT_INFO.Model;
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
    /// Interaction logic for wInfoProduct.xaml
    /// </summary>
    public partial class wInfoProduct : Window, INotifyPropertyChanged
    {
        private Product product;

        public Product Product { get => product; set { product = value; OnPropertyChanged(); } }

        public wInfoProduct()
        {
            InitializeComponent();
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

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(Product.Name))
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }

            if (String.IsNullOrEmpty(Product.Unit))
            {
                MessageBox.Show("Vui lòng nhập tên đơn vị tính", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }
            this.Product.Save();
            this.Close();
        }
    }
}
