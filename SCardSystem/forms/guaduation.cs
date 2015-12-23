using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCardSystem
{
    public partial class guaduation : Form
    {
        private MySql.Data.MySqlClient.MySqlDataReader msReader;

        public guaduation()
        {
            InitializeComponent();
        }

        public guaduation(MySql.Data.MySqlClient.MySqlDataReader msReader)
        {
            // TODO: Complete member initialization
            this.msReader = msReader;
            InitializeComponent();
            msReader.Read();
            Sno.Text = msReader[0].ToString();
            Sid.Text = msReader[1].ToString();
            Sname.Text = msReader[2].ToString();
            Ssex.Text = msReader[3].ToString();
            Sbirth.Text = msReader[4].ToString();
            Sdept.Text = msReader[5].ToString();
            Sspecial.Text = msReader[6].ToString();
            Sclass.Text = msReader[7].ToString();
            Saddr.Text = msReader[8].ToString();
            Cardno.Text = msReader[9].ToString();
        }

        private void go_Click(object sender, EventArgs e)
        {
            var mysqlEdit = "UPDATE `shoppingcard`.`stu_info` SET `Sstate` = 0'";
            var dbConnect = new DbConnect();
            DbConnect.getmysqlcom(mysqlEdit);
            this.Hide();
        }
    }
}
