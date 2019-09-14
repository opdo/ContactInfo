using CONTACT_INFO.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace CONTACT_INFO.Model
{
    public class PrinterControl
    {
        private string printerName;

        public string PrinterName { get => printerName; set => printerName = value; }

        public PrinterControl()
        { }

        public PrinterControl(string printerName)
        {
            this.PrinterName = printerName;
        }

        [STAThread]
        public void CheckPrinterStatus()
        {
            if (String.IsNullOrEmpty(PrinterName)) throw new Exception("2");
            string query = string.Format("SELECT * from Win32_Printer WHERE Name LIKE '{0}'", printerName);

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            using (ManagementObjectCollection coll = searcher.Get())
            {
                try
                {
                    foreach (ManagementObject printer in coll)
                    {
                        bool isOffline = bool.Parse(printer.Properties["WorkOffline"].Value.ToString());
                        if (isOffline) throw new Exception("Máy in không hoạt động");
                        return;
                    }
                }
                catch (ManagementException ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            throw new Exception("Không tìm thấy máy in");
        }

        [STAThread]
        public void PrintTicket(Bill bill)
        {
            wTicket w = new wTicket();
            w.Bill = bill;
            w.txtTitle.Text = Properties.Settings.Default.BillTitle;
            string phone = "<Không tìm thấy>", name = "<Không tìm thấy>", address = "<Không tìm thấy>";
            Customer c = StaticModel.MainVM.ListData.Where(x => x.ID == bill.IdCustomer).FirstOrDefault();
            if (c != null)
            {
                phone = c.PhoneText;
                name = c.Fullname;
                address = c.Address;
            }
            string info = "Mã hóa đơn: "+ bill.Id +"\nKhách hàng: " + name + "\nĐiện thoại: " + phone + "\nĐịa chỉ: " + address;
            
            name = "<Không tìm thấy>";
            phone = name;
            Employee e = StaticModel.EmployeeVM.List.Where(x => x.Id == bill.IdEmployee).FirstOrDefault();
            if (e != null)
            {
                phone = e.Phone;
                name = e.Name;
            }
            info += "\nNhân viên: " + name + " ("+phone+")";
            w.txtDetails.Text = info;
            string detail = String.Format("{0,-10}{1}\n", "#", "Sản phẩm");
            int count = 0;
            foreach (Product p in bill.ListProduct)
            {
                Product pp = StaticModel.ProductVM.List.Where(x => x.Id == p.Id).FirstOrDefault();
                if (pp != null) p.Name = pp.Name;
                detail += String.Format("{0,-10}{1}\n- SL {2} * {3} = {4} VNĐ\n", ++count, p.Name, p.Count, String.Format("{0:n0}", p.Price), String.Format("{0:n0}", p.Count * p.Price));

            }
            w.BillDetails.Text = detail;
            w.txtSmallInfo.Text = "In vào ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " lúc " + DateTime.Now.ToString("HH:mm:ss");
            w.Print();
        }
    }
}
