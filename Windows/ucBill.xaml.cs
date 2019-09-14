using CONTACT_INFO.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CONTACT_INFO.Windows
{
    /// <summary>
    /// Interaction logic for ucBill.xaml
    /// </summary>
    public partial class ucBill : UserControl
    {
        private ObservableCollection<Product> listProduct;

        public ObservableCollection<Product> ListProduct { get => listProduct; set => listProduct = value; }

        public ucBill()
        {
            InitializeComponent();
        }

        public Visibility CustomerId
        {
            get { return (Visibility)GetValue(CustomerIdProperty); }
            set { SetValue(CustomerIdProperty, value); }
        }

        public static DependencyProperty CustomerIdProperty =
            DependencyProperty.Register("CustomerId", typeof(Visibility), typeof(ucBill));

        public Visibility DateBill
        {
            get { return (Visibility)GetValue(DateBillProperty); }
            set { SetValue(DateBillProperty, value); }
        }

        public static DependencyProperty DateBillProperty =
            DependencyProperty.Register("DateBill", typeof(Visibility), typeof(ucBill));

        private void ListProduct2_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            
        }

        private void CheckListUnique()
        {
            int idRemove = -1;
            for (int i = 0; i < ListProduct.Count; i++)
            {
                for (int j = i + 1; j < ListProduct.Count; j++)
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
        }

        private void ListProduct2_CurrentCellChanged(object sender, EventArgs e)
        {
            ListProduct = listProduct2.ItemsSource as ObservableCollection<Product>;
            CheckListUnique();
        }
    }
}
