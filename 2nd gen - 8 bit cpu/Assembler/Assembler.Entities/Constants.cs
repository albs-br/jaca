using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Entities
{
    public static class Constants
    {
        public static string VERSION = "0.11.1";

        // Work
        //public static string BASE_PATH = @"C:\Users\xdad\Source\Repos\jaca\2nd gen - 8 bit cpu\";

        // Home
        public static string BASE_PATH = @"C:\Users\albs_\Source\Repos\jaca\2nd gen - 8 bit cpu";

        public static string LOGISIM_BIN_FILEPATH = BASE_PATH;



        public static string[] REGISTERS = { "A", "B", "H", "L", "C", "D", "E", "F" };

        public const int BASE_PROGRAM_ADDRESS = 0x0000;
        public const int BASE_VAR_ADDRESS = 0x0c00;

        public static string[] REGISTERS_BANK_A = { "A", "B", "H", "L" };
        public static string[] REGISTERS_BANK_B = { "C", "D", "E", "F" };
        public static string[] ALU_INSTRUCTIONS = {
            "ADD", "SUB", "NOT", "AND",
            "OR", "XOR", "NOR", "XNOR",
            "INC", "DEC", "DNW", "SUBM",
            "SHL", "SHR"
        };
        public static string[] SINGLE_OPERAND_ALU_INSTRUCTIONS = {
            "NOT", "INC", "DEC", "DNW", "SHL", "SHR"
        };


    }
}
