using CSCompiler.Entities.CS;
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
        /// Step 3 of 3 - Convert commands to machine code
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public static MachineCodeProgram ConvertCommandsToMachineCode(CSProgram csProgram)
        {
            var machineCodeProgram = new MachineCodeProgram();

            var address = Constants.BASE_ADDR_PROGRAM;

            foreach (var command in csProgram.Commands)
            {
                foreach (var b in command.MachineCode())
                {
                    machineCodeProgram.Bytes[address] = b;

                    address++;
                }
            }

            return machineCodeProgram;
        }
    }
}
