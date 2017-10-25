using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCompiler.Entities.CS
{
    public class AtributionFromVarInstruction : SimpleCommand
    {
        // LD H, addrSource_hi
        // LD L, addrSource_lo
        // LD A, [HL]
        // LD H, addrDest_hi
        // LD L, addrDest_lo
        // ST [HL], A
        
        // TODO:
        // change all 6 instructions to ST [addr], A / LD A, [HL] (depends on correct implementation in circuit)

        public Variable VariableSource { get; set; }
        public Variable VariableDestiny { get; set; }

        public override IList<byte> MachineCode()
        {
            int addrSource = this.VariableSource.Address;
            byte addrSource_hi = HiByteOf(addrSource);
            byte addrSource_lo = LowByteOf(addrSource);

            int addrDestiny = this.VariableDestiny.Address;
            byte addrDestiny_hi = HiByteOf(addrDestiny);
            byte addrDestiny_lo = LowByteOf(addrDestiny);

            var bytes = new List<byte>();

            //var test = GetInstructionBytes(EnumOpCodes.LD_R1_data, EnumRegisters.A, null, null, null, value);

            bytes.Add(0x05);           // LD H, value
            bytes.Add(0x00);
            bytes.Add(addrSource_hi);

            bytes.Add(0x05);           // LD L, value
            bytes.Add(0x80);
            bytes.Add(addrSource_lo);

            bytes.Add(0x10);           // LD A, [HL]
            bytes.Add(0x00);
            bytes.Add(0x00);

            bytes.Add(0x05);           // LD H, value
            bytes.Add(0x00);
            bytes.Add(addrDestiny_hi);

            bytes.Add(0x05);           // LD L, value
            bytes.Add(0x80);
            bytes.Add(addrDestiny_lo);

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
