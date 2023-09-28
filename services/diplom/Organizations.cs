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
    public partial class Organizations : Form
    {
        DataBase db = new DataBase();
        UpdateData dt = new UpdateData();
        SqlCommand command = null;
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();
        public Organizations()
        {
            InitializeComponent();         
        }
        
        //Очистка всех полей(textboxes)
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
            string name_of_org = $"SELECT название FROM Обслуживаемые_организации where название='{textBox1.Text}'";
            adapter = new SqlDataAdapter(name_of_org, db.GetConnection());
            table = new DataTable();
            adapter.Fill(table);
            try
            {
                db.OpenConnection();
                if (String.IsNullOrWhiteSpace(textBox1.Text) || String.IsNullOrWhiteSpace(textBox2.Text) || String.IsNullOrWhiteSpace(textBox3.Text) || String.IsNullOrWhiteSpace(textBox4.Text))
                {
                    MessageBox.Show("Заполните все поля для добавления", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (table.Rows.Count > 0)
                {
                    MessageBox.Show("Вы пытаетесь добавить существующую организацию", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string AddData = $"INSERT INTO [Обслуживаемые_организации] VALUES ('{textBox1.Text}', '{textBox2.Text}','{textBox3.Text}','{textBox4.Text}')";
                    command = new SqlCommand(AddData, db.GetConnection());
                    command.ExecuteNonQuery();
                    dt.ShowTable("Обслуживаемые_организации", dataGridView1);
                    MessageBox.Show("Данные добавлены", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dt.UpdateComboBox(comboBox1, comboBox2, "название", "Обслуживаемые_организации");
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Неверный формат данных", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            ClearTextBoxes();
        }

        //Кнопка изменения данных
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
                    string AddData = $"UPDATE [Обслуживаемые_организации] SET " +
                    $"название = CASE WHEN '{textBox5.Text}' <> '' THEN '{textBox5.Text}' ELSE название END, " +
                    $"адрес = CASE WHEN '{textBox6.Text}' <> '' THEN '{textBox6.Text}' ELSE адрес END, " +
                    $"контактное_лицо = CASE WHEN '{textBox7.Text}' <> '' THEN '{textBox7.Text}' ELSE контактное_лицо END, " +
                    $"телефон = CASE WHEN '{textBox8.Text}' <> '' THEN '{textBox8.Text}' ELSE телефон END " +
                    $"WHERE название = '{comboBox1.Text}'";
                    //string AddData = $"UPDATE [Обслуживаемые_организации] SET название='{textBox5.Text}', адрес='{textBox6.Text}', контактное_лицо='{textBox7.Text}', телефон='{textBox8.Text}' where название='{comboBox1.Text}'";
                    command = new SqlCommand(AddData, db.GetConnection());
                    command.ExecuteNonQuery();
                    MessageBox.Show("Данные успешно обновились", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dt.ShowTable("Обслуживаемые_организации", dataGridView1);
                    dt.UpdateComboBox(comboBox1, comboBox2, "название", "Обслуживаемые_организации");
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Неверный формат данных", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException)
            {
                MessageBox.Show("Неверный формат данных", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            ClearTextBoxes();
        }

        //Кнопка удаления данных
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null && MessageBox.Show($"Вы действительно хотите удалить организацию {comboBox2.Text}?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                db.OpenConnection();
                string AddData = $"DELETE FROM [Обслуживаемые_организации] where название='{comboBox2.Text}'";
                command = new SqlCommand(AddData, db.GetConnection());
                command.ExecuteNonQuery();
                MessageBox.Show("Данные успешно удалены", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dt.ShowTable("Обслуживаемые_организации", dataGridView1);
                db.CloseConnection();
                dt.UpdateComboBox(comboBox1, comboBox2, "название", "Обслуживаемые_организации");
            }
            else if (comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Выберите организацию для удаления", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        //Загрузка формы и заполения всех combobox значениями
        private void Organizations_Load(object sender, EventArgs e)
        {
            dt.ShowTable("Обслуживаемые_организации", dataGridView1);
            dt.UpdateComboBox(comboBox1, comboBox2, "название", "Обслуживаемые_организации");
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
