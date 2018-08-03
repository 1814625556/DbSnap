using System;
using System.Data.OleDb;
using System.Data;
using System.IO;

namespace Access
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Path.Combine(@"E:\分享", @"mdb\information.mdb");
            bool success = false;
            ReadAllData("myguestinfo", @"E:\分享\mdb\information.mdb", ref success);

        }
        public static DataTable ReadAllData(string tableName, string mdbPath, ref bool success)
        {
            DataTable dt = new DataTable();
            try
            {
                DataRow dr;

                //1、建立连接 C#操作Access之读取mdb  

                string strConn = $"Provider=Microsoft.Jet.OLEDB.4.0; Data Source={mdbPath};jet oledb:database password=11929093;";
                //Provider = Microsoft.Jet.OLEDB.4.0; Jet OLEDB:DataBase Password = Zd$1234; Data Source = **.mdb
                OleDbConnection odcConnection = new OleDbConnection(strConn);

                //2、打开连接 C#操作Access之读取mdb  
                odcConnection.Open();

                //建立SQL查询   
                OleDbCommand odCommand = odcConnection.CreateCommand();

                //3、输入查询语句 C#操作Access之读取mdb  

                odCommand.CommandText = "select * from " + tableName;

                //建立读取   
                OleDbDataReader odrReader = odCommand.ExecuteReader();

                //查询并显示数据   
                odrReader.Read();
                string dbhost = odrReader.GetString(1);
                string username = odrReader.GetString(2);
                string password = odrReader.GetString(3);

                int size = odrReader.FieldCount;
                for (int i = 0; i < size; i++)
                {
                    DataColumn dc;
                    dc = new DataColumn(odrReader.GetName(i));
                    dt.Columns.Add(dc);
                }
                while (odrReader.Read())
                {
                    dr = dt.NewRow();
                    for (int i = 0; i < size; i++)
                    {
                        //dr[odrReader.GetName(i)] =
                        //odrReader[odrReader.GetName(i)].ToString();
                        
                    }
                    dt.Rows.Add(dr);
                }
                //关闭连接 C#操作Access之读取mdb  
                odrReader.Close();
                odcConnection.Close();
                success = true;
                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                success = false;
                return dt;
            }
        }
    }
}
