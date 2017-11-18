using CSCompiler.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCompiler.Entities.CS
{
    public class ArithmeticInstruction : SimpleCommand
    {
        //  LD H, addrLeftOperand_hi
        //  LD L, addrLeftOperand_lo
        //  LD A, [HL]
        //  LD H, addrRightOperand_hi
        //  LD L, addrRightOperand_lo
        //  LD C, [HL]
        //  ADD A, C         // or any other two operand ALU operation
        //  LD H, addrDest_hi
        //  LD L, addrDest_lo
        //  ST [HL], A

        public Variable VariableLeftOperand { get; set; }
        public Variable VariableRightOperand { get; set; }
        public Variable VariableDestiny { get; set; }
        public EnumArithmeticOperation ArithmeticOperation { get; set; }

        public override IList<byte> MachineCode()
        {
            int addrLeftOperand = this.VariableLeftOperand.Address;
            byte addrLeftOperand_hi = HiByteOf(addrLeftOperand);
            byte addrLeftOperand_lo = LowByteOf(addrLeftOperand);

            int addrRightOperand = this.VariableRightOperand.Address;
            byte addrRightOperand_hi = HiByteOf(addrRightOperand);
            byte addrRightOperand_lo = LowByteOf(addrRightOperand);

            int addrDestiny = this.VariableDestiny.Address;
            byte addrDestiny_hi = HiByteOf(addrDestiny);
            byte addrDestiny_lo = LowByteOf(addrDestiny);

            var bytes = new List<byte>();

            //var test = GetInstructionBytes(EnumOpCodes.LD_R1_data, EnumRegisters.A, null, null, null, value);

            bytes.Add(0x05);           // LD H, value
            bytes.Add(0x00);
            bytes.Add(addrLeftOperand_hi);

            bytes.Add(0x05);           // LD L, value
            bytes.Add(0x80);
            bytes.Add(addrLeftOperand_lo);

            bytes.Add(0x10);           // LD A, [HL]
            bytes.Add(0x00);
            bytes.Add(0x00);

            bytes.Add(0x05);           // LD H, value
            bytes.Add(0x00);
            bytes.Add(addrRightOperand_hi);

            bytes.Add(0x05);           // LD L, value
            bytes.Add(0x80);
            bytes.Add(addrRightOperand_lo);

            bytes.Add(0x12);           // LD C, [HL]
            bytes.Add(0x00);
            bytes.Add(0x00);

            switch (ArithmeticOperation)
            {
                case EnumArithmeticOperation.Addition:
                    bytes.Add(0x80);           // ADD A, C
                    bytes.Add(0x40);
                    bytes.Add(0x00);
                    break;

                case EnumArithmeticOperation.Subtraction:
                    bytes.Add(0x84);           // SUB A, C
                    bytes.Add(0x40);
                    bytes.Add(0x00);
                    break;

                case EnumArithmeticOperation.Multiplication:
                    break;
                
                case EnumArithmeticOperation.Division:
                    break;
                
                default:
                    break;
            }

            bytes.Add(0x05);           // LD H, value
            bytes.Add(0x00);
            bytes.Add(addrDestiny_hi);

            bytes.Add(0x05);           // LD L, value
            bytes.Add(0x80);
            bytes.Add(addrDestiny_lo);

            bytes.Add(0x2c);           // ST [HL], A
            bytes.Add(0x00);
            bytes.Add(0x00);
            
            return bytes;
        }
    }
}
