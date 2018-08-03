using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace ReadFile
{
    class Program
    {
        static void Main(string[] args)
        {
            //string data = File.ReadAllText(@"E:\share\人和\User.ini",Encoding.GetEncoding("gb2312"));
            //Match match = Regex.Match(data, "");
            INIClass inic = new INIClass(@"E:\share\人和\User.ini");
            string password = inic.IniReadValue("", "RHDLL_数据库密码列表");
            StreamReader sr = new StreamReader(@"E:\share\人和\User.ini", System.Text.Encoding.GetEncoding("gb2312"));
            string dataSource = "";
            string dbName = "";
            //string password = "";
            while (!sr.EndOfStream)
            {
                string strTemp = sr.ReadLine();
                if (strTemp.Contains("RHDLL_服务器名称"))
                {
                    dataSource = sr.ReadLine().Replace("\"","");
                }
                if (strTemp.Contains("RHDLL_数据库名称"))
                {
                    dbName = sr.ReadLine().Replace("\"", "");
                }
                if (strTemp.Contains("RHDLL_服务器密码"))
                {
                    password = sr.ReadLine().Replace("\"", "");
                }
            }

        }
    }
}
