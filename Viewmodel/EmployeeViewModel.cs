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
    public class EmployeeViewModel : BaseViewModel
    {
        private ObservableCollection<Employee> list, list2;
        private Employee selectedEmployee;
        private string searchString;
        public ObservableCollection<Employee> List { get => list; set { list = value; OnPropertyChanged(); } }
        public Employee SelectedEmployee { get => selectedEmployee; set { selectedEmployee = value; OnPropertyChanged(); } }
        public string SearchString { get => searchString; set { searchString = value; Search(); OnPropertyChanged(); } }

        public ICommand DeleteEmployee { get; set; }
        public ICommand NewEmployee { get; set; }
        public ICommand InfoEmployee { get; set; }


        public EmployeeViewModel()
        {
            List = new ObservableCollection<Employee>();
            list2 = new ObservableCollection<Employee>();
            SelectedEmployee = null;
            GetListEmployee();
            StaticModel.EmployeeVM = this;

            // command
            DeleteEmployee = new RelayCommand<object>((p) => { return true; }, (p) => { _DeleteEmployee(); });
            NewEmployee = new RelayCommand<object>((p) => { return true; }, (p) => { SelectedEmployee = null; _InfoEmployee(); });
            InfoEmployee = new RelayCommand<object>((p) => { return true; }, (p) => { _InfoEmployee(); });

        }

        private void _InfoEmployee()
        {
            wInfoEmployee w = new wInfoEmployee();
            w.Employee = new Employee(SelectedEmployee);
            w.Employee.AutoSave = false;
            w.ShowDialog();
            GetListEmployee();
        }

        public void _DeleteEmployee()
        {
            if (SelectedEmployee is null) return;
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên " + SelectedEmployee.Name + " hay không?", "Xóa nhân viên", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                SelectedEmployee.Delete();
                GetListEmployee();
            }
        }

        public void GetListEmployee()
        {
            int count = 0;
            List.Clear();
            list2.Clear();
            SQLite sQLite = new SQLite();
            DataTable data = sQLite.Execute("select * from [EMPLOYEE] order by IdEmployee DESC");
            foreach (DataRow r in data.Rows)
            {
                Employee e = new Employee(r);
                e.STT = ++count;
                List.Add(e);
                list2.Add(e);
            }
            List = List;
            foreach (Employee p in list)
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
            searchString = StaticModel.RemoveSign4VietnameseString(searchString.ToLower());

            List = new ObservableCollection<Employee>(list2.Where(c => (StaticModel.RemoveSign4VietnameseString(c.Name.ToLower()).IndexOf(searchString) >= 0 || c.Phone.ToLower().IndexOf(searchString) >= 0)).ToList<Employee>());
        }
    }
}
