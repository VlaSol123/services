using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//Форма авторизации пользователя
namespace diplom
{
    public partial class Authorization : Form
    {
        DataBase db = new DataBase();
        SqlCommand command = null;
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();
        SqlDataReader reader = null;
        public Authorization()
        {
            InitializeComponent();
        }

        
        //Кнопка входа в приложение
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(textBox_name.Text) || String.IsNullOrWhiteSpace(textBox_pass.Text))
                {
                    MessageBox.Show("Заполните поля", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Users.loginUser = textBox_name.Text;
                    Users.passUser = textBox_pass.Text;
                    db.OpenConnection();
                    string PassAndLog = $"Select ФИО,id,логин,пароль from [Сотрудники] where логин='{Users.loginUser}' and пароль='{Users.passUser}'";
                    command = new SqlCommand(PassAndLog, db.GetConnection());
                    adapter.SelectCommand = command;
                    adapter.Fill(table);
                    //записываем результат выборки(ФИО) в переменную 
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Users.FIO = reader.GetString(0);
                        Users.idUser = reader.GetInt32(1);
                    }
                    reader.Close();
                    db.CloseConnection();
                    if (table.Rows.Count == 0 && Users.loginUser == "Admin" && Users.passUser == "12345")
                    {
                        this.Hide();
                        MessageBox.Show("Добро пожаловать в систему администрирования", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                    }
                    else if (table.Rows.Count == 1)
                    {
                        this.Hide();
                        MessageBox.Show($"Добро пожаловать, {Users.FIO}", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        EmployeeWindow employee = new EmployeeWindow();
                        employee.Show();
                    }
                    else
                    {
                        MessageBox.Show($"Введенные данные отсутствуют", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Отсутствует подключение к БД:\nНеверная строка подключения", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
           
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox_pass.PasswordChar = '*';
                checkBox1.Text = "Показать пароль";
            }
            else
            {
                textBox_pass.PasswordChar = '\0';
                checkBox1.Text = "Скрыть пароль";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
