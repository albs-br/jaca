using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCompiler.Entities
{
    public class Constants
    {
        public const int SIZE_RAM_MEMORY = 32768;
        public const int BASE_ADDR_PROGRAM = 0;
        public const int BASE_ADDR_SUBROUTINES = 1000;
        public const int BASE_ADDR_VARIABLES = 2000;

        public string[] VALID_TYPES = { "byte" };
    }

}
