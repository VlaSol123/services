using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//Главная страница для сотрудника
namespace diplom
{
    public partial class EmployeeWindow : Form
    {
        DataBase db = new DataBase();
        SqlCommand command = null;
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();
        SqlDataReader reader = null;
        UpdateData dt = new UpdateData();
        public EmployeeWindow()
        {
            InitializeComponent();

        }

        //Загрузка формы
        private void EmployeeWindow_Load(object sender, EventArgs e)
        {
            label1.Text = $"Добро пожаловать, {Users.FIO}";
            string querry = $"SELECT id FROM Заказ WHERE сотрудник_id='{Users.idUser}'";
            SqlCommand command = new SqlCommand(querry, db.GetConnection());
            db.OpenConnection();
            SqlDataReader reader = command.ExecuteReader();
            comboBox1.Items.Clear();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader[0].ToString());

            }
            reader.Close();     
            label3.Text = "Все заказы";
            adapter = new SqlDataAdapter($"SELECT Заказ.id as Номер, Заказ.дата_заказа, Заказ.статус, Заказ.комментарий, Обслуживаемые_организации.название, Обслуживаемые_организации.контактное_лицо, Услуги.стоимость, Сотрудники.ФИО" +
            $" FROM Заказ INNER JOIN Услуги ON Заказ.услуга_id = Услуги.id " +
            $"INNER JOIN Обслуживаемые_организации ON Заказ.организация_id = Обслуживаемые_организации.id " +
            $"INNER JOIN Сотрудники ON Заказ.сотрудник_id = Сотрудники.id WHERE Сотрудники.id = '{Users.idUser}'; ", db.GetConnection());
            table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            db.CloseConnection();


        }

        //Обновление статуса заказа
        private void button1_Click(object sender, EventArgs e)
        {
            db.OpenConnection();
            string UpdateState = $"UPDATE Заказ SET статус='{comboBox2.Text}' where id={comboBox1.Text}";
            command = new SqlCommand(UpdateState, db.GetConnection());
            command.ExecuteNonQuery();
            adapter = new SqlDataAdapter($"SELECT Заказ.id as Номер, Заказ.дата_заказа, Заказ.статус, Заказ.комментарий, Обслуживаемые_организации.название, Обслуживаемые_организации.контактное_лицо, Услуги.стоимость, Сотрудники.ФИО" +
            $" FROM Заказ INNER JOIN Услуги ON Заказ.услуга_id = Услуги.id " +
            $"INNER JOIN Обслуживаемые_организации ON Заказ.организация_id = Обслуживаемые_организации.id " +
            $"INNER JOIN Сотрудники ON Заказ.сотрудник_id = Сотрудники.id WHERE Сотрудники.id = '{Users.idUser}'; ", db.GetConnection());
            table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            db.CloseConnection();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            label3.Text = "Все заказы";
            label3.ForeColor = Color.Black;
            db.OpenConnection();
            adapter = new SqlDataAdapter($"SELECT Заказ.id as Номер, Заказ.дата_заказа, Заказ.статус, Заказ.комментарий, Обслуживаемые_организации.название, Обслуживаемые_организации.контактное_лицо, Услуги.стоимость, Сотрудники.ФИО" +
            $" FROM Заказ INNER JOIN Услуги ON Заказ.услуга_id = Услуги.id " +
            $"INNER JOIN Обслуживаемые_организации ON Заказ.организация_id = Обслуживаемые_организации.id " +
            $"INNER JOIN Сотрудники ON Заказ.сотрудник_id = Сотрудники.id WHERE Сотрудники.id = '{Users.idUser}'; ", db.GetConnection());
            table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            db.CloseConnection();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            label3.Text = "Новые заказы";
            label3.ForeColor = Color.Blue;
            db.OpenConnection();
            adapter = new SqlDataAdapter($"SELECT Заказ.id as Номер, Заказ.дата_заказа, Заказ.статус, Заказ.комментарий, Обслуживаемые_организации.название, Обслуживаемые_организации.контактное_лицо, Услуги.стоимость, Сотрудники.ФИО" +
            $" FROM Заказ INNER JOIN Услуги ON Заказ.услуга_id = Услуги.id " +
            $"INNER JOIN Обслуживаемые_организации ON Заказ.организация_id = Обслуживаемые_организации.id " +
            $"INNER JOIN Сотрудники ON Заказ.сотрудник_id = Сотрудники.id WHERE Заказ.статус='В обработке' and Сотрудники.id = '{Users.idUser}'", db.GetConnection());
            table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            db.CloseConnection();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            label3.Text = "В процессе выполнения";
            label3.ForeColor = Color.OrangeRed;
            db.OpenConnection();
            adapter = new SqlDataAdapter($"SELECT Заказ.id as Номер, Заказ.дата_заказа, Заказ.статус, Заказ.комментарий, Обслуживаемые_организации.название, Обслуживаемые_организации.контактное_лицо, Услуги.стоимость, Сотрудники.ФИО" +
            $" FROM Заказ INNER JOIN Услуги ON Заказ.услуга_id = Услуги.id " +
            $"INNER JOIN Обслуживаемые_организации ON Заказ.организация_id = Обслуживаемые_организации.id " +
            $"INNER JOIN Сотрудники ON Заказ.сотрудник_id = Сотрудники.id WHERE Сотрудники.id = '{Users.idUser}' and Заказ.статус='В процессе'", db.GetConnection());
            table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            db.CloseConnection();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            label3.Text = "Выполненные заказы";
            label3.ForeColor = Color.Green;
            db.OpenConnection();
            adapter = new SqlDataAdapter($"SELECT Заказ.id as Номер, Заказ.дата_заказа, Заказ.статус, Заказ.комментарий, Обслуживаемые_организации.название, Обслуживаемые_организации.контактное_лицо, Услуги.стоимость, Сотрудники.ФИО" +
            $" FROM Заказ INNER JOIN Услуги ON Заказ.услуга_id = Услуги.id " +
            $"INNER JOIN Обслуживаемые_организации ON Заказ.организация_id = Обслуживаемые_организации.id " +
            $"INNER JOIN Сотрудники ON Заказ.сотрудник_id = Сотрудники.id WHERE Сотрудники.id = '{Users.idUser}' and Заказ.статус='Завершен'", db.GetConnection());
            table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            db.CloseConnection();
        }
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            label3.Text = "Заказы на удержании";
            label3.ForeColor = Color.Orange;
            db.OpenConnection();
            adapter = new SqlDataAdapter($"SELECT Заказ.id as Номер, Заказ.дата_заказа, Заказ.статус, Заказ.комментарий, Обслуживаемые_организации.название, Обслуживаемые_организации.контактное_лицо, Услуги.стоимость, Сотрудники.ФИО" +
            $" FROM Заказ INNER JOIN Услуги ON Заказ.услуга_id = Услуги.id " +
            $"INNER JOIN Обслуживаемые_организации ON Заказ.организация_id = Обслуживаемые_организации.id " +
            $"INNER JOIN Сотрудники ON Заказ.сотрудник_id = Сотрудники.id WHERE Сотрудники.id = '{Users.idUser}' and Заказ.статус='На удержании'", db.GetConnection());
            table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            db.CloseConnection();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Authorization authorization = new Authorization();
            authorization.Show();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(@"E:\diplom\diplom\spravka_diplom.chm");
            }
            catch (System.Exception)
            {
                MessageBox.Show("Невозможно открыть справку или указан неверный путь", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            
            
        }
    }
}
