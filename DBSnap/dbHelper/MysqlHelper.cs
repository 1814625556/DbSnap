using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace DBSnap.dbHelper
{
    public class MysqlHelper
    {
        public static MySqlConnection GetConnection(string conStr)
        {
            return new MySqlConnection(conStr);
        }
        public static void CloseConnection(MySqlConnection conn)
        {
            conn.Close();
        }
        public static DataSet GetDataSetByTables(MySqlConnection conn, List<string> tableList)
        {
            var dt = new DataSet();
            var sql = "";
            foreach (var table in tableList)
            {
                sql += $"select * from {table};";
            }
            using (MySqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;
                var adapter = new MySqlDataAdapter(cmd);
                dt = new DataSet();
                adapter.Fill(dt);
            }
            return dt;
        }
        public static List<string> GetTabelNameList(MySqlConnection conn)
        {
            var tableList = new List<string>();
            conn.Open(); // 打开数据库连 
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT Table_name FROM information_schema.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_SCHEMA = 'silverbox' ORDER BY TABLE_NAME";
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        tableList.Add(dr.GetString(0));
                    }

                }
            }
            return tableList;
        }
        public static DataTable GetDataTable(string sqlStr, MySqlConnection conn)
        {
            var dt = new DataTable();
            conn.Open(); // 打开数据库连 
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = sqlStr;
                var adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            return dt;
        }
    }
}
