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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CONTACT_INFO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // Kiểm tra clone
            Mutex _mutex;
            try
            {
                Mutex.OpenExisting("Contact Info");
                MessageBox.Show("App đã khởi động trước đó!");
                Environment.Exit(0);
            }
            catch
            {
                _mutex = new Mutex(true, "Contact Info");
            }

            InitializeComponent();
        }
    }
}
