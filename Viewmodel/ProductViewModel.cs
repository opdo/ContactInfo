using CONTACT_INFO.Model;
using CONTACT_INFO.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CONTACT_INFO.Viewmodel
{
    public class ProductViewModel : BaseViewModel
    {
        private ObservableCollection<Product> list, list2;
        private Product selectedProduct;
        private string searchString;
        public ObservableCollection<Product> List { get => list; set { list = value; OnPropertyChanged(); } }
        public Product SelectedProduct { get => selectedProduct; set { selectedProduct = value; OnPropertyChanged(); } }
        public string SearchString { get => searchString; set { searchString = value; Search(); OnPropertyChanged(); } }

        public ICommand DeleteProduct { get; set; }
        public ICommand NewProduct { get; set; }
        public ICommand InfoProduct { get; set; }
        public ICommand DownSortCommand { get; set; }
        public ICommand UpSortCommand { get; set; }


        public ProductViewModel()
        {
            List = new ObservableCollection<Product>();
            list2 = new ObservableCollection<Product>();
            SelectedProduct = null;
            GetListProduct();
            StaticModel.ProductVM = this;

            // command
            DeleteProduct = new RelayCommand<object>((p) => { return true; }, (p) => { _DeleteProduct(); });
            NewProduct = new RelayCommand<object>((p) => { return true; }, (p) => { SelectedProduct = null;  _InfoProduct(); });
            InfoProduct = new RelayCommand<object>((p) => { return true; }, (p) => { _InfoProduct(); });
            UpSortCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortProduct(p as Product, -1); });
            DownSortCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortProduct(p as Product, 1); });

        }

        private void SortProduct(Product product, int v)
        {
            Product p = List.Where(x => x.Id == product.Id).FirstOrDefault();
            if (p is null) return;
            int index = List.IndexOf(p);
            if (index + v >= List.Count || index + v < 0) return;
            try
            {
                List.Move(index, index + v);
            }
            catch
            {
                return;
            }
            
            List[index].STT -= v;
            List[index+v].STT += v;
            List[index].SaveSort();
            List[index + v].SaveSort();
            //StaticModel.WarehouseVM.GetListProduct();
        }

        private void _InfoProduct()
        {
            wInfoProduct w = new wInfoProduct();
            w.Product = new Product(SelectedProduct);
            w.Product.AutoSave = false;
            w.ShowDialog();
            GetListProduct();
        }

        public void _DeleteProduct()
        {
            if (SelectedProduct is null) return;
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm " + SelectedProduct.Name + " hay không?", "Xóa sản phẩm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                SelectedProduct.Delete();
                GetListProduct();
                StaticModel.BillVM.GetListBill();
                StaticModel.WarehouseVM.GetListProduct();
            }
        }

        public void GetListProduct()
        {
            int count = 0;
            List.Clear();
            list2.Clear();
            SQLite sQLite = new SQLite();
            DataTable data = sQLite.Execute("select * from [Product] where Hidden = 0 order by IndexSort");
            foreach (DataRow r in data.Rows)
            {
                Product e = new Product(r);
                e.STT = ++count;
                List.Add(e);
                list2.Add(e);
            }
            List = List;
            foreach (Product p in list)
            {
                p.AutoSave = true;
            }
        }

        public void Search()
        {
            string searchString = this.searchString;

            // Nếu ko có nội dung tìm kiếm
            if (String.IsNullOrEmpty(searchString)) return;

            // Nếu có nội dung tìm kiếm, parse lại string theo chuẩn: viết thường
            searchString = searchString.ToLower();

            List = new ObservableCollection<Product>(list2.Where(c => (c.Name.ToLower().IndexOf(searchString) >= 0 || c.Unit.ToLower().IndexOf(searchString) >= 0)).ToList<Product>());
        }
    }
}
