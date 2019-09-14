using CONTACT_INFO.Model;
using System;
using System.Collections;
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
    public class CustomerInfoModel: BaseViewModel
    {
        private Customer customer, local_customer;
        private bool isOpenDiaglog = false;
        private string contentDialog = "";
        private bool isAddPhoneDialog = false;
        private string inputPhoneNumber = "";
        private Phone selectedPhone;

        public Customer CustomerData { get => customer; set { customer = value; Customer = new Customer(customer); } }
        public Customer Customer { get => local_customer; set { local_customer = value; OnPropertyChanged(); } }
        public ICommand SaveInfo { get; set; }
        public ICommand AddPhone { get; set; }
        public ICommand CloseDialog { get; set; }
        public ICommand DeletePhone { get; set; }
        public bool IsOpenDiaglog { get => isOpenDiaglog; set { isOpenDiaglog = value; OnPropertyChanged(); } }
        public bool IsAddPhoneDialog { get => isAddPhoneDialog; set { isAddPhoneDialog = value; OnPropertyChanged(); } }
        public string ContentDialog { get => contentDialog; set { contentDialog = value; OnPropertyChanged(); } }
        public string InputPhoneNumber { get => inputPhoneNumber; set { inputPhoneNumber = value; OnPropertyChanged(); } }
        public Phone SelectedPhone { get => selectedPhone; set { selectedPhone = value; OnPropertyChanged(); } }

        public CustomerInfoModel()
        {
            StaticModel.CustomerVM = this;
            if (customer is null) customer = new Customer();
            local_customer = new Customer(customer);
            local_customer.AutoSave = false;
            SaveInfo = new RelayCommand<object>((p) => { return true; }, (p) => { Save(); });
            AddPhone = new RelayCommand<object>((p) => { return true; }, (p) => { _AddPhone(); });
            CloseDialog = new RelayCommand<object>((p) => { return true; }, (p) => { _CloseDialog(); });
            DeletePhone = new RelayCommand<object>((p) => { return true; }, (p) => { Delete(new ObservableCollection<Phone>(((IList)p).Cast<Phone>().ToList())); });
        }


        public void Delete(ObservableCollection<Phone> listDelete)
        {
            if (listDelete is null) return;
            if (listDelete.Count < 1) return;
            if (MessageBox.Show("Bạn có thực sự muốn xóa " + listDelete.Count + " số điện thoại đang chọn hay không?", "Xóa số điện thoại", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No) return;
            foreach (Phone c in listDelete)
            {
                c.Delete();
                Customer.Phone.Remove(c);
            }
        }
        public void _CloseDialog()
        {
            if (IsAddPhoneDialog)
            {
                if (!String.IsNullOrEmpty(inputPhoneNumber))
                {
                    inputPhoneNumber = inputPhoneNumber.Replace(" ", "");
                    if (StringFormat.isPhone(inputPhoneNumber))
                    {
                        SQLite db = new SQLite();
                        DataTable dataTable = db.Execute("select * from PHONE where PhoneNumber='" + inputPhoneNumber + "'");
                        if (dataTable.Rows.Count > 0)
                        {
                            MessageBox.Show("Số điện thoại bạn nhập đã thuộc về một khách hàng khác, vui lòng nhập số khác", "Thêm số điện thoại", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        else
                        {
                            Phone p = new Phone(0, inputPhoneNumber);
                            if (Customer.ID > 0) p.Save(Customer.ID);
                            OnPropertyChanged("PhoneText");
                            Customer.Phone.Add(p);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Số điện thoại nhập vào không hợp lệ", "Thêm số điện thoại", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }
            InputPhoneNumber = "";
            IsOpenDiaglog = false;
        }

        public void _AddPhone()
        {
            InputPhoneNumber = "";
            IsAddPhoneDialog = true;
            ContentDialog = "Vui lòng nhập số điện thoại cần thêm và ấn đồng ý.\nĐể hủy bạn có thể ấn bất kỳ vị trí nào trên màn hình";
            IsOpenDiaglog = true;
        }

        public void Save()
        {
            IsAddPhoneDialog = false;
            if (String.IsNullOrEmpty(local_customer.Fullname))
            {
                ContentDialog = "Vui lòng nhập họ tên khách hàng";
            }
            else
            {
                if (Customer.ID <= 0) Customer.Save();
                if (customer is null)
                {
                    customer = new Customer();
                }
                else
                {
                    customer.CopyData(local_customer);
                    customer.Save();
                }

                ContentDialog = "Lưu dữ liệu hoàn tất";
                StaticModel.MainVM.ListData = new ListCustomer().List;
            }

            IsOpenDiaglog = true;
        }
    }
}
