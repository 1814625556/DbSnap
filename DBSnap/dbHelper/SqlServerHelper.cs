using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DBSnap.dbHelper
{
    public class SqlServerHelper
    {
        public static SqlConnection GetConnection(string conStr)
        {
            return new SqlConnection(conStr);
        }
        public static void CloseConnection(SqlConnection conn)
        {
            conn.Close();
        }
        public static DataSet GetDataSetByTables(SqlConnection conn, List<string> tableList)
        {
            DataSet dt = new DataSet();
            string sql = "";
            foreach (var table in tableList)
            {
                sql += $"select * from {table};";
            }
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                dt = new DataSet();
                adapter.Fill(dt);
            }
            return dt;
        }
        public static List<string> GetTabelNameList(SqlConnection conn)
        {
            List<string> tableList = new List<string>();
            conn.Open(); // 打开数据库连 
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT Name FROM SysObjects Where XType='U' ORDER BY Name";
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        tableList.Add(dr.GetString(0));
                    }

                }
            }
            return tableList;
        }
        public static DataTable GetDataTable(string sqlStr,SqlConnection conn)
        {
            DataTable dt = new DataTable();
            conn.Open(); // 打开数据库连 
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = sqlStr;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            return dt;
        }
    }
}
