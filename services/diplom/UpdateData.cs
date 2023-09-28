using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace diplom
{

    class UpdateData
    {
        DataBase db = new DataBase();
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();
        SqlCommand command = new SqlCommand();
        public void ShowTable(string table_name, DataGridView dataGridView1)
        {
            db.OpenConnection();
            adapter = new SqlDataAdapter($"SELECT * FROM [{table_name}]", db.GetConnection());
            table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            db.CloseConnection();
        }
        public void UpdateComboBox(ComboBox comboBox1,ComboBox comboBox2,string field,string table_name)
        {
            string querry = $"SELECT {field} FROM {table_name}";
            SqlCommand command = new SqlCommand(querry, db.GetConnection());
            db.OpenConnection();           
            SqlDataReader reader = command.ExecuteReader();
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader[$"{field}"].ToString());
                comboBox2.Items.Add(reader[$"{field}"].ToString());
            }
            reader.Close();
            db.OpenConnection();
        }
        public void UpdateComboBoxForEmployee(ComboBox comboBox1, string field, string table_name)
        {
            string querry = $"SELECT {field} FROM {table_name}";
            SqlCommand command = new SqlCommand(querry, db.GetConnection());
            db.OpenConnection();
            SqlDataReader reader = command.ExecuteReader();
            comboBox1.Items.Clear();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader[$"{field}"].ToString());            
            }
            reader.Close();
            db.OpenConnection();
        }
       
    }
}
