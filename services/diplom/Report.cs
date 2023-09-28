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
using Word = Microsoft.Office.Interop.Word;

//Форма для формирования отчета
namespace diplom
{
    public partial class Report : Form
    {

        DataBase db = new DataBase();
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();
        SqlCommand command = new SqlCommand();
        public Report()
        {
            InitializeComponent();
        }

        //Кнопка для формирования отчета и диаграммы за выбранный пользователем период времени
        private void button1_Click(object sender, EventArgs e)
        {
            //заполнение datagridview
            var date_of_begin = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day).ToString("yyyy-MM-dd");
            var date_of_end = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, dateTimePicker2.Value.Day).ToString("yyyy-MM-dd");
            db.OpenConnection();
            adapter = new SqlDataAdapter($"SELECT Заказ.id as 'Номер заказа', Обслуживаемые_организации.название as Организация, " +
                $"Услуги.название as Услуги, Сотрудники.ФИО, Заказ.дата_заказа as 'Дата Заказа'," +
                $"Заказ.статус as Статус, Заказ.комментарий as Комментарий FROM Заказ" +
                $" LEFT JOIN Обслуживаемые_организации ON Обслуживаемые_организации.id = Заказ.организация_id" +
                $" LEFT JOIN Услуги ON Услуги.id = Заказ.услуга_id" +
                $" LEFT JOIN Сотрудники ON Сотрудники.id = Заказ.сотрудник_id WHERE Заказ.дата_заказа BETWEEN '{date_of_begin}' AND '{date_of_end}' and Заказ.статус='Завершен' ORDER BY Заказ.дата_заказа", db.GetConnection());
            table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;

            //заполнение chart
            adapter = new SqlDataAdapter($"SELECT CONCAT(YEAR(Заказ.дата_заказа), '-', DATENAME(month, Заказ.дата_заказа)) as Дата, " +
                $" COUNT(id) as Количество_заказов FROM Заказ WHERE Заказ.дата_заказа BETWEEN '{date_of_begin}' AND '{date_of_end}' AND Заказ.статус='завершен' " +
                $" GROUP BY CONCAT(YEAR(Заказ.дата_заказа), '-', DATENAME(month, Заказ.дата_заказа)) ORDER BY Дата", db.GetConnection());
            table = new DataTable();
            adapter.Fill(table);
            chart1.DataSource = table;
            db.CloseConnection();

            // тип диаграммы
            chart1.Series[0].ChartType = SeriesChartType.Column;
            chart1.ChartAreas[0].AxisY.Interval = 1;
            // задаем данные для построения диаграммы
            chart1.Series[0].XValueMember = "Дата";
            chart1.Series[0].YValueMembers = "Количество_заказов";


            chart1.Series[0].Color = Color.FromArgb(255, 150, 0);
            chart1.ChartAreas[0].AxisX.Title = "Периоды";
            chart1.ChartAreas[0].AxisY.Title = "Количество заказов";

            // обновляем данные на диаграмме
            chart1.DataBind();



        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void Report_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            chart1.Series[0].IsVisibleInLegend = false;
        }

        //экспорт результата формирования отчёта в word
        private void button2_Click(object sender, EventArgs e)
        {
            var date_of_begin = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day).ToString("dd-MM-yyyy");
            var date_of_end = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, dateTimePicker2.Value.Day).ToString("dd-MM-yyyy");
            try
            {
                int columns = 7;
                int rows = dataGridView1.RowCount;


                Word.Application application = new Word.Application(); 
                Object missing = Type.Missing;
                application.Documents.Add(ref missing, ref missing, ref missing, ref missing);
                Word.Document document = application.ActiveDocument;
                Word.Range range = application.Selection.Range;
                Object behiavor = Word.WdDefaultTableBehavior.wdWord9TableBehavior;
                Object autoFitBehiavor = Word.WdAutoFitBehavior.wdAutoFitFixed;

                Word.Section section = document.Sections[1];
                Word.HeaderFooter header = section.Headers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary];
                header.Range.Text = $"Отчёт о завершенных услугах c {date_of_begin} по {date_of_end}";
                header.Range.Font.Color = Word.WdColor.wdColorBlack;

                //Word.Paragraph paragraph;
                //paragraph = document.Paragraphs.Add();
                //paragraph.Range.Text = "Отчёт о выполенных заказаз за выбранный период времени";

                document.Tables.Add(range, rows, columns, ref behiavor, ref autoFitBehiavor);
                //Заголовок
                document.Tables[1].Cell(1, 1).Range.Text = "Номер заказа";
                document.Tables[1].Cell(1, 2).Range.Text = "Организация-заказчик";
                document.Tables[1].Cell(1, 3).Range.Text = "Заказываемая услуга";
                document.Tables[1].Cell(1, 4).Range.Text = "ФИО исполнителя";
                document.Tables[1].Cell(1, 5).Range.Text = "Дата заказа";
                document.Tables[1].Cell(1, 6).Range.Text = "Статус заказа";
                document.Tables[1].Cell(1, 7).Range.Text = "Комментрий";
                //ориентация страницы
                document.PageSetup.Orientation = Word.WdOrientation.wdOrientLandscape;

                for (int j = 0; j < rows; j++)
                {
                    for (int i = 0; i < columns; i++)
                    {
                        document.Tables[1].Cell(j + 2, i + 1).Range.Text = dataGridView1[i, j].Value.ToString();
                    }
                }
                application.Visible = true;
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                MessageBox.Show("Перед выгрузкой сформируйте отчёт", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
       


        }
    }
}
