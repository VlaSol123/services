using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace diplom
{
    public partial class Employee : Form
    {
        DataBase db = new DataBase();
        UpdateData dt = new UpdateData();
        SqlCommand command = null;
        SqlDataAdapter adapter=new SqlDataAdapter();
        DataTable table=new DataTable();
        public Employee()
        {         
            InitializeComponent();           
        }

        //Метод очищающий все TextBox
        public void ClearTextBoxes()
        {
            foreach (var panel in Controls.OfType<Panel>())
            {
                foreach (var textbox in panel.Controls.OfType<TextBox>())
                {
                    textbox.Clear();
                }
            }
        }

        //Загрузка формы и заполнение всех combobox на форме данными
        private void Employee_Load(object sender, EventArgs e)
        {
            dt.ShowTable("Сотрудники", dataGridView1);
            dt.UpdateComboBox(comboBox1, comboBox2, "ФИО", "Сотрудники");
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        //Добавление данных в БД
        private void AddButton_Click(object sender, EventArgs e)
        {
            string FIO_of_employee = $"SELECT ФИО FROM Сотрудники where ФИО='{textBox3.Text}'";           
            adapter = new SqlDataAdapter(FIO_of_employee,db.GetConnection());
            table = new DataTable();
            adapter.Fill(table);
            try
            {
                db.OpenConnection();
                if (String.IsNullOrWhiteSpace(textBox1.Text) || String.IsNullOrWhiteSpace(textBox2.Text) || String.IsNullOrWhiteSpace(textBox3.Text) || String.IsNullOrWhiteSpace(textBox4.Text))
                {
                    MessageBox.Show("Заполните все поля для добавления", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if(table.Rows.Count > 0)
                {
                    MessageBox.Show("Вы пытаетесь добавить существующего пользователя", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string AddData = $"INSERT INTO [Сотрудники] VALUES ('{textBox1.Text}', '{textBox2.Text}','{textBox3.Text}','{textBox4.Text}')";
                    command = new SqlCommand(AddData, db.GetConnection());
                    command.ExecuteNonQuery();
                    dt.ShowTable("Сотрудники", dataGridView1);
                    dt.UpdateComboBox(comboBox1, comboBox2, "ФИО", "Сотрудники");               
                    MessageBox.Show("Данные добавлены", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);                   
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Неверный формат данных", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException)
            {
                MessageBox.Show("Введенные вами значения превышают допустимый лимит", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            ClearTextBoxes();
        }

        //Изменение данных 
        private void UpdateButton_Click(object sender, EventArgs e)
        {
            
            try
            {
                db.OpenConnection();
                if (comboBox1.SelectedItem == null)
                {
                    MessageBox.Show("Выберите сотрудника для изменения", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string AddData = $"UPDATE [Сотрудники] SET " +
                    $"логин = CASE WHEN '{textBox6.Text}' <> '' THEN '{textBox6.Text}' ELSE логин END, " +
                    $"пароль = CASE WHEN '{textBox7.Text}' <> '' THEN '{textBox7.Text}' ELSE пароль END, " +
                    $"ФИО = CASE WHEN '{textBox8.Text}' <> '' THEN '{textBox8.Text}' ELSE ФИО END, " +
                    $"телефон = CASE WHEN '{textBox9.Text}' <> '' THEN '{textBox9.Text}' ELSE телефон END " +
                    $"WHERE ФИО = '{comboBox1.Text}'";                    
                    command = new SqlCommand(AddData, db.GetConnection());
                    command.ExecuteNonQuery();
                    MessageBox.Show("Данные успешно обновились", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dt.ShowTable("Сотрудники", dataGridView1);
                    dt.UpdateComboBox(comboBox1, comboBox2, "ФИО", "Сотрудники");
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Неверный формат данных", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException)
            {
                MessageBox.Show("Введенные вами значения превышают допустимый лимит", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            ClearTextBoxes();
        }

        //Удаление данных
        private void DeleteButton_Click(object sender, EventArgs e)
        {

           
            if (comboBox2.SelectedItem != null && MessageBox.Show($"Вы действительно хотите удалить пользователя {comboBox2.Text}?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                db.OpenConnection();
                string AddData = $"DELETE FROM [Сотрудники] where ФИО='{comboBox2.SelectedItem}'";               
                command = new SqlCommand(AddData, db.GetConnection());
                command.ExecuteNonQuery();
                MessageBox.Show("Данные успешно удалены", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dt.ShowTable("Сотрудники", dataGridView1);
                db.CloseConnection();
                dt.UpdateComboBox(comboBox1, comboBox2, "ФИО", "Сотрудники");
                
            }
            else if (comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Выберите фамилию для удаления", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            
        }

    }
}
