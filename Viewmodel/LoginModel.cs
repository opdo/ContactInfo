using CONTACT_INFO.Model;
using CONTACT_INFO.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CONTACT_INFO.Viewmodel
{
    public class LoginModel : BaseViewModel
    {
        private string password;
        private string account, servername, database;
        private string isShow = "Visiable";
        private bool isOpenDiaglog = false;
        public ICommand LoginCommand { get; set; }
      //  private wLoading wLoading = new wLoading();

        public string Account { get => account; set { account = value; OnPropertyChanged(); } }
        public string Password { get => password; set => password = value; }
        public string Database { get => database; set { database = value; OnPropertyChanged(); } }
        public string Servername { get => servername; set { servername = value; OnPropertyChanged(); } }
        public string IsShow { get => isShow; set { isShow = value; OnPropertyChanged(); } }
        public bool IsOpenDiaglog { get => isOpenDiaglog; set { isOpenDiaglog = value; OnPropertyChanged(); }}

        public LoginModel()
        {
            //GetSetting();
            //LoginCommand = new RelayCommand<wLogin>((p) => { return true; }, (p) => { _Login(p); });
            //CheckDatabase(false);
        }

        private void _Login(wLogin p)
        {
            password = p.txtPassword.Password;
            SaveSetting();
            CheckDatabase();
        }

        public void CheckDatabase(bool Message = true)
        {
            Database db = new Database();
            if (!db.Connect())
            {
               // wLoading.Hide();
                //IsOpenDiaglog = false;
                if (Message) MessageBox.Show("Kết nối cơ sở dữ liệu thất bại, vui lòng kiểm tra lại thông tin kết nối", "Kết nối cơ sở dữ liệu", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
               // wLoading.Hide();
                //IsOpenDiaglog = false;
                IsShow = "Hidden";
                wMain w = new wMain();
                w.ShowDialog();
                Environment.Exit(0);
            }
        }


        private void GetSetting()
        {
            Account = Properties.Settings.Default.LoginAccount;
            Servername = Properties.Settings.Default.LoginServername;
            Database = Properties.Settings.Default.LoginDatabase;
        }

        private void SaveSetting()
        {
            Properties.Settings.Default.LoginAccount = account;
            Properties.Settings.Default.LoginPassword = password;
            Properties.Settings.Default.LoginServername = servername;
            Properties.Settings.Default.LoginDatabase = database;
            Properties.Settings.Default.Save();
        }
    }
}
