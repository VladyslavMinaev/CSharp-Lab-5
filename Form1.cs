using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using variant1.Control;

namespace variant1
{
    public partial class Form1 : Form
    {
        //Shell Shell;
        public Form1()
        {
            InitializeComponent();
            SalesShell.GetSalesList();
            Shell.InitializateShell();

            cashCountLbl.Text = $"Каса {Shell.GetCurrentMoneyCount()} UAH";
        }

        private void BuySome()
        {
            try
            {
                Shell.MakeSupplies(int.Parse(textBox1.Text));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void SellSomething()
        {
            try
            {
                SalesShell.MakeSales(int.Parse(textBox2.Text));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                BuySome();
                //cashCountLbl.Text = $"Каса {Shell.GetCurrentMoneyCount()} UAH";
                MessageBox.Show("Переоблік завершено");
            });
            thread.Start();
        }

        //void BuySom
        private void button2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                SellSomething();
                //cashCountLbl.Text = $"Каса {Shell.GetCurrentMoneyCount()} UAH";
                MessageBox.Show("Закупку завершено");
            });
            thread.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = LogsGetter.GetSupplyLogs();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = LogsGetter.GetSalesLogs();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cashCountLbl.Text = $"Каса {Shell.GetCurrentMoneyCount()} UAH";
        }
    }
}