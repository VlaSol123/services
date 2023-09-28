using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//Главная форма админ-панели
namespace diplom
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.BackColor = Color.Transparent;
            linkLabel1.BackColor = Color.Transparent;
            linkLabel2.BackColor = Color.Transparent;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Employee employee = new Employee();
            employee.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Organizations organizations = new Organizations();
            organizations.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Services service = new Services();
            service.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Order order = new Order();
            order.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Authorization authorization = new Authorization();
            authorization.Show();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Report report = new Report();
            report.Show();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Statistic statistic = new Statistic();
            statistic.Show();
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
