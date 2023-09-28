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

//Форма Заказ
namespace diplom
{
    public partial class Order : Form
    {
        DataBase db = new DataBase();
        UpdateData dt = new UpdateData();
        SqlCommand command = null;
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();
        int result, result2, result3;
        public Order()
        {
            InitializeComponent();
            dt.ShowTable("Заказы", dataGridView1);
            dt.UpdateComboBox(comboBox2, comboBox7, "название", "Услуги");
            dt.UpdateComboBox(comboBox3, comboBox8, "ФИО", "Сотрудники");
            dt.UpdateComboBox(comboBox4, comboBox9, "название", "Обслуживаемые_организации");
            dt.UpdateComboBox(comboBox5, comboBox10, "id", "Заказ");
            
        }
        //Очистка checkbox
        public void ClearCheckBoxes()
        {
            foreach (var panel in Controls.OfType<Panel>())
            {
                foreach (var checkbox in panel.Controls.OfType<CheckBox>())
                {
                    checkbox.Checked=false;
                }
            }
        }
        //Очистка полей(textbox)
        public void ClearTextBoxes()
        {
            foreach (var panel in Controls.OfType<Panel>())
            {
                foreach (var textbox in panel.Controls.OfType<TextBox>())
                {
                    textBox3.Clear();
                }
            }
        }

        //добавление данных об заказе в БД
        private void AddButton_Click(object sender, EventArgs e)
        {

            try
            {
                db.OpenConnection();
                if (String.IsNullOrWhiteSpace(textBox1.Text) || comboBox1.SelectedItem==null || comboBox2.SelectedItem == null || comboBox3.SelectedItem == null || comboBox4.SelectedItem == null)
                {
                    MessageBox.Show("Заполните все поля для добавления", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string querry_service = $"SELECT id FROM [Услуги] where название='{comboBox2.Text}'";
                    command = new SqlCommand(querry_service, db.GetConnection());
                    int result = (int)command.ExecuteScalar();
                    string querry_employee = $"SELECT id FROM [Сотрудники] where ФИО='{comboBox3.Text}'";
                    command = new SqlCommand(querry_employee, db.GetConnection());
                    int result2 = (int)command.ExecuteScalar();
                    string querry_organiz = $"SELECT id FROM [Обслуживаемые_организации] where название='{comboBox4.Text}'";
                    command = new SqlCommand(querry_organiz, db.GetConnection());
                    int result3 = (int)command.ExecuteScalar();

                    string AddData = $"INSERT INTO [Заказ] VALUES ('{new DateTime(dateTimePicker1.Value.Year,dateTimePicker1.Value.Month,dateTimePicker1.Value.Day).ToString("yyyy-MM-dd")}','{comboBox1.Text}','{textBox1.Text}','{result}','{result2}','{result3}')";
                    command = new SqlCommand(AddData, db.GetConnection());
                    command.ExecuteNonQuery();
                    dt.ShowTable("Заказы", dataGridView1);
                    dt.UpdateComboBox(comboBox2, comboBox7, "название", "Услуги");
                    dt.UpdateComboBox(comboBox3, comboBox8, "ФИО", "Сотрудники");
                    dt.UpdateComboBox(comboBox4, comboBox9, "название", "Обслуживаемые_организации");
                    dt.UpdateComboBox(comboBox5, comboBox10, "id", "Заказ");
                    dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                    MessageBox.Show("Данные добавлены", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                db.CloseConnection();
            }
            catch (FormatException)
            {
                MessageBox.Show("Неверный формат данных", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            ClearTextBoxes();
            
        }

        //изменение данных об заказе в БД
        private void UpdateButton_Click(object sender, EventArgs e)
        {

            db.OpenConnection();
            try
            {            
                if (comboBox5.SelectedItem==null)
                {
                    MessageBox.Show("Выберите заказ для изменения", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {

                    if (comboBox7.SelectedItem != null)
                    {
                        string querry_service = $"SELECT id FROM [Услуги] where название='{comboBox7.Text}'";
                        command = new SqlCommand(querry_service, db.GetConnection());
                        result = (int)command.ExecuteScalar();
                    }
                    if(comboBox8.SelectedItem != null)
                    {
                        string querry_employee = $"SELECT id FROM [Сотрудники] where ФИО='{comboBox8.Text}'";
                        command = new SqlCommand(querry_employee, db.GetConnection());
                        result2 = (int)command.ExecuteScalar();
                    }
                    if (comboBox9.SelectedItem != null)
                    {
                        string querry_organiz = $"SELECT id FROM [Обслуживаемые_организации] where название='{comboBox9.Text}'";
                        command = new SqlCommand(querry_organiz, db.GetConnection());
                        result3 = (int)command.ExecuteScalar();
                    }
                    db.OpenConnection();
                    //string AddData = $"UPDATE [Заказ] SET дата_заказа='{new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, dateTimePicker2.Value.Day).ToString("yyyy-MM-dd")}', статус='{comboBox6.Text}', комментарий='{textBox2.Text}', услуга_id='{result}', сотрудник_id='{result2}', организация_id='{result3}' where id='{comboBox5.Text}'";
                    string AddData = $"UPDATE [Заказ] SET " +
                    $"статус = CASE WHEN '{comboBox6.Text}' <> '' THEN '{comboBox6.Text}' ELSE статус END, " +
                    $"дата_заказа = CASE WHEN '{new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, dateTimePicker2.Value.Day).ToString("yyyy-MM-dd")}' <> '' THEN '{new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, dateTimePicker2.Value.Day).ToString("yyyy-MM-dd")}' ELSE дата_заказа END, " +
                    $"комментарий = CASE WHEN '{textBox2.Text}' <> '' THEN '{textBox2.Text}' ELSE комментарий END, " +
                    $" услуга_id = CASE WHEN {result} <> '' THEN {result} ELSE услуга_id END, " +
                    $" сотрудник_id = CASE WHEN {result2} <> '' THEN {result2} ELSE сотрудник_id END, " +
                    $" организация_id = CASE WHEN {result3} <> '' THEN {result3} ELSE организация_id END" +
                    $" WHERE id = '{comboBox5.Text}'";
                    command = new SqlCommand(AddData, db.GetConnection());
                    command.ExecuteNonQuery();
                    MessageBox.Show("Данные успешно обновились", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dt.ShowTable("Заказы", dataGridView1);                 
                    db.CloseConnection();
                    dt.UpdateComboBox(comboBox2, comboBox7, "название", "Услуги");
                    dt.UpdateComboBox(comboBox3, comboBox8, "ФИО", "Сотрудники");
                    dt.UpdateComboBox(comboBox4, comboBox9, "название", "Обслуживаемые_организации");
                    dt.UpdateComboBox(comboBox5, comboBox10, "id", "Заказ");
                    dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                }
                db.CloseConnection();
            }
            catch (FormatException)
            {
                MessageBox.Show("Неверный формат данных", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            ClearTextBoxes();
   
        }

        //удаление данных об заказе в БД
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (comboBox10.SelectedItem != null && MessageBox.Show($"Вы действительно хотите удалить заказ c ID: {comboBox10.Text}?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                db.OpenConnection();
                string AddData = $"DELETE FROM [Заказы] where id='{comboBox10.Text}'";
                command = new SqlCommand(AddData, db.GetConnection());
                command.ExecuteNonQuery();
                MessageBox.Show("Данные успешно удалены", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dt.ShowTable("Заказы", dataGridView1);
                db.CloseConnection();
                dt.UpdateComboBox(comboBox2, comboBox7, "название", "Услуги");
                dt.UpdateComboBox(comboBox3, comboBox8, "ФИО", "Сотрудники");
                dt.UpdateComboBox(comboBox4, comboBox9, "название", "Обслуживаемые_организации");
                dt.UpdateComboBox(comboBox5, comboBox10, "id", "Заказ");
                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            else if (comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Выберите заказ для удаления", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        //Загрузка формы
        private void Order_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }


        //Поиск
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            db.OpenConnection();
            string query = $"SELECT CAST(Заказ.id as varchar) as 'Номер заказа', Обслуживаемые_организации.название as 'Организация-заказчик', " +
                $"Услуги.название as Услуги, Сотрудники.ФИО as 'ФИО исполнителя'," +
                $"CAST(Заказ.дата_заказа as varchar) as 'Дата заказа', " +
                $"Заказ.статус as Статус, Заказ.комментарий as Комментарий FROM Заказ" +
                $" LEFT JOIN Обслуживаемые_организации ON Обслуживаемые_организации.id = Заказ.организация_id" +
                $" LEFT JOIN Услуги ON Услуги.id = Заказ.услуга_id " +
                $" LEFT JOIN Сотрудники ON Сотрудники.id = Заказ.сотрудник_id WHERE CAST(Заказ.id as varchar)='{textBox3.Text}'" +
                $" OR Обслуживаемые_организации.название like '%'+'{textBox3.Text}'+'%'" +
                $" OR Услуги.название like '{textBox3.Text}'+'%' OR Сотрудники.ФИО like '{textBox3.Text}'+'%'" +
                $" OR Заказ.статус like'{textBox3.Text}'+'%' OR Заказ.дата_заказа like '{textBox3.Text}'+'%'" +
                $" OR Заказ.комментарий like'{textBox3.Text}'+'%'";

            if (CheckOrg.Checked)
            {
                query = query.Replace($"OR Обслуживаемые_организации.название like '%'+'{textBox3.Text}'", $"AND Обслуживаемые_организации.название not like '%'+'{textBox3.Text}'");
            }
            if (CheckServices.Checked)
            {
                query = query.Replace($"OR Услуги.название like '{textBox3.Text}'", $"AND Услуги.название not like '{textBox3.Text}'");
            }
            if (checkFio.Checked)
            {
                query = query.Replace($"OR Сотрудники.ФИО like '{textBox3.Text}'", $"OR Сотрудники.ФИО not like '{textBox3.Text}'");
            }
            if (checkDate.Checked)
            {
                query = query.Replace($"OR Заказ.дата_заказа like '{textBox3.Text}'", $"AND Заказ.дата_заказа not like '{textBox3.Text}'");
            }
            if (checkStatus.Checked)
            {
                query = query.Replace($"OR Заказ.статус like'{textBox3.Text}'", $"AND Заказ.статус not like '{textBox3.Text}'");
            }
            if (checkComment.Checked)
            {
                query = query.Replace($"OR Заказ.комментарий like'{textBox3.Text}'", $"AND Заказ.комментарий not like '{textBox3.Text}'");
            }
            command = new SqlCommand(query, db.GetConnection());
            adapter = new SqlDataAdapter(command);
            table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            db.CloseConnection();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            panel4.Visible = !panel4.Visible;
        }
        private void comboBox11_SelectedIndexChanged(object sender, EventArgs e)
        {
            var sortColIndex = comboBox11.SelectedIndex;
            dataGridView1.Sort(sortColIndex == 0 ? dataGridView1.Columns[0] : sortColIndex == 1 ? dataGridView1.Columns[0] : sortColIndex == 2 ? dataGridView1.Columns[1] : sortColIndex == 3 ? dataGridView1.Columns[2] : sortColIndex == 4 ? dataGridView1.Columns[3] : sortColIndex == 5 ? dataGridView1.Columns[4] : sortColIndex == 6 ? dataGridView1.Columns[5] : dataGridView1.Columns[6], ListSortDirection.Ascending);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearCheckBoxes();
        }
    }
}

