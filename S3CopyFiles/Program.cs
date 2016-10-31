using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S3Tranfers.Models;

namespace S3CopyFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            HomeModels model = new HomeModels();
            string value = model.copyFiles();
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine(value);
            Console.ReadKey();
        }
    }
}
