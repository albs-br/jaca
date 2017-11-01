using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCompiler.Entities.CS
{
    /// <summary>
    /// Command instruction. e.g. out(0, myVar);
    /// </summary>
    public class CommandInstruction : SimpleCommand
    {
        //  LD H, addrOperand_hi
        //  LD L, addrOperand_lo
        //  LD A, [HL]
        //  OUT 0, A

        public Variable VariableOperand { get; set; }

        public override IList<byte> MachineCode()
        {
            int addrOperand = this.VariableOperand.Address;
            byte addrOperand_hi = HiByteOf(addrOperand);
            byte addrOperand_lo = LowByteOf(addrOperand);

            var bytes = new List<byte>();

            bytes.Add(0x05);           // LD H, value
            bytes.Add(0x00);
            bytes.Add(addrOperand_hi);

            bytes.Add(0x05);           // LD L, value
            bytes.Add(0x80);
            bytes.Add(addrOperand_lo);

            bytes.Add(0x10);           // LD A, [HL]
            bytes.Add(0x00);
            bytes.Add(0x00);

            bytes.Add(0x44);           // OUT 0, A
            bytes.Add(0x00);
            bytes.Add(0x00);

            return bytes;
        }
    }
}
