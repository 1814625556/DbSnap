using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Oracle.ManagedDataAccess.Client;

namespace DBSnap.dbHelper
{
    public class OracleHelper
    {
        public static OracleConnection GetConnection(string conStr)
        {
            return new OracleConnection(conStr);
        }
        public static void CloseConnection(OracleConnection conn)
        {
            conn.Close();
        }
        /// <summary>
        /// 获取指定数据库中的所有表
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
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
            for (int i = 0; i < tableList.Count; i++)
            {
                DataTable dtable = new DataTable();
                sql = $"select * from {tableList[i]}";

                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    OracleDataAdapter oda = new OracleDataAdapter(cmd);
                    try
                    {
                        oda.Fill(dtable);
                        dt.Tables.Add(dtable);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(sql);
                    }
                }
            }
            return dt;
        }
        public static DataTable GetDataTable(string sqlStr, OracleConnection conn)
        {
            DataTable dt = new DataTable();
            conn.Open(); // 打开数据库连 
            using (OracleCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = sqlStr;
                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                adapter.Fill(dt);
            }
            return dt;
        }
    }
}
