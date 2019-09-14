using CONTACT_INFO.Model;
using CONTACT_INFO.Viewmodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    public partial class wMain : Window
    {
        public wMain()
        {
            // Kiểm tra clone
            try
            {
                Mutex.OpenExisting("Contact Info");
                MessageBox.Show("App đã khởi động trước đó!");
                Environment.Exit(0);
            }
            catch
            {
                StaticModel._mutex = new Mutex(true, "Contact Info");
            }

            InitializeComponent();
        }

        private void btnClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }

        private void ColorZone_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void btnMax_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }

        private void btnMini_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

    }
}
