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
    public partial class Services : Form
    {
        DataBase db = new DataBase();
        UpdateData dt = new UpdateData();
        SqlCommand command = null;
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();
        public Services()
        {
            InitializeComponent();
            dt.UpdateComboBox(comboBox1, comboBox2, "название", "Услуги");
        }

        //Очистака всех полей(textboxes)
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

        //Кнопка добавления данных в БД
        private void AddButton_Click(object sender, EventArgs e)
        {
            string name_of_services = $"SELECT название FROM Услуги where название='{textBox1.Text}'";
            adapter = new SqlDataAdapter(name_of_services, db.GetConnection());
            table = new DataTable();
            adapter.Fill(table);
            try
            {
                db.OpenConnection();
                if (String.IsNullOrWhiteSpace(textBox1.Text) || String.IsNullOrWhiteSpace(textBox2.Text) || String.IsNullOrWhiteSpace(textBox3.Text))
                {
                    MessageBox.Show("Заполните все поля для добавления", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (table.Rows.Count > 0)
                {
                    MessageBox.Show("Вы пытаетесь добавить существующую услугу", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string AddData = $"INSERT INTO [Услуги] VALUES ('{textBox1.Text}', '{textBox2.Text}','{textBox3.Text}')";
                    command = new SqlCommand(AddData, db.GetConnection());
                    command.ExecuteNonQuery();
                    dt.ShowTable("Услуги", dataGridView1);
                    dt.UpdateComboBox(comboBox1, comboBox2, "название", "Услуги");
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


        //Кнопка изменения данных в БД
        private void UpdateButton_Click(object sender, EventArgs e)
        {
            try
            {
                db.OpenConnection();
                if (comboBox1.SelectedItem == null)
                {
                    MessageBox.Show("Заполните все поля для изменения", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string AddData = $"UPDATE [Услуги] SET " +
                    $"название = CASE WHEN '{textBox4.Text}' <> '' THEN '{textBox4.Text}' ELSE название END, " +
                    $"описание = CASE WHEN '{textBox5.Text}' <> '' THEN '{textBox5.Text}' ELSE описание END, " +
                    $"стоимость = CASE WHEN '{textBox6.Text}' <> '' THEN '{textBox6.Text}' ELSE стоимость END " +
                    $"WHERE название = '{comboBox1.Text}'";
                    //string AddData = $"UPDATE [Услуги] SET название='{textBox4.Text}', описание='{textBox5.Text}', стоимость='{textBox6.Text}'  where название='{comboBox1.Text}'";
                    command = new SqlCommand(AddData, db.GetConnection());
                    command.ExecuteNonQuery();
                    MessageBox.Show("Данные успешно обновились", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dt.ShowTable("Услуги", dataGridView1);
                    dt.UpdateComboBox(comboBox1, comboBox2, "название", "Услуги");
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

        //Кнопка удаления данных из БД
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null && MessageBox.Show($"Вы действительно хотите удалить услугу {comboBox2.Text}?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                db.OpenConnection();
                string AddData = $"DELETE FROM [Услуги] where название='{comboBox2.Text}'";
                command = new SqlCommand(AddData, db.GetConnection());
                command.ExecuteNonQuery();
                MessageBox.Show("Данные успешно удалены", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dt.ShowTable("Услуги", dataGridView1);
                dt.UpdateComboBox(comboBox1, comboBox2, "название", "Услуги");
                
            }
            else if (comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Выберите услугу для удаления", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        //Загрузка формы и заполнение combobox
        private void Services_Load(object sender, EventArgs e)
        {
            dt.ShowTable("Услуги", dataGridView1);
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dataGridView1.Columns[0].Visible = false;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
