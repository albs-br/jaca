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

        public static string[] VALID_TYPES = { "byte" };

        public static bool IsValidType(string text)
        {
            foreach (string t in Constants.VALID_TYPES)
            {
                if (text == t)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true to Space, New Line and Tab characters
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsIrrelevantChar(char c)
        {
            return (c == ' ' || c == '\r' || c == '\n' || c == '\t');
        }
    }

}
