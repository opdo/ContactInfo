using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace CONTACT_INFO.Windows
{
    /// <summary>
    /// Interaction logic for wLogin.xaml
    /// </summary>
    public partial class wLogin : Window
    {
        public wLogin()
        {
            InitializeComponent();
        }

        // Bấm nút thoát
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            // Đóng chương trình, exit code = 0
            Environment.Exit(0);
        }

        private void ColorZone_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
