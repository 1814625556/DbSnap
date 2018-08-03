using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data.Common;
using System.Threading;

namespace SqliteTestWinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
                    textBox3.Text = "连接字符串为空";
                if (textBox2.Text.ToLower().Contains("select") && textBox2.Text.ToLower().Contains("from"))
                {
                    QueryAllTableInfo();
                }
                else
                {
                    textBox3.Text = "必须是查询语句。。。";
                }
            }
            catch (Exception ex)
            {
                textBox3.Text = ex.Message;
            }
        }
        public void QueryAllTableInfo()
        {
            string path = textBox1.Text; // @"C:\sqlite\sqlite-tools-win32-x86-3240000\chenchang.db";
            SQLiteConnection cn = new SQLiteConnection(path);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();

                DbCommand comm = cn.CreateCommand();
                comm.CommandText = textBox2.Text; // "select * from COMPANY";
                comm.CommandType = CommandType.Text;
                using (IDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                           textBox3.AppendText($"{reader[i]}");
                        }
                        textBox3.AppendText("\r\n");
                    }
                }
            }
            cn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.textBox3.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //for (int i = 0; i < 10000; i++)
            //{
            //    ExecuteSql("update t_version set datatype='test' where version='ec824cdac5cd76a232aebc566e511d0a'");
            //}
            ExecuteSqlTran(this.textBox2.Text);

        }
        public int ExecuteSql(string SQLString)
        {
            using (SQLiteConnection connection = new SQLiteConnection(this.textBox1.Text.ToString()))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SQLite.SQLiteException E)
                    {
                        connection.Close();
                        throw new Exception(E.Message);
                    }
                }
            }
        }
        public bool ExecuteSqlTran(string SQLString)
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.textBox1.Text.ToString()))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;
                SQLiteTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    this.textBox3.Text = "begin trans\r\n";
                    Thread.Sleep(10000);
                    this.textBox3.Text += "after 10 s";
                    cmd.CommandText = SQLString;
                    cmd.ExecuteNonQuery();

                    this.textBox3.Text = "begin trans\r\n";
                    Thread.Sleep(10000);
                    this.textBox3.Text += "after 10 s";

                    tx.Commit();
                    return true;
                }
                catch (System.Data.SQLite.SQLiteException E)
                {
                    tx.Rollback();
                    return false;
                    // throw new Exception(E.Message);
                }
            }
        }
    }
}
