using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DBSnap.Common
{
    public class CompareHelper
    {
        public static Tuple<bool,string> IsUpdate(DataTable dt1, DataTable dt2)
        {
            bool flag = false;
            StringBuilder sb = new StringBuilder();
            StringBuilder sbtmp;
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                sbtmp = new StringBuilder();
                sbtmp.Append(" {\r\n");
                for (int j = 0; j < dt1.Columns.Count; j++)
                {
                    if (dt1.Rows[i][j].ToString() != dt2.Rows[i][j].ToString())
                    {
                        sbtmp.Append($"\t列名称：{dt1.Columns[j].ColumnName} : {dt1.Rows[i][j]} --> {dt2.Rows[i][j]}\r\n");
                        sbtmp.Append($"\t  列类型：{dt1.Columns[j].DataType}\r\n");
                        sbtmp.Append("\t~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\r\n");
                        flag = true;
                    }
                }
                sbtmp.Append(" }\r\n");
                if (sbtmp.ToString().Length > 15)
                {
                    sb.Append(sbtmp.ToString());
                }
            }
            return new Tuple<bool, string>(flag,sb.ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<Dictionary<string, object>> GetFromTable(DataTable dt)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    result.Add(dc.ColumnName, dr[dc]);
                }
                list.Add(result);
            }
            return list;
        }
        /// <summary>
        /// 将table 转化为 list<string> 字符串列表
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<string> GetListStrFromTable(DataTable dt)
        {
            List<string> listdt = new List<string>();
            StringBuilder sb;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb = new StringBuilder();
                sb.Append(" {\r\n");
                for (int j= 0; j < dt.Columns.Count;j++)
                {
                    sb.Append($"\t列名称：{dt.Columns[j].ColumnName} : {dt.Rows[i][j].ToString()}\r\n");
                    sb.Append($"\t  列类型：{dt.Columns[j].DataType}\r\n");
                    sb.Append("\t~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\r\n");
                }
                sb.Append(" }\r\n");
                listdt.Add(sb.ToString());
            }
            return listdt;
        }
    }
}
