using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace OracleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection("data source=127.0.0.1:10086/XE;User Id=kelin;Password=kelin;"))
                {
                    List<string> tableList = GetTabelNameList(conn);
                    DataSet dt = GetDataSetByTables(conn, tableList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static void GetTable()
        {
            using (OracleConnection conn = new OracleConnection("data source=127.0.0.1:10086/XE;User Id=kelin;Password=kelin;"))
            {
                List<string> tableList = new List<string>();
                conn.Open();
                using (OracleCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "select table_name from user_tables";
                    //cmd.Parameters.AddRange(parameters);
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        tableList.Add(reader.GetOracleString(0).ToString());
                    }
                }

            }
        }
        /// <summary>
        /// 可用
        /// </summary>
        /// <returns></returns>
        static DataTable Select()

        {

            OracleConnection conn = new OracleConnection("data source=127.0.0.1:10086/XE;User Id=kelin;Password=kelin;");//Data Source后面跟你数据库的名字，User ID为用户名，Password为密码

            conn.Open();

            string sql = "select * from CB_TYPE";

            OracleCommand cmd = new OracleCommand(sql, conn);

            OracleDataAdapter oda = new OracleDataAdapter(cmd);

            DataTable dt = new DataTable();

            oda.Fill(dt);

            conn.Close();

            cmd.Dispose();

            return dt;

        }
        public static List<string> GetTabelNameList(OracleConnection conn)
        {
            List<string> tableList = new List<string>();
            conn.Open();
            using (OracleCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "select table_name from user_tables";
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tableList.Add(reader.GetOracleString(0).ToString());
                }
            }
            return tableList;
        }
        /// <summary>
        /// 获取dataset
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="tableList"></param>
        /// <returns></returns>
        public static DataSet GetDataSetByTables(OracleConnection conn, List<string> tableList)
        {
            DataSet dt = new DataSet();
            string sql = "";
            for (int i=0;i<tableList.Count;i++)
            {
                DataTable dtable = new DataTable();
                sql = $"select * from {tableList[i]}";

                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    OracleDataAdapter oda = new OracleDataAdapter(cmd);
                    oda.Fill(dtable);
                    dt.Tables.Add(dtable);
                }
            }
            return dt;
        }
    }
}
