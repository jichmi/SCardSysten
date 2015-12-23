using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SCardSystem
{
    public partial class edit : Form
    {
        private String oldName;
        private MySqlDataReader msReader = admin.msReader;
        public edit()
        {
            InitializeComponent();

         }

        /**
         *修改信息
         *未完成
         */
        private void go_Click(object sender, EventArgs e)
        {
            var mysqlEdit = "UPDATE `shoppingcard`.`stu_info` SET `Sno` = '"+Sno.Text+"', `Sid` = '"+Sid.Text+"', `Sname` = '"+Sname.Text+"', `Ssex` = '"+Ssex.Text+"', `Sbirth` = '"+Sbirth.Text+"', `Sdept` = '"+Sdept.Text+"', `Sspecial` = '"+Sspecial.Text+"', `Sclass` = '"+Sclass.Text+"', `Saddr` = '"+Saddr.Text+"', `Cardno` = '"+Cardno.Text+"', `Spwd` = '"+lib.getMD5(Spwd.Text.Trim())+"' WHERE `stu_info`.`Sno` = '"+oldName+"'";
            var dbConnect=new DbConnect();
            DbConnect.getmysqlcom(mysqlEdit);
            this.Hide();
        }

        private void edit_Load(object sender, EventArgs e)
        {
            msReader.Read();
            Sno.Text = msReader[0].ToString();
            Sid.Text = msReader[1].ToString();
            oldName = msReader[2].ToString();
            Sname.Text = oldName;
            Ssex.Text = msReader[3].ToString();
            Sbirth.Text = msReader[4].ToString();
            Sdept.Text = msReader[5].ToString();
            Sspecial.Text = msReader[6].ToString();
            Sclass.Text = msReader[7].ToString();
            Saddr.Text = msReader[8].ToString();
            Cardno.Text = msReader[9].ToString();
        }
    }
}
