using CSCompiler.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCompiler.Entities.CS
{
    /// <summary>
    /// Increment (or decrement) instruction. E.g. myVar++ or myVar--
    /// </summary>
    public class IncrementInstruction : SimpleCommand
    {
        //  LD H, addrOperand_hi
        //  LD L, addrOperand_lo
        //  LD A, [HL]
        //  INC A         // or any other one operand ALU operation
        //  ST [HL], A

        public Variable VariableOperand { get; set; }
        public EnumIncrementOperation IncrementOperation { get; set; }

        public override IList<byte> MachineCode()
        {
            int addrOperand = this.VariableOperand.Address;
            byte addrOperand_hi = HiByteOf(addrOperand);
            byte addrOperand_lo = LowByteOf(addrOperand);

            var bytes = new List<byte>();

            //var test = GetInstructionBytes(EnumOpCodes.LD_R1_data, EnumRegisters.A, null, null, null, value);

            bytes.Add(0x05);           // LD H, value
            bytes.Add(0x00);
            bytes.Add(addrOperand_hi);

            bytes.Add(0x05);           // LD L, value
            bytes.Add(0x80);
            bytes.Add(addrOperand_lo);

            bytes.Add(0x10);           // LD A, [HL]
            bytes.Add(0x00);
            bytes.Add(0x00);

            AddAssemblyCommand("LD H, " + addrOperand_hi);
            AddAssemblyCommand("LD L, " + addrOperand_lo);
            AddAssemblyCommand("LD A, [HL]");

            switch (IncrementOperation)
            {
                case EnumIncrementOperation.Increment:
                    bytes.Add(0xa0);           // INC A
                    bytes.Add(0x40);
                    bytes.Add(0x00);

                    AddAssemblyCommand("INC A");
                    break;

                case EnumIncrementOperation.Decrement:
                    bytes.Add(0xa4);           // DEC A
                    bytes.Add(0x40);
                    bytes.Add(0x00);

                    AddAssemblyCommand("DEC A");
                    break;

                default:
                    break;
            }

            bytes.Add(0x2c);           // ST [HL], A
            bytes.Add(0x00);
            bytes.Add(0x00);

            AddAssemblyCommand("ST [HL], A");

            return bytes;
        }
    }
}
