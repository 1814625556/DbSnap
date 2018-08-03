using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;//需要引入System,Manager
using System.Text;
using System.Threading.Tasks;

namespace DirectoryTest
{
    class Program
    {
        static void Main(string[] args)
        {
            testSubString();
            //string cpuid = Get_CPUID();
            //DirectoryHelper.GetCurrentDirectory();
            Console.ReadKey();
        }
        static void testSubString()
        {
            string str = "0123";
            str = str.Substring(0, 2);
        }
        public static string Get_CPUID()
        {
            try
            {
                //需要在解决方案中引用System.Management.DLL文件  
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                string strCpuID = null;
                foreach (ManagementObject mo in moc)
                {
                    strCpuID = mo.Properties["ProcessorId"].Value.ToString();
                    mo.Dispose();
                    break;
                }
                return strCpuID;
            }
            catch
            {
                return "";
            }
        }
    }
}
