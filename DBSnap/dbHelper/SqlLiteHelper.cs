using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data.Common;
using System.Data;
using System.Collections;
using System.Diagnostics;

namespace DBSnap.dbHelper
{
    public class SqlLiteHelper
    {
        public static SQLiteConnection GetConnection(string conStr)
        {
            return new SQLiteConnection(conStr);
        }
        public static void CloseConnection(SQLiteConnection conn)
        {
            conn.Close();
        }
        public static DataSet GetDataSetByTables(SQLiteConnection conn, List<string> tableList)
        {
            DataSet dt = new DataSet();
            string sql = "";
            foreach (var table in tableList)
            {
                sql += $"select * from {table};";
            }
            using (SQLiteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                dt = new DataSet();
                adapter.Fill(dt);
            }
            return dt;
        }
        public static List<string> GetTabelNameList(SQLiteConnection conn)
        {
            List<string> tableList = new List<string>();
            conn.Open(); // 打开数据库连 
            using (SQLiteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' ORDER BY name";
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        tableList.Add(dr.GetString(0));
                    }

                }
            }
            return tableList;
        }
        public static DataTable GetDataTable(string sqlStr, SQLiteConnection conn)
        {
            DataTable dt = new DataTable();
            conn.Open(); // 打开数据库连 
            using (SQLiteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = sqlStr;
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(dt);
            }
            return dt;
        }
        /// <summary>
        /// 执行增删改查
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static int ExcuteNoQuery(string sqlStr, SQLiteConnection conn)
        {
            int row = 0;
            using (SQLiteCommand cmd = new SQLiteCommand(sqlStr, conn))
            {
                try
                {
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                }
                catch (System.Data.SQLite.SQLiteException E)
                {
                    throw new Exception(E.Message);
                }
            }
            return row;
        }
    }
}
