using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;

namespace diplom
{
    class DataBase
    {
        SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\db_dp.mdf;Integrated Security=True;Connect Timeout=30");
        public void OpenConnection()
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

        }
        public void CloseConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
        public SqlConnection GetConnection()
        {
            return connection;
        }

    }
}
