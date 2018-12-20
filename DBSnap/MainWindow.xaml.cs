using DBSnap.Common;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Windows;

namespace DBSnap
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private List<string> TableList { get; set; }//所有的表名
        private DataSet Dt1 { get; set; }
        private DataSet Dt2 { get; set; }
        private string Connsql { get; set; }
        private void Refresh(object sender, RoutedEventArgs e)
        {
            SizeSet_TextBoxResult();
            //this.ListBox_Tables.Items.Clear();
            Dt1 = null;
            Dt2 = null;
            //this.btn_first.IsEnabled = false;
            //this.btn_second.IsEnabled = false;
            //this.btn_search.IsEnabled = false;
            //this.btn_compare.IsEnabled = false;
            Result.Text = "刷新成功...";
        }
        private void Button_Conn(object sender, RoutedEventArgs e)
        {
            SizeSet_TextBoxResult();
            //string constr = "server=127.0.0.1;database=myschool;integrated security=SSPI";
            TableList = new List<string>();
            if (string.IsNullOrEmpty(this.txt_Str.Text.Trim()))
            {
                MessageBox.Show("连接字符串为空");
                return;
            }
            try
            {
                Connsql = this.txt_Str.Text.ToString();         //对连接字符串赋值
                switch (this.Com_box.SelectedIndex)
                {
                    case 0:
                        using (SqlConnection con = dbHelper.SqlServerHelper.GetConnection(Connsql))
                            TableList = dbHelper.SqlServerHelper.GetTabelNameList(con);
                        break;
                    case 1:
                        break;
                    case 2:
                        using (OracleConnection Oraclcon = dbHelper.OracleHelper.GetConnection(Connsql))
                            TableList = dbHelper.OracleHelper.GetTabelNameList(Oraclcon);
                        break;
                    case 3:
                        break;
                    case 4:
                        using (SQLiteConnection sqlLitecon = new SQLiteConnection(Connsql))
                            TableList = dbHelper.SqlLiteHelper.GetTabelNameList(sqlLitecon);
                        break;
                }
           
                for (int i=0;i<TableList.Count;i++)
                {
                    this.ListBox_Tables.Items.Add($"{i}:{TableList[i]}");
                }
                this.btn_first.IsEnabled = true;
                this.btn_second.IsEnabled = true;
                this.btn_search.IsEnabled = true;
                this.btn_compare.IsEnabled = true;
                this.Result.Text = "连接测试Success";
            }
            catch (Exception ex)
            {
                this.Result.Text = ex.Message;
            }
        }
        //第一次
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SizeSet_TextBoxResult();
            try
            {
                switch (this.Com_box.SelectedIndex)
                {
                    case 0:
                        using (SqlConnection conn = dbHelper.SqlServerHelper.GetConnection(Connsql))
                            Dt1 = dbHelper.SqlServerHelper.GetDataSetByTables(conn, TableList);
                        break;
                    case 1:
                        break;
                    case 2:
                        using (OracleConnection orconn = dbHelper.OracleHelper.GetConnection(Connsql))
                            Dt1 = dbHelper.OracleHelper.GetDataSetByTables(orconn, TableList);
                        break;
                    case 3:
                        break;
                    case 4:
                        using (SQLiteConnection sqlLitecon = new SQLiteConnection(Connsql))
                            Dt1 = dbHelper.SqlLiteHelper.GetDataSetByTables(sqlLitecon, TableList);
                        break;
                }
                
                this.Result.Text = "1ok";
            }
            catch (Exception ex)
            {
                this.Result.Text = ex.Message;
            }
        }
        //第二次
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SizeSet_TextBoxResult();
            try
            {
                switch (this.Com_box.SelectedIndex)
                {
                    case 0://sqlserver
                        using (SqlConnection conn = dbHelper.SqlServerHelper.GetConnection(Connsql))
                            Dt2 = dbHelper.SqlServerHelper.GetDataSetByTables(conn, TableList);
                        break;
                    case 1://mysql
                        break;
                    case 2://oracle
                        using (OracleConnection orconn = dbHelper.OracleHelper.GetConnection(Connsql))
                            Dt2 = dbHelper.OracleHelper.GetDataSetByTables(orconn, TableList);
                        break;
                    case 3://postgresql
                        break;
                    case 4://sqllite
                        using (SQLiteConnection sqlLitecon = new SQLiteConnection(Connsql))
                            Dt2 = dbHelper.SqlLiteHelper.GetDataSetByTables(sqlLitecon, TableList);
                        break;
                }

                this.Result.Text = "2ok";
            }
            catch (Exception ex)
            {
                this.Result.Text = ex.Message;
            }
        }
        /// <summary>
        /// 对比函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Compare(object sender, RoutedEventArgs e)
        {
            SizeSet_TextBoxResult();
            try
            {
                this.Result.Text = "";
                for (int i = 0; i < Dt1.Tables.Count; i++)
                {
                    var t1 = Dt1.Tables[i];
                    var t2 = Dt2.Tables[i];
                    if (t1.Rows.Count == 0 && t2.Rows.Count == 0)
                        continue;
                    if (t1.Rows.Count != t2.Rows.Count) //表示删除，添加
                    {
                        if (t1.Rows.Count > t2.Rows.Count)//删除操作
                        {
                            this.Result.AppendText($"=====================================================" +
                                $"\r\n表名称 : {TableList[i]} -- 操作 : Delete\r\n");
                            List<string> deleteList = CompareHelper.GetListStrFromTable(t1).Except(CompareHelper.GetListStrFromTable(t2)).ToList();
                            foreach (var str in deleteList)
                            {
                                this.Result.AppendText(str);
                            }
                        }
                        else                              //添加操作
                        {
                            this.Result.AppendText($"=====================================================" +
                                $"\r\n表名称 : {TableList[i]} -- 操作 : Insert\r\n");
                            List<string> deleteList = CompareHelper.GetListStrFromTable(t2).Except(CompareHelper.GetListStrFromTable(t1)).ToList();
                            foreach (var str in deleteList)
                            {
                                this.Result.AppendText(str);
                            }
                        }
                    }
                    else                                   //修改操作 
                    {
                        Tuple<bool, string> tp = CompareHelper.IsUpdate(t1, t2);
                        if (tp.Item1)
                        {
                            this.Result.AppendText($"=====================================================" +
                                $"\r\n表名称 : {TableList[i]} -- 操作 : Update\r\n");
                            this.Result.AppendText(tp.Item2);
                        }
                    }
                }
                if (this.Result.Text.Trim() == "")
                {
                    this.Result.Text = "没有改动";
                }
            }
            catch (Exception ex)
            {
                this.Result.Text = ex.Message;
            }
        }
        /// <summary>
        /// SQL查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sqlStr = this.Txt_sql.Text.ToString();
                if (!string.IsNullOrEmpty(sqlStr) && sqlStr.ToLower().Contains("select") &&
                    sqlStr.ToLower().Contains("from"))
                {
                    DataTable dt = new DataTable();
                    switch (this.Com_box.SelectedIndex)
                    {
                        case 0:
                            using (SqlConnection conn = new SqlConnection(Connsql))
                                dt = dbHelper.SqlServerHelper.GetDataTable(sqlStr, conn);
                            break;
                        case 1:
                            break;
                        case 2:
                            using (OracleConnection conn = new OracleConnection(Connsql))
                                dt = dbHelper.OracleHelper.GetDataTable(sqlStr, conn);
                            break;
                        case 3:
                            break;
                        case 4:
                            using (SQLiteConnection conn = new SQLiteConnection(Connsql))
                                dt = dbHelper.SqlLiteHelper.GetDataTable(sqlStr, conn);
                            break;
                    }
                    SizeSet_DataGrid();
                    this.Dg_Table.ItemsSource = dt.DefaultView;
                }
                else
                {
                    switch (this.Com_box.SelectedIndex)
                    {
                        case 0:
                            break;
                        case 1:
                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                        case 4:
                            using (SQLiteConnection conn = new SQLiteConnection(Connsql))
                                this.Result.Text = $"影响行数：{dbHelper.SqlLiteHelper.ExcuteNoQuery(sqlStr, conn)}";
                            break;
                    }
                }
                
            }
            catch (Exception ex)
            {
                SizeSet_TextBoxResult();
                this.Result.Text = ex.Message;
            }
        }

        #region 表格控件样式设计
        /// <summary>
        /// 设置输出窗口
        /// </summary>
        private void SizeSet_TextBoxResult()
        {
            this.Result.Width = 644;
            this.Result.Height = 390;
            this.Result.HorizontalAlignment = HorizontalAlignment.Left;
            this.Result.VerticalAlignment = VerticalAlignment.Top;
            this.Result.Margin = new Thickness(304,99,0,0);
            this.Result.Visibility = Visibility.Visible;

            this.Dg_Table.Width = 60;
            this.Dg_Table.Height = 100;
            this.Dg_Table.Margin = new Thickness(230, 276, 0, 0);
            this.Dg_Table.HorizontalAlignment = HorizontalAlignment.Left;
            this.Dg_Table.VerticalAlignment = VerticalAlignment.Top;
            this.Dg_Table.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// 设置数据表格窗口
        /// </summary>
        private void SizeSet_DataGrid()
        {
            this.Dg_Table.Width = 644;
            this.Dg_Table.Height = 390;
            this.Dg_Table.HorizontalAlignment = HorizontalAlignment.Left;
            this.Dg_Table.VerticalAlignment = VerticalAlignment.Top;
            this.Dg_Table.Margin = new Thickness(304,99,0,0);
            this.Dg_Table.Visibility = Visibility.Visible;

            this.Result.Width = 60;
            this.Result.Height = 100;
            this.Result.Margin = new Thickness(230, 276, 0, 0);
            this.Result.HorizontalAlignment = HorizontalAlignment.Left;
            this.Result.VerticalAlignment = VerticalAlignment.Top;
            this.Result.Visibility = Visibility.Collapsed;
        }
        #endregion
        /// <summary>
        /// 根据指定字段搜索全库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Search_ClickAll(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.Txt_sql.Text) || Dt1 == null) return;
            for(int i = 0; i < Dt1.Tables.Count; i++)
            {
                var result = CompareHelper.SearchAll(Dt1.Tables[i], this.Txt_sql.Text);
                if (result != "")
                {
                    this.Result.Text += $"{result} \r\n";
                }
            }
        }
    }
}
