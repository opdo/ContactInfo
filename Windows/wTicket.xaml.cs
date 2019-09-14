using CONTACT_INFO.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Printing;
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
    /// Interaction logic for wTicket.xaml
    /// </summary>
    public partial class wTicket : Window, INotifyPropertyChanged
    {
        private Bill bill;
        public Bill Bill { get => bill; set { bill = value; OnPropertyChanged(); } }
        public wTicket()
        {
            InitializeComponent();
        }
        public void Print()
        {
            this.Show();
            this.Height = HoaDon.ActualHeight;
            PrintDialog printDlg = new PrintDialog();

            printDlg.PrintQueue.UserPrintTicket.PageMediaSize = new System.Printing.PageMediaSize(310, this.HoaDon.ActualWidth);
            printDlg.PrintQueue.DefaultPrintTicket.PageMediaSize = new System.Printing.PageMediaSize(310, this.HoaDon.ActualWidth);
            printDlg.PrintTicket.PageMediaSize = new System.Printing.PageMediaSize(302.3, this.HoaDon.ActualWidth);
            printDlg.PrintTicket.PageOrientation = PageOrientation.Portrait;
            printDlg.PrintQueue.UserPrintTicket.PageOrientation = PageOrientation.Portrait;
            printDlg.PrintQueue.DefaultPrintTicket.PageOrientation = PageOrientation.Portrait;
            //  printDlg.ShowDialog();


            var capabilities = printDlg.PrintQueue.GetPrintCapabilities(printDlg.PrintTicket);

            //get scale of the print wrt to screen of WPF visual
            var scale = Math.Min(capabilities.PageImageableArea.ExtentWidth / this.ActualWidth, capabilities.PageImageableArea.ExtentHeight / this.ActualHeight);
            //Transform the Visual to scale
            this.LayoutTransform = new ScaleTransform(scale, scale);

            // Get the size of the printer page
            var sz = new System.Windows.Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);

            //update the layout of the visual to the printer page size.
            this.Measure(sz);
            this.Arrange(new Rect(new System.Windows.Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight), sz));





            printDlg.PrintVisual(this, "Bill ID " + Bill.Id);
            this.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
