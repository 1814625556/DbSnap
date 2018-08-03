using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlliteTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("AAAAA");
                SqlLiteHelp.QueryAllTableInfo();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }
    }
}
