using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCompiler.Entities.CS
{
    public class IfInstruction : ComplexCommand
    {
        //              LD H, addrLeftOperand_hi
        //              LD L, addrLeftOperand_lo
        //              LD A, [HL]
        //              LD H, addrRightOperand_hi
        //              LD L, addrRightOperand_lo
        //              LD C, [HL]
        //              SUB A, C            // test (leftVar == rightVar)
        //              JP Z, addrTrue
        //              JP addrFalse
        //  addrTrue:   // instructions inside "then" braces
        //  addrFalse:  // addr of first instruction after If

        public Variable VariableLeftOperand { get; set; }
        public Variable VariableRightOperand { get; set; }
        //public EnumComparisonType ComparisonType { get; set; }

        //public int AddressOfThen { get; set; }
        //public int AddressOfElse { get; set; }

        public override IList<byte> MachineCode()
        {
            int addrLeftOperand = this.VariableLeftOperand.Address;
            byte addrLeftOperand_hi = HiByteOf(addrLeftOperand);
            byte addrLeftOperand_lo = LowByteOf(addrLeftOperand);

            int addrRightOperand = this.VariableRightOperand.Address;
            byte addrRightOperand_hi = HiByteOf(addrRightOperand);
            byte addrRightOperand_lo = LowByteOf(addrRightOperand);

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

            //switch (ComparisonType)
            //{
            //    case EnumComparisonType.Equal:
                    bytes.Add(0x84);           // SUB A, C
                    bytes.Add(0x40);
                    bytes.Add(0x00);
            //        break;
                
            //    default:
            //        break;
            //}

            int addrTrue = BaseInstructionAddress + bytes.Count + 6; // 6 is the size in byte of the next two JP instructions
            byte addrTrue_hi = HiByteOf(addrTrue);
            byte addrTrue_lo = LowByteOf(addrTrue);
            //AddressOfThen = addrTrue;

            bytes.Add(0x18);            // JP Z, addrTrue
            bytes.Add(addrTrue_hi);     //
            bytes.Add(addrTrue_lo);

            bytes.Add(0x14);            // JP addrFalse
            int indexOfJpInstrAddr = bytes.Count;
            bytes.Add(0xff);            // temporary value, 
            bytes.Add(0xff);            // will be changed later

            // Commands of addrTrue:
            foreach (var command in this.InnerCommands)
            {
                var b = command.MachineCode();
                bytes.AddRange(b);
            }

            // Calc and put bytes of the "false" addr path of the If :
            int addrFalse = BaseInstructionAddress + bytes.Count;
            byte addrFalse_hi = HiByteOf(addrFalse);
            byte addrFalse_lo = LowByteOf(addrFalse);
            //AddressOfElse = addrFalse;

            bytes[indexOfJpInstrAddr] = addrFalse_hi;
            bytes[indexOfJpInstrAddr+1] = addrFalse_lo;

            return bytes;
        }
    }
}
