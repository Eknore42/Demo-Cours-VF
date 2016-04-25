using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string test = "chat";
            Console.WriteLine(test);
            string res = BCrypt.Net.BCrypt.HashPassword(test);
            Console.WriteLine(res);
            bool val = BCrypt.Net.BCrypt.Verify("rat", res);
            Console.WriteLine(val);
            Console.ReadLine();
        }
    }
}
