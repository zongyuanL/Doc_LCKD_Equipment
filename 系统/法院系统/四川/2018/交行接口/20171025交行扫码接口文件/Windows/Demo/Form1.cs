using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CardDownData data = Trans.Login();
            setTextBox2(button1.Text, data);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int amount = int.Parse(textBox3.Text);
            String channel = this.comboBox1.Text.Substring(0, 2);
            String authCode = this.textBox1.Text;
            String detail = this.textBox4.Text;
            CardDownData data = Trans.Purchase(amount, channel, detail, authCode);
            setTextBox2(button2.Text, data);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int amount = int.Parse(textBox3.Text);
            String channel = this.comboBox1.Text.Substring(0, 2);
            String authCode = this.textBox1.Text;
            String detail = this.textBox4.Text;
            String deviceID = this.textBox5.Text;
            CardDownData data = Trans.AutoPurchase(amount, channel, detail, 45, authCode, deviceID);
            setTextBox2(button3.Text, data);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int amount;
            try
            {
                amount = int.Parse(textBox3.Text);
            }
            catch
            {
                amount = -1;
            }
            String orderNo = this.textBox1.Text;
            CardDownData data = Trans.Void(amount, orderNo);
            setTextBox2(button5.Text, data);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int amount = int.Parse(textBox3.Text);
            String orderNo = this.textBox1.Text;
            CardDownData data = Trans.Refund(amount, orderNo);
            setTextBox2(button6.Text, data);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            String orderNo = this.textBox1.Text;
            CardDownData data = Trans.Query(orderNo);
            setTextBox2(button7.Text, data);
        }

        string rn = "\r\n";
        private void setTextBox2(string title, CardDownData data)
        {
            textBox2.Clear();
            textBox2.Text = title + rn;
            textBox2.Text += Encoding.ASCII.GetString(data.aucRespCode) + rn;
            textBox2.Text += Encoding.GetEncoding("GBK").GetString(data.aucRespInfo);
            textBox2.Text += rn;
            textBox2.Text += Encoding.ASCII.GetString(data.aucAmount) + rn;
            textBox2.Text += Encoding.ASCII.GetString(data.aucTransDate) + rn;
            textBox2.Text += Encoding.ASCII.GetString(data.aucTransTime) + rn;
            textBox2.Text += Encoding.ASCII.GetString(data.aucOrderNo);
            textBox2.Text += rn;
            textBox2.Text += Encoding.GetEncoding("GBK").GetString(data.aucReserve);
            textBox2.Text += rn;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            String deviceID = this.textBox5.Text;
            Trans.SetParam(0, deviceID);
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
