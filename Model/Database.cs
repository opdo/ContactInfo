using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CONTACT_INFO.Model
{
    class Database
    {
        private SqlConnection sqlConn;
        private SqlDataAdapter sqlData;
        private DataSet data;
        public Database()
        {
            Connect();
        }

        public bool Connect(string sqlConnectString = null)
        {
            if (sqlConnectString is null)
            {
                if (String.IsNullOrEmpty(Properties.Settings.Default.LoginServername) || String.IsNullOrEmpty(Properties.Settings.Default.LoginDatabase)) return false;

                if (String.IsNullOrEmpty(Properties.Settings.Default.LoginAccount))
                    sqlConnectString = "Data Source=" + Properties.Settings.Default.LoginServername + ";Initial Catalog=" + Properties.Settings.Default.LoginDatabase + ";Integrated Security=True";
                else
                    sqlConnectString = "Data Source=" + Properties.Settings.Default.LoginServername + ";Initial Catalog=" + Properties.Settings.Default.LoginDatabase + ";User ID = " + Properties.Settings.Default.LoginAccount + ";Password=" + Properties.Settings.Default.LoginPassword;
            }

            sqlConn = new SqlConnection(sqlConnectString);
            try
            {
                sqlConn.Open();
            }
            catch
            {
                return false;
            }
            if (sqlConn.State == ConnectionState.Open) return true;
            return false;
        }

        public DataTable Execute(string sql)
        {
            sqlData = new SqlDataAdapter(sql, sqlConn);
            data = new DataSet();
            sqlData.Fill(data);
            return data.Tables[0];
        }

        public void ExecuteNonQuery(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, sqlConn);
            if (sqlConn.State != ConnectionState.Open) sqlConn.Open();
            cmd.ExecuteNonQuery();
            sqlConn.Close();
        }

        public bool CheckDatabaseExists(string databaseName)
        {
            string sqlCreateDBQuery = string.Format("SELECT database_id FROM sys.databases WHERE Name = '{0}'", databaseName);
            DataTable ds = Execute(sqlCreateDBQuery);
            if (ds.Rows.Count > 0) return true;
            return false;
        }
    }
}
