using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler
{
    public class Converter
    {
        string[] registers = { "A", "B", "C", "D", "E", "F", "G", "H" };

        public byte[] ConvertLine(string line)
        {
            line = line.Trim().ToUpper();

            // Replace all double spaces
            do
            {
                line = line.Replace("  ", " ");
            }
            while (line.Contains("  "));
    


            byte opcode = 0;
            byte r1Addr = 0;

            var lineParts = line.Split(' ');



            var instruction = lineParts[0].ToString();

            if (instruction == "LD")
            {
                var r1 = lineParts[1].Replace(",", "");

                r1Addr = RegisterNameToAddress(r1);

                var thirdPart = lineParts[2].ToString();

                if (thirdPart.StartsWith("0X"))
                {
                    opcode = 1;
                    byte data = Convert.ToByte(thirdPart.Replace("0X", "").ToString(), 16);

                    // return 3 bytes of immediate instruction
                    return ImediateInstruction(opcode, r1Addr, data);
                }
                else if(registers.Contains(thirdPart))
                {
                    opcode = 2;
                    byte r2Addr = RegisterNameToAddress(thirdPart);

                    return ByRegisterInstruction(opcode, r1Addr, r2Addr);
                }
                else if (thirdPart.StartsWith("[0X") && thirdPart.EndsWith("]"))
                {
                    opcode = 3;
                    thirdPart = thirdPart.Replace("[0X", "").Replace("]", "");
                    
                    if (thirdPart.Length > 3)
                    {
                        throw new Exception("Max address possible is 12 bits");
                    }

                    byte addr;

                    //if (thirdPart.Length == 3)
                    //{
                    //    var addrHi = thirdPart.Substring(0, 4);
                    //    var addrLo = thirdPart.Substring(4, 8);
                        
                    //    r1Addr = Convert.ToByte(addrHi.ToString(), 16);


                    //}
                    //else
                    {
                        addr = Convert.ToByte(thirdPart.ToString(), 16);
                    }
                    
                    return DirectInstruction(opcode, r1Addr, addr);
                }
            }

            throw new Exception("The line could not be converted");
        }

        private byte RegisterNameToAddress(string registerName)
        {
            if (registers.Contains(registerName))
            {
                return Convert.ToByte(Array.IndexOf(registers, registerName));
            }
            else
            {
                throw new Exception("Invalid register name");
            }

            //switch (registerName)
            //{
            //    case "A":
            //        return 0;

            //    case "B":
            //        return 1;

            //    case "C":
            //        return 2;

            //    //...

            //    default:
            //        throw new Exception("Invalid register name");
            //    //break;
            //}
        }

        private byte[] ImediateInstruction(byte opcode, byte r1Adrr, byte data)
        {
            string opcodeBinary = Convert.ToString(opcode, 2);
            opcodeBinary = opcodeBinary.PadLeft(6, '0');

            string r1AddrBinary = Convert.ToString(r1Adrr, 2);
            r1AddrBinary = r1AddrBinary.PadLeft(3, '0');

            string dataBinary = Convert.ToString(data, 2);
            dataBinary = dataBinary.PadLeft(8, '0');



            string instructionBinary = opcodeBinary + r1AddrBinary + "0000000" + dataBinary;



            byte[] bytes = InstructionBinaryToBytes(instructionBinary);

            return bytes;
        }

        private byte[] ByRegisterInstruction(byte opcode, byte r1Adrr, byte r2Adrr)
        {
            string opcodeBinary = Convert.ToString(opcode, 2);
            opcodeBinary = opcodeBinary.PadLeft(6, '0');

            string r1AddrBinary = Convert.ToString(r1Adrr, 2);
            r1AddrBinary = r1AddrBinary.PadLeft(3, '0');

            string r2AddrBinary = Convert.ToString(r2Adrr, 2);
            r2AddrBinary = r2AddrBinary.PadLeft(3, '0');



            string instructionBinary = opcodeBinary + r1AddrBinary + r2AddrBinary + "000000000000";



            byte[] bytes = InstructionBinaryToBytes(instructionBinary);

            return bytes;
        }

        private byte[] DirectInstruction(byte opcode, byte r1Adrr, byte addr)
        {
            string opcodeBinary = Convert.ToString(opcode, 2);
            opcodeBinary = opcodeBinary.PadLeft(6, '0');

            string r1AddrBinary = Convert.ToString(r1Adrr, 2);
            r1AddrBinary = r1AddrBinary.PadLeft(3, '0');

            string addrBinary = Convert.ToString(addr, 2);
            addrBinary = addrBinary.PadLeft(12, '0');



            string instructionBinary = opcodeBinary + r1AddrBinary + "000" + addrBinary;



            byte[] bytes = InstructionBinaryToBytes(instructionBinary);

            return bytes;
        }
        private byte[] InstructionBinaryToBytes(string instructionBinary)
        {
            byte[] bytes = new byte[3];

            bytes[0] = Convert.ToByte(instructionBinary.Substring(0, 8), 2);
            bytes[1] = Convert.ToByte(instructionBinary.Substring(8, 8), 2);
            bytes[2] = Convert.ToByte(instructionBinary.Substring(16, 8), 2);

            return bytes;
        }
    }
}
