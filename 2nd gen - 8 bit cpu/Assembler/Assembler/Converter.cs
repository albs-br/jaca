using Assembler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler
{
    public static class Converter
    {
        private static string[] registers = { "A", "B", "H", "L", "C", "D", "E", "F"};
        private static string[] aluInstructions = {
            "ADD", "SUB", "NOT", "AND",
            "OR", "XOR", "NOR", "XNOR",
            "INC", "DEC", "DNW", "SUBM",
        };

        public static MachineCodeProgram ResolveLabels(string asmSource)
        {
            var currentAddress = 0;
            var machineCodeProgram = new MachineCodeProgram(asmSource);

            foreach (var line in machineCodeProgram.GetLines())
            {
                var trimmedLine = line.Trim();

                // test if line is a label definition
                if (trimmedLine.EndsWith(":"))
                {
                    var label = new KeyValuePair<string, int>(
                        trimmedLine.Remove(trimmedLine.Length - 1, 1),
                        currentAddress
                        );
                    machineCodeProgram.Labels.Add(label);
                }
                else
                {
                    try
                    {
                        var instruction = ConvertLine(line);
                        if (instruction != null)
                        {
                            currentAddress += instruction.Length;
                        }
                    }
                    catch (NotCommandLineException)
                    {
                        //Do nothing
                    }
                }

            }

            return machineCodeProgram;
        }

        public static void ConvertSource(MachineCodeProgram machineCodeProgram)
        {
            foreach (var line in machineCodeProgram.GetLines())
            {
                var instruction = ConvertLine(line);

                if (instruction != null)
                {
                    var text = String.Format("{0:x2} {1:x2} {2:x2}",
                        instruction[0],
                        instruction[1],
                        instruction[2]
                        );
                    machineCodeProgram.BytesAsText += text;
                    ((List<byte>)machineCodeProgram.Bytes).AddRange(instruction);
                }

                machineCodeProgram.BytesAsText += Environment.NewLine;
            }

            //return machineCodeProgram;
        }

        public static byte[] ConvertLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return null;

            line = line.Trim().ToUpper();

            if (line.EndsWith(":")) return null;

            // Ignore Comments
            if (line.StartsWith("//"))
            {
                return null;
            }


            // Replace all double spaces
            do
            {
                line = line.Replace("  ", " ");
            }
            while (line.Contains("  "));
    



            var lineParts = line.Split(' ', ',');

            var tempList = new List<string>();
            foreach (var str in lineParts)
            {
                if (!string.IsNullOrWhiteSpace(str))
                {
                    tempList.Add(str);
                }
            }

            lineParts = tempList.ToArray<string>();



            byte opcode = 0;

            var instruction = lineParts[0].ToString();

            var secondPart = "";
            if (lineParts.Length >= 2)
            {
                secondPart = lineParts[1].ToString();
            }

            byte r1Addr = 0;
            if (registers.Contains(secondPart))
            {
                r1Addr = RegisterNameToAddress(secondPart);
            }
            byte r2Addr = 0;

            var thirdPart = "";
            if (lineParts.Length >= 3)
            {
                thirdPart = lineParts[2].ToString();
            }

            if (instruction == "RET")
            {
                byte[] bytes = { 0x24, 0x00, 0x00 };

                return bytes;
            }
            else if (aluInstructions.Contains(instruction))
            {
                opcode = OpcodeOfAluInstruction(instruction);

                if (thirdPart != "")
                {
                    r2Addr = RegisterNameToAddress(thirdPart);
                }

                return ByRegisterInstruction(opcode, r1Addr, r2Addr);
            }
            else if (instruction == "JP" || instruction == "CALL")
            {
                int addr = 0;

                if (thirdPart == "")
                {
                    if (instruction == "JP")
                    {
                        opcode = 5; // JP
                    }
                    else
                    {
                        opcode = 7; // CALL
                    }

                    addr = ConvertAddress(secondPart);
                }
                else if(secondPart == "Z")
                {
                    if (instruction == "JP")
                    {
                        opcode = 6; // JP Z
                    }
                    else
                    {
                        opcode = 8; // CALL Z
                    }

                    addr = ConvertAddress(thirdPart);
                }
                else if (secondPart == "C")
                {
                    if (instruction == "JP")
                    {
                        opcode = 12; // JP Z
                    }
                    else
                    {
                        opcode = 13; // CALL Z
                    }
                    r1Addr = 0; // Necessary because C is mistankely idenfied as the C register
                    addr = ConvertAddress(thirdPart);
                }
                return DirectInstruction(opcode, r1Addr, addr);
            }
            else if (instruction == "LD")
            {
                if (thirdPart.StartsWith("0X"))
                {
                    opcode = 1;
                    byte data = Convert.ToByte(thirdPart.Replace("0X", "").ToString(), 16);

                    // return 3 bytes of immediate instruction
                    return ImediateInstruction(opcode, r1Addr, data);
                }
                else if (registers.Contains(thirdPart))
                {
                    opcode = 2;
                    r2Addr = RegisterNameToAddress(thirdPart);

                    return ByRegisterInstruction(opcode, r1Addr, r2Addr);
                }
                else if (thirdPart.StartsWith("[0X") && thirdPart.EndsWith("]"))
                {
                    opcode = 3;
                    int addr = ConvertAddress(thirdPart);

                    return DirectInstruction(opcode, r1Addr, addr);
                }
                else if (thirdPart == "[HL]")
                {
                    opcode = 4;

                    return IndirectByRegisterInstruction(opcode, r1Addr);
                }
            }
            else if (instruction == "ST")
            {
                if (secondPart.StartsWith("[0X") && secondPart.EndsWith("]"))
                {
                    opcode = 10;
                    int addr = ConvertAddress(secondPart);

                    r1Addr = RegisterNameToAddress(thirdPart);

                    return DirectInstruction(opcode, r1Addr, addr);
                }
                else if (secondPart == "[HL]")
                {
                    opcode = 11;

                    r1Addr = RegisterNameToAddress(thirdPart);

                    return IndirectByRegisterInstruction(opcode, r1Addr);
                }
            }
            else if (instruction == "OUT")
            {
                opcode = 17;

                byte IOAddr = Convert.ToByte(secondPart);

                r1Addr = RegisterNameToAddress(thirdPart);

                r2Addr = 0;
                if (lineParts.Length >= 4)
                {
                    r2Addr = RegisterNameToAddress(lineParts[3]);
                }

                return IOInstruction(opcode, IOAddr, r1Addr, r2Addr);
            }

             throw new NotCommandLineException("The line could not be converted");
        }

        private static byte RegisterNameToAddress(string registerName)
        {
            if (registers.Contains(registerName))
            {
                return Convert.ToByte(Array.IndexOf(registers, registerName));
            }
            else
            {
                throw new Exception("Invalid register name");
            }
        }

        private static byte OpcodeOfAluInstruction(string aluInstruction)
        {
            if (aluInstructions.Contains(aluInstruction))
            {
                return Convert.ToByte(Array.IndexOf(aluInstructions, aluInstruction) + 32);
            }
            else
            {
                throw new Exception("Invalid ALU instruction");
            }
        }

        private static byte[] ImediateInstruction(byte opcode, byte r1Adrr, byte data)
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

        private static byte[] ByRegisterInstruction(byte opcode, byte r1Adrr, byte r2Adrr)
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

        private static byte[] DirectInstruction(byte opcode, byte r1Adrr, int addr)
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

        private static byte[] IndirectByRegisterInstruction(byte opcode, byte r1Addr)
        {
            string opcodeBinary = Convert.ToString(opcode, 2);
            opcodeBinary = opcodeBinary.PadLeft(6, '0');

            string r1AddrBinary = Convert.ToString(r1Addr, 2);
            r1AddrBinary = r1AddrBinary.PadLeft(3, '0');



            string instructionBinary = opcodeBinary + r1AddrBinary + "000000000000000";



            byte[] bytes = InstructionBinaryToBytes(instructionBinary);

            return bytes;
        }

        private static byte[] IOInstruction(byte opcode, byte IOAddr, byte r1Addr, byte r2Addr)
        {
            string opcodeBinary = Convert.ToString(opcode, 2);
            opcodeBinary = opcodeBinary.PadLeft(6, '0');

            string IOAddrBinary = Convert.ToString(IOAddr, 2);
            IOAddrBinary = IOAddrBinary.PadLeft(3, '0');

            string r1AddrBinary = Convert.ToString(r1Addr, 2);
            r1AddrBinary = r1AddrBinary.PadLeft(3, '0');

            string r2AddrBinary = Convert.ToString(r2Addr, 2);
            r2AddrBinary = r2AddrBinary.PadLeft(3, '0');


            string instructionBinary = opcodeBinary + r1AddrBinary +  r2AddrBinary + IOAddrBinary +"000000000";



            byte[] bytes = InstructionBinaryToBytes(instructionBinary);

            return bytes;
        }

        private static byte[] InstructionBinaryToBytes(string instructionBinary)
        {
            byte[] bytes = new byte[3];

            bytes[0] = Convert.ToByte(instructionBinary.Substring(0, 8), 2);
            bytes[1] = Convert.ToByte(instructionBinary.Substring(8, 8), 2);
            bytes[2] = Convert.ToByte(instructionBinary.Substring(16, 8), 2);

            return bytes;
        }

        private static int ConvertAddress(string address)
        {
            address = address.Replace("[0X", "").Replace("]", "");

            if (address.Length > 3)
            {
                throw new Exception("Max address possible is 12 bits");
            }

            return int.Parse(address, System.Globalization.NumberStyles.HexNumber);
        }
    }
}
