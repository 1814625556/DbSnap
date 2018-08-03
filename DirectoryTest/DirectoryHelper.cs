using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DirectoryTest
{
    class DirectoryHelper
    {
        public static void GetCurrentDirectory()
        {
            string path = @"C:\Program Files\octopsoft\V8\Eat\OCTOPMain";
            Console.WriteLine($"Current Directory is {path}");
            System.IO.DirectoryInfo parentPath = System.IO.Directory.GetParent(path);
            Console.WriteLine($"ParentPath is : {parentPath.FullName}");
            string iniPath = Path.Combine(parentPath.FullName, @"OCTOPSYS\file\Sys.ini");
            Console.WriteLine($"CombinePath:{iniPath}");
        }
    }
}
