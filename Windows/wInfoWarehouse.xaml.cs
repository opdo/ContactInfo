using CONTACT_INFO.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace CONTACT_INFO.Windows
{
    /// <summary>
    /// Interaction logic for wInfoWarehouse.xaml
    /// </summary>
    public partial class wInfoWarehouse : Window, INotifyPropertyChanged
    {
        private Warehouse warehouse;
        public Warehouse Warehouse { get => warehouse; set { warehouse = value; OnPropertyChanged(); } }

        public wInfoWarehouse()
        {
            InitializeComponent();

            if (Warehouse is null)
            {
                Warehouse = new Warehouse();
                Warehouse.DateInput = DateTime.Now;
            }
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
            if (Warehouse.DateInput is null)
            {
                MessageBox.Show("Vui lòng chọn ngày nhập", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }

            if (Warehouse.IdProduct < 1)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }

            if (Warehouse.Count < 1)
            {
                MessageBox.Show("Vui lòng nhập số lượng nhập > 0", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }

            this.Warehouse.Save();
            this.Close();
        }
    }
}
