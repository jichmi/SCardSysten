using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;


    /// <summary>
    /// 数据库访问类
    /// 使用Mysql
    /// creat by 江诚铭
    /// @2015.12.17 3：38
    /// </summary>

namespace SCardSystem
{
    public class DbConnect
    {
        #region  建立MySql数据库连接
        /// <summary>
        /// 建立连接
        /// </summary>
        /// <returns>MysqlConnection对象</returns>
        public static MySqlConnection getmysqlcon()
        {
            string M_str_sqlcon = "server=localhost;user id=root;password=;database=shoppingcard;Charset=utf8"; //Mysql服务器设置
            MySqlConnection myCon = new MySqlConnection(M_str_sqlcon);
            return myCon;
        }
        #endregion

        #region  执行MySqlCommand命令
        /// <summary>
        /// 执行MySqlCommand
        /// </summary>
        /// <param name="M_str_sqlstr">SQL语句</param>
        public static void getmysqlcom(string M_str_sqlstr)
        {
            MySqlConnection mysqlcon = getmysqlcon();
            mysqlcon.Open();
            MySqlCommand mysqlcom = new MySqlCommand(M_str_sqlstr, mysqlcon);
            mysqlcom.ExecuteNonQuery();
            mysqlcom.Dispose();
            mysqlcon.Close();
            mysqlcon.Dispose();
        }
        #endregion

        #region  创建MySqlDataReader对象
        /// <summary>
        /// 创建一个MySqlDataReader对象
        /// </summary>
        /// <param name="M_str_sqlstr">SQL语句</param>
        /// <returns>返回MySqlDataReader对象</returns>
        public static MySqlDataReader getmysqlread(string M_str_sqlstr)
        {
            MySqlConnection mysqlcon = getmysqlcon();
            MySqlCommand mysqlcom = new MySqlCommand(M_str_sqlstr, mysqlcon);
            mysqlcon.Open();
            MySqlDataReader mysqlread = mysqlcom.ExecuteReader();
            return mysqlread;
        }
        #endregion

    }
}

