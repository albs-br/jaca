using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCompiler.Entities.CS
{
    public class VarDefinitionInstruction : SimpleCommand
    {
        // LD A, value
        // ST [addr], A

        public IList<byte> MachineCode()
        {
            var value = Convert.ToByte(this.Tokens[3]);

            var addr = Constants.BASE_ADDR_VARIABLES + this.csProgram.Variables.Count - 1;

            var bytes = new List<byte>();

            bytes.Add(4);           // LD A, value
            bytes.Add(0);   
            bytes.Add(value);

            bytes.Add(10);           // ST [addr], A
            //bytes.Add(addr_hi);
            //bytes.Add(addr_lo);

            return bytes;
        }
    }
}
