using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCompiler.Entities.CS
{
    public class AtributionFromLiteralInstruction : SimpleCommand
    {
        // LD A, value
        // LD H, addr_hi
        // LD L, addr_lo
        // ST [HL], A
        
        // TODO:
        // change last 3 instructions to ST [addr], A (depends on correct implementation in circuit)

        public Variable VariableResult { get; set; }

        public override IList<byte> MachineCode()
        {
            var value = Convert.ToByte(this.Tokens[2].Text);

            int addr = this.VariableResult.Address;
            byte addr_hi = HiByteOf(addr);
            byte addr_lo = LowByteOf(addr);

            var bytes = new List<byte>();

            //var test = GetInstructionBytes(EnumOpCodes.LD_R1_data, EnumRegisters.A, null, null, null, value);

            bytes.Add(0x04);           // LD A, value
            bytes.Add(0x00);   
            bytes.Add(value);

            bytes.Add(0x05);           // LD H, value
            bytes.Add(0x00);
            bytes.Add(addr_hi);

            bytes.Add(0x05);           // LD L, value
            bytes.Add(0x80);
            bytes.Add(addr_lo);

            bytes.Add(0xa0);           // ST [HL], A
            bytes.Add(0x80);
            bytes.Add(0x00);

            return bytes;
        }

        //protected IList<byte> GetInstructionBytes(EnumOpCodes opcode, EnumRegisters? r1, EnumRegisters? r2, int? io_addr, int? addr, byte? data)
        //{
        //    var bytes = new List<byte>();

        //    int firstByte = (((int)opcode) << 2) & ((int)r1 ;

        //    return bytes;
        //}
    }
}
