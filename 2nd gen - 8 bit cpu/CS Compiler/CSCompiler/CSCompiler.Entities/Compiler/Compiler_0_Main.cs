using CSCompiler.Entities.CS;
using CSCompiler.Entities.CS.Tokens;
using CSCompiler.Entities.MachineCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCompiler.Entities.Compiler
{
    public static partial class Compiler
    {
        /// <summary>
        /// Full compilation (source code to machine code)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static MachineCodeProgram Compile(string source)
        {
            var tokens = ConvertSourceToTokens(source);
            var csProgram = ConvertTokensToCommands(tokens);
            var machineCodeProgram = ConvertCommandsToMachineCode(csProgram);

            return machineCodeProgram;
        }
    }
}
