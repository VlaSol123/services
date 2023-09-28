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
using System.Windows.Forms.DataVisualization.Charting;

namespace diplom
{
    public partial class Statistic : Form
    {

        DataBase db = new DataBase();
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();
        SqlCommand command = new SqlCommand();
        public Statistic()
        {
            InitializeComponent();
        }

       
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            MainWindow mainMenu = new MainWindow();
            mainMenu.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var date_of_begin = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day).ToString("yyyy-MM-dd");
            var date_of_end = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, dateTimePicker2.Value.Day).ToString("yyyy-MM-dd");
            db.OpenConnection();
            adapter = new SqlDataAdapter($"SELECT Услуги.название as Услуга,Услуги.стоимость,Услуги.описание, COUNT(*) " +
            $"as 'Количество заказов' FROM Заказ LEFT JOIN Услуги ON Услуги.id = Заказ.услуга_id WHERE Заказ.дата_заказа BETWEEN '{date_of_begin}' and '{date_of_end}'" +
            $" GROUP BY Услуги.название,Услуги.стоимость,Услуги.описание  ORDER BY COUNT(*) DESC", db.GetConnection());   
            table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            db.CloseConnection();
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            chart1.Titles.Clear();
            chart1.Series.Clear();
          

            if (comboBox1.SelectedItem == "Круговая")
            {
                //Получаем область диаграммы
                ChartArea chartArea = chart1.ChartAreas[0];
                // Устанавливаем начальные координаты диаграммы
                chartArea.Position.X = 1;
                chartArea.Position.Y = 5;
                // Устанавливаем ширину и высоту диаграммы
                chartArea.Position.Width = 50;
                chartArea.Position.Height = 70;
                // Обновляем отображение графика
                chart1.Update();
                chart1.Titles.Add("Круговая диаграмма о самых заказываемых услугах");
                chart1.Titles[0].Font = new Font("Arial", 14, FontStyle.Bold);               
                chart1.Series.Clear();
                adapter = new SqlDataAdapter($"SELECT Услуги.название as Услуга, COUNT(*)" +
                $"as 'Количество заказов' FROM Заказ LEFT JOIN Услуги ON Услуги.id = Заказ.услуга_id WHERE Заказ.дата_заказа BETWEEN '{date_of_begin}' and '{date_of_end}'" +
                $" GROUP BY Услуги.название ORDER BY COUNT(*) DESC", db.GetConnection());
                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                table = new DataTable();
                adapter.Fill(table);            
                Series series = new Series();
                series.ChartType = SeriesChartType.Pie;
                series.IsValueShownAsLabel = true;               
                foreach (DataRow row in table.Rows)
                {
                    series.Points.AddXY(row["Услуга"], row["Количество заказов"]);
                }
                chart1.Series.Add(series);
                //chart1.ResetAutoValues();
            }
            if (comboBox1.SelectedItem == "Гистограмма")
            {
                
                ChartArea chartArea = chart1.ChartAreas[0];
                // Устанавливаем X-координату области на 0
                chartArea.Position.X = 2;
                chartArea.Position.Y = 15;
                // Устанавливаем ширину области на половину ширины chart1
                chartArea.Position.Width = 94;
                chartArea.Position.Height = 90;
                // Обновляем отображение графика
                chart1.Update();
                //название
                chart1.Titles.Add("Гистограмма о самых заказываемых услугах");
                chart1.Titles[0].Font = new Font("Arial", 14, FontStyle.Bold);
                chart1.Series.Clear();
                adapter = new SqlDataAdapter($"SELECT Услуги.название as Услуга, COUNT(*)" +
                $"as 'Количество заказов' FROM Заказ LEFT JOIN Услуги ON Услуги.id = Заказ.услуга_id WHERE Заказ.дата_заказа BETWEEN '{date_of_begin}' and '{date_of_end}'" +
                $" GROUP BY Услуги.название ORDER BY COUNT(*) DESC", db.GetConnection());
                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                table = new DataTable();
                adapter.Fill(table);
                //создаём серию
                Series series = new Series();
                series.IsVisibleInLegend = false;
                //настраиваем текст у диаграммы
                chart1.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Trebushet MS", 10, FontStyle.Regular);
                chart1.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                chart1.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Arial", 9, FontStyle.Regular);
                series.ChartType = SeriesChartType.Column;
                series.IsValueShownAsLabel = false;               
                foreach (DataRow row in table.Rows)
                {
                    series.Points.AddXY(row["Услуга"], row["Количество заказов"]);
                }
                chart1.Series.Add(series);


            }
            if (comboBox1.SelectedItem == "Пирамида")
            {
                //Получаем первую область диаграммы
                ChartArea chartArea = chart1.ChartAreas[0];
                // Устанавливаем X-координату области на 0
                chartArea.Position.X = 2;
                chartArea.Position.Y = 15;
                // Устанавливаем ширину области на половину ширины chart1
                chartArea.Position.Width = 50;
                chartArea.Position.Height = 70;
                // Обновляем отображение графика
                chart1.Update();
                chart1.Titles.Add("Диаграмма-пирамида о самых заказываемых услугах");
                chart1.Titles[0].Font = new Font("Arial", 14, FontStyle.Bold);
                chart1.Series.Clear();
                adapter = new SqlDataAdapter($"SELECT Услуги.название as Услуга, COUNT(*)" +
                $"as 'Количество заказов' FROM Заказ LEFT JOIN Услуги ON Услуги.id = Заказ.услуга_id WHERE Заказ.дата_заказа BETWEEN '{date_of_begin}' and '{date_of_end}'" +
                $" GROUP BY Услуги.название ORDER BY COUNT(*) DESC", db.GetConnection());
                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                table = new DataTable();
                adapter.Fill(table);
                Series series = new Series();
                series.ChartType = SeriesChartType.Pyramid;
                series.IsValueShownAsLabel = true;
                foreach (DataRow row in table.Rows)
                {
                    series.Points.AddXY(row["Услуга"], row["Количество заказов"]);
                }
                chart1.Series.Add(series);
                //chart1.ResetAutoValues();
            }
        }

        private void Statistic_Load(object sender, EventArgs e)
        {
            chart1.Series[0].IsVisibleInLegend = false;
         
        }
    }
}
