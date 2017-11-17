    using CSCompiler.Entities.CS.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCompiler.Entities.CS
{
    public abstract class Command
    {
        public Command()
        {
            this.Tokens = new List<Token>();
            this.AssemblyCommands = new List<string>();
        }

        public Command ParentCommand { get; set; }

        public CSProgram CsProgram { get; set; }

        public IList<Token> Tokens { get; set; }

        public int BaseInstructionAddress { get; set; }

        public IList<string> AssemblyCommands { get; set; }

        public abstract IList<byte> MachineCode();

        /// <summary>
        /// Get High Byte of a 16 bit number
        /// </summary>
        /// <param name="number">16 bit number (0 to 65535)</param>
        /// <returns></returns>
        protected byte HiByteOf(int number)
        {
            return (byte)(number >> 8);
        }

        /// <summary>
        /// Get Low Byte of a 16 bit number
        /// </summary>
        /// <param name="number">16 bit number (0 to 65535)</param>
        /// <returns></returns>
        protected byte LowByteOf(int number)
        {
            return (byte)(number & 0xFF);
        }

        public void AddAssemblyCommand(string assemblyCommand)
        {
            var str = string.Format("{0:x4}    {1}", BaseInstructionAddress, assemblyCommand);

            AssemblyCommands.Add(str);
        }
    }
}
