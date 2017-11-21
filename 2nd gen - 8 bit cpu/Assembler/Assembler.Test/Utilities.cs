using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Test
{
    public static class Utilities
    {
        public static string GetFromFile(string fileName)
        {
            var directory = "TestSourceCode";
            var extension = ".txt";

            return File.OpenText(directory + "\\" + fileName + extension).ReadToEnd();
        }
    }
}
