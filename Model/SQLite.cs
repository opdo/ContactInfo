using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Data;

namespace CONTACT_INFO.Model
{
    public class SQLite
    {
        private SQLiteConnection _con = new SQLiteConnection();
        private SQLiteDataAdapter sqlData;
        private DataSet data;

        public SQLite()
        {
            CreateTable();
            //UpdateDatabase();
        }

        private void Connect()
        {
            string _strConnect = "Data Source=ContactInfo.sqlite;Version=3;";
            _con.ConnectionString = _strConnect;
            _con.Open();
        }

        public void CreateTable()
        {
            string sql = "CREATE TABLE IF NOT EXISTS CUSTOMER (IdCustomer INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, FullnameCustomer nvarchar(50), AddressCustomer nvarchar(255), GasType nvarchar(100), NoteCustomer nvarchar(300))";
            if (!File.Exists("ContactInfo.sqlite")) SQLiteConnection.CreateFile("ContactInfo.sqlite");
            else
            {
                Connect();
                return;
            }
            Connect();
            ExecuteNonQuery(sql);
            sql = "CREATE TABLE IF NOT EXISTS PHONE (IdPhone INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, IdCustomer INTEGER NOT NULL, PhoneNumber varchar(15) NOT NULL, CONSTRAINT fk_IdCustomer FOREIGN KEY(IdCustomer) REFERENCES CUSTOMER(IdCustomer) ON DELETE CASCADE)";
            ExecuteNonQuery(sql);
            UpdateDatabase();
        }

        public void UpdateDatabase()
        {
            string sql = "CREATE TABLE IF NOT EXISTS EMPLOYEE (IdEmployee INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, FullnameEmployee nvarchar(50), PhoneEmployee nvarchar(10))";
            ExecuteNonQuery(sql);
            sql = "CREATE TABLE IF NOT EXISTS [PRODUCT] (IdProduct INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, ProductName nvarchar(50), ProductUnit nvarchar(15))";
            ExecuteNonQuery(sql);
            sql = "CREATE TABLE IF NOT EXISTS [BILL] (IdBill INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, IdCustomer INTEGER, IdEmployee INTEGER, DateBuy DATETIME, CONSTRAINT fk_IdCustomer2 FOREIGN KEY(IdCustomer) REFERENCES CUSTOMER(IdCustomer) ON UPDATE CASCADE, CONSTRAINT fk_IdEmployee2 FOREIGN KEY(IdEmployee) REFERENCES EMPLOYEE(IdEmployee) ON UPDATE CASCADE)";
            ExecuteNonQuery(sql);
            sql = "CREATE TABLE IF NOT EXISTS [BILL_INFO] (IdBill INTEGER NOT NULL, IdProduct INTEGER NOT NULL, Count INTEGER,  Price INTEGER, CONSTRAINT IdBill3 FOREIGN KEY(IdBill) REFERENCES BILL(IdBill) ON DELETE CASCADE, CONSTRAINT fk_IdProduct3 FOREIGN KEY(IdProduct) REFERENCES PRODUCT(IdProduct) ON DELETE CASCADE, CONSTRAINT PK_CUSTID PRIMARY KEY (IdBill, IdProduct))";
            ExecuteNonQuery(sql);
            sql = "CREATE TABLE IF NOT EXISTS [WAREHOUSE_INPUT] (IdInput INTEGER NOT NULL PRIMARY KEY, IdProduct INTEGER NOT NULL, InputCount INTEGER, InputDate DATETIME, InputNote nvarchar(100),CONSTRAINT fk_IdProduct4 FOREIGN KEY(IdProduct) REFERENCES PRODUCT(IdProduct) ON DELETE CASCADE)";
            ExecuteNonQuery(sql);
            try
            {
                sql = "ALTER TABLE [PRODUCT] ADD COLUMN Hidden Integer default 0";
                ExecuteNonQuery(sql);
            }
            catch { }

            try
            {
                sql = "ALTER TABLE [PRODUCT] ADD COLUMN IndexSort Integer default 999999";
                ExecuteNonQuery(sql);
            }

            catch { }

            try
            {
                sql = "ALTER TABLE [PRODUCT] ADD COLUMN ProductNote nvarchar(255)";
                ExecuteNonQuery(sql);
            }

            catch { }

            try
            {
                sql = "ALTER TABLE [PRODUCT] ADD COLUMN ProductPrice Integer";
                ExecuteNonQuery(sql);
            }

            catch {
                Properties.Settings.Default.UpdateDB = 2;
            }

        }

        public DataTable Execute(string sql)
        {
            sqlData = new SQLiteDataAdapter(sql, _con);
            data = new DataSet();
            sqlData.Fill(data);
            return data.Tables[0];
        }

        public void ExecuteNonQuery(string sql)
        {
            SQLiteCommand command = new SQLiteCommand(sql, _con);
            command.ExecuteNonQuery();
        }

    }
}
