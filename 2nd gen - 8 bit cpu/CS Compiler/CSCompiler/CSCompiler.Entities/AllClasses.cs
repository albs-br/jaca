using CSCompiler.Entities.MachineCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCompiler.Entities
{
    public class Constants
    {
        internal const int SIZE_RAM_MEMORY = 32768;
        internal const int BASE_ADDR_PROGRAM = 0;
        internal const int BASE_ADDR_SUBROUTINES = 1000;
        internal const int BASE_ADDR_VARIABLES = 2000;
    }
}

namespace CSCompiler.Entities.CS
{
    public class CSProgram
    {
        public string SourceCodeText;

        public IList<Command> Commands { get; set; }

        public MachineCodeProgram Compile()
        {
            var machineCodeProgram = new MachineCodeProgram();

            //string[] breakLineChars = ["a", "b"];

            //var splittedText = this.SourceCodeText.Split()

            for (int i = 0; i < this.SourceCodeText.Length; i++)
            { 
                //TODO: continue here
                char actualChar = SourceCodeText.ElementAt(i);

                if (actualChar == ' ')
                {
                    continue;
                }
            }

            return machineCodeProgram;
        }
    }

    public abstract class Command
    {
    }

    public class SimpleCommand : Command
    { 
    }

    public class ComplexCommand : Command
    {
        IList<Command> InnerCommands { get; set; }
    }
}

namespace CSCompiler.Entities.MachineCode
{ 
    public class MachineCodeProgram
    {
        // Constructor
        public MachineCodeProgram()
        {
            this.bytes = new List<byte>(Constants.SIZE_RAM_MEMORY);

            // Initialize array filled of zeroes
            for (int i = 0; i < Constants.SIZE_RAM_MEMORY; i++)
            {
                this.bytes.Add(0);
            }
        }

        public IList<byte> bytes { get; set; }
    }
}
