using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCompiler.Entities
{
    public static class Constants
    {
        public const int SIZE_RAM_MEMORY = 65536;
        public const int BASE_ADDR_PROGRAM = 32768;         // 0x8000
        public const int BASE_ADDR_SUBROUTINES = 32768 + 10000; // TODO: convert to a base 2 "round" number
        public const int BASE_ADDR_VARIABLES = 32768 + 20000; // TODO: convert to a base 2 "round" number

        public static string[] VALID_TYPES = { "byte" };
        public static string[] KEYWORDS = { "if", "for" };
        public static string[] COMMANDS = { "out" };

        public static bool IsValidType(string text)
        {
            return Constants.VALID_TYPES.Contains(text);
        }

        public static bool IsCommand(string text)
        {
            return Constants.COMMANDS.Contains(text);
        }

        public static bool IsKeyword(string text)
        {
            return Constants.KEYWORDS.Contains(text);
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

        /// <summary>
        /// Returns true to +, -, * and /
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsArithmeticSignal(char c)
        {
            return (c == '+' || c == '-' || c == '*' || c == '/');
        }
    }

}
