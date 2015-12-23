using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SCardSystem.libs;

namespace SCardSystem
{
    public partial class shop : Form
    {

        public shop()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            float money = float.Parse(this.money.Text.Trim());
            float pay = float.Parse(this.textBox1.Text.Trim());
            money -= pay;
            Connect.writeData(money.ToString(),3);
            this.money.Text = money.ToString();
        }

        private void shop_Load(object sender, EventArgs e)
        {
            String read = Connect.readData(3);
            float money = float.Parse(read);
            this.money.Text = money.ToString();
        }

    }
}
