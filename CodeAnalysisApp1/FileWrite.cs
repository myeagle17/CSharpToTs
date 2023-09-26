using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terry
{
    internal class FileWrite
    {
        public static List<string> strings = new List<string>();

        public static void Begin()
        {
            strings.Clear();
        }
        public static void WriteLine(string msg)
        {
            strings.Add(msg);
            //Console.WriteLine(msg);
        }

        public static void End(string path)
        {
            var fileStr = "";
            using (var fs =new StreamWriter(path))
            {
                foreach (var item in strings)
                {
                    fs.WriteLine(item);
                }
                fs.Close();
            }
        }
    }
}
