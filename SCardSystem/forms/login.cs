using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using SCardSystem.libs;


namespace SCardSystem
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
            //界面初始化
            MySqlConnection Conn = new MySqlConnection("server=localhost;user id=root;password=;database=shoppingcard;Charset=utf8");
            Conn.Open();

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string psw = lib.getMD5(this.Password.Text.Trim());
            String userName = User.Text.Trim();
            String type = UserType.Text;
            DbConnect dbConnect = new DbConnect();

            if (type == "消费点")
            {
                MySqlDataReader checkLogin = DbConnect.getmysqlread("SELECT * FROM `shop_spot` WHERE `SPno` = " + userName + " AND `SPpwd` LIKE '" + psw + "' ORDER BY `SPno` ASC ");
                if (checkLogin.HasRows)
                {
                    shop shopForm=new shop();
                    this.Hide();
                    shopForm.Show();
                }
                else
                {
                    MessageBox.Show( "userID or password error!!","error!");
                }
            }
            else if (type=="服务点")
            {
                MySqlDataReader checkLogin = DbConnect.getmysqlread("SELECT * FROM `card_ct` WHERE `CCno` = " + userName + " AND `CCpwd` LIKE '" + psw + "' ORDER BY `CCno` ASC ");
                if (checkLogin.HasRows)
                {
                    service serForm=new service();
                    this.Hide();
                    serForm.Show();
                }
                else
                {
                    MessageBox.Show("userID or password error!!", "error!");
                }
            }
            else if (type=="系统管理员")
            {
                MySqlDataReader checkLogin = DbConnect.getmysqlread("SELECT * FROM `admin_info` WHERE `Ano` = " + userName + " AND `Apwd`  LIKE '" + psw + "' ORDER BY `Ano` ASC ");
                if (checkLogin.HasRows)
                {
                    admin adminForm = new admin(userName);
                    this.Hide();
                    adminForm.Show();
                }
                else
                {
                    MessageBox.Show("userID or password error!!", "error!");
                }
            }
            else
            {
                MessageBox.Show("Choose the type!!", "error!");
            }
         }
    }
}
