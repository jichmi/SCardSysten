using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using SCardSystem.libs;

namespace SCardSystem
{
    public partial class admin : Form
    {
        public static  MySqlDataReader msReader;

        public admin(string userName)
        {
            InitializeComponent();
            DbConnect dbConnect=new DbConnect();
            MySqlDataReader userInfo = DbConnect.getmysqlread("SELECT `Aname` FROM `admin_info` WHERE `Ano` = " + userName);
            userInfo.Read();
            adminName.Text = "welcome " + userInfo[0]; 
            Connect.connectCom();
            Connect.loadKey("");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            String cardNo = Cardno.Text.Replace(" ", "");
            String mysqlCom = "INSERT INTO `stu_info`(`Sno`, `Sid`, `Sname`, `Ssex`, `Sbirth`, `Sdept`, `Sspecial`, `Sclass`, `Saddr`, `Cardno`, `Spwd`) VALUES ('"+Sno.Text.Trim()+"','"+Sid.Text.Trim()+"','"+Sname.Text.Trim()+"','"+Ssex.Text.Trim()+"','"+Sbirth.Text.Trim()+"','"+Sdept.Text.Trim()+"','"+Sspecial.Text.Trim()+"','"+Sclass.Text.Trim()+"','"+Saddr.Text.Trim()+"','"+cardNo+"','"+lib.getMD5(Spwd.Text.Trim())+"') ";
            DbConnect.getmysqlcom(mysqlCom);
            String mysqlCom1 = "INSERT INTO `shoppingcard`.`card_info` (`Cno`, `Cstate`, `Cmoney`, `Cstyle`, `Ctime`) VALUES ('" + cardNo + "', '1', '0','1', '" + DateTime.Now.ToString().Replace("/","-") + "');";
            DbConnect.getmysqlcom(mysqlCom1);
        }

        private String oldNo;
        private void button2_Click(object sender, EventArgs e)
        {
            String mysqlRead = "SELECT * FROM `stu_info` WHERE  `Sno` = '" + Stuno.Text + "' AND `Sname` = '"+Stuname.Text + "' ORDER BY `Sno` ASC ";
            DbConnect dbConnect=new DbConnect();
            msReader=DbConnect.getmysqlread(mysqlRead);
            if (msReader.HasRows)
            {
            panel1.Visible = false;
            panel2.Visible = true;
            msReader.Read();
            nSno.Text = msReader[0].ToString();
            oldNo = nSno.Text.Trim();
            nSid.Text = msReader[1].ToString();
            nSname.Text = msReader[2].ToString();
            nSsex.Text = msReader[3].ToString();
            nSbirth.Text = msReader[4].ToString();
            nSdept.Text = msReader[5].ToString();
            nSspecial.Text = msReader[6].ToString();
            nSclass.Text = msReader[7].ToString();
            nSaddr.Text = msReader[8].ToString();
            nCardno.Text = msReader[9].ToString();
                Stuname.Text = "";
                Stuno.Text = "";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String mysqlRead = "SELECT * FROM `stu_info` WHERE `Sno` = '"+Stuno+"' AND `Sname` = '"+Stuname+"' ";
            DbConnect dbConnect = new DbConnect();
            var msReader = DbConnect.getmysqlread(mysqlRead);
            if (msReader.HasRows)
            {
                guaduation gu=new guaduation(msReader);
                gu.Show();
            }
            else
            {
                MessageBox.Show("无此用户", "error!!");
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {            
            Cardno.Text = Connect.readID();
        }

        private void go_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel1.Visible = true;
            var mysqlEdit = "UPDATE `shoppingcard`.`stu_info` SET `Sno` = '"+nSno.Text.Trim()+"', " +
                            "`Sid`      = '"+nSid.Text.Trim()+"', " +
                            "`Sname`    = '"+nSname.Text.Trim()+"', " +
                            "`Ssex`     = '"+nSsex.Text.Trim()+"', " +
                            "`Sbirth`   = '"+nSbirth.Text.Trim()+"', " +
                            "`Sdept`    = '"+nSdept.Text.Trim()+"', " +
                            "`Sspecial` = '"+nSspecial.Text.Trim()+"', " +
                            "`Sclass`   = '"+nSclass.Text.Trim()+"', " +
                            "`Saddr`    = '"+nSaddr.Text.Trim()+"', " +
                            "`Cardno`   = '"+nCardno.Text.Trim()+"', " +
                            "`Spwd`     = '"+lib.getMD5(nSpwd.Text.Trim())+"' " +
                            "WHERE `stu_info`.`Sno` = '"+oldNo+"';";
            var dbConnect = new DbConnect();
            DbConnect.getmysqlcom(mysqlEdit);
            nSno.Text = "";
            oldNo = "";
            nSid.Text = "";
            nSname.Text = "";
            nSsex.Text = "";
            nSbirth.Text = "";
            nSdept.Text = "";
            nSspecial.Text = "";
            nSclass.Text = "";
            nSaddr.Text = "";
            nCardno.Text = "";

        }


    }
}
