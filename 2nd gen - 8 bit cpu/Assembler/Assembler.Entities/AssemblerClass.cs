using Assembler.Entities.Enum;
using Assembler.Entities.Exceptions;
using Assembler.Entities.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Entities
{
    public static class AssemblerClass
    {
        public static AsmSource SourceToMachineCode(string source)
        {
            var asmSource = new AsmSource(source);

            AssemblerClass.ResolveIncludes(asmSource);
            AssemblerClass.ConvertToTokens(asmSource);
            AssemblerClass.ResolveLabelsAndDirectives(asmSource);
            AssemblerClass.ConvertTokensToMachineCode(asmSource);

            return asmSource;
        }

        public static void ResolveIncludes(AsmSource asmSource)
        {
            string[] lines = asmSource.Text.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.None
            );

            string newText = "";
            foreach (var line in lines)
            {
                if (line.Trim().ToLower().StartsWith("#include"))
                {
                    var fileName = line.Trim().Substring(8).Trim();

                    var linesIncludeFile = File.ReadAllLines(fileName);

                    foreach (var lineInclude in linesIncludeFile)
                    {
                        newText += lineInclude + Environment.NewLine;
                    }
                }
                else
                {
                    newText += line + Environment.NewLine;
                }
            }

            asmSource.Text = newText;
        }

        #region auxiliary code

        private enum StateMachine 
        { 
            None,
            CommandStarted,
            LiteralStarted,
            LabelStarted,
            CommentStarted,
            AddressStarted,
            DirectiveStarted
        }

        private static char[] irrelevantChars = { ' ', '\t' };

        private static bool IsRegister(string text)
        {
            if (text.Length == 1 && Constants.REGISTERS.Contains(text.ToUpper()))
            {
                return true;
            }

            return false;
        }

        private static bool IsHexaDigit(char ch)
        {
            char[] hexaAlfaDigits = { 'a', 'b', 'c', 'd', 'e', 'f' };

            ch = ch.ToString().ToLower().ElementAt(0);

            if (Char.IsDigit(ch) || hexaAlfaDigits.Contains(ch))
            {
                return true;
            }

            return false;
        }

        private static bool IsBinaryDigit(char ch)
        {
            char[] binaryDigits = { '0', '1' };

            if (binaryDigits.Contains(ch))
            {
                return true;
            }

            return false;
        }

        private static bool IsValidIdentifierChar(char ch)
        {
            if (Char.IsLetterOrDigit(ch) || ch == '_')
            {
                return true;
            }

            return false;
        }

        #endregion

        public static void ConvertToTokens(AsmSource asmSource)
        {
            var lines = asmSource.Text.Split(
                    new[] { Environment.NewLine },
                    StringSplitOptions.None
                );


            foreach (var line in lines)
            {
                Line newLine = ConvertLineToTokens(line);

                if (newLine.Tokens.Count > 0)
                {
                    asmSource.Lines.Add(newLine);
                }
            }
        }

        public static Line ConvertLineToTokens(string line)
        {
            var state = StateMachine.None;

            var newLine = new Line(line);
            var currentToken = "";

            for (var counter = 0; counter < line.Length; counter++)
            {
                bool isLastChar = (counter == line.Length - 1);
                char currentChar = line[counter];

                switch (state)
                {
                    case StateMachine.None:
                        if (irrelevantChars.Contains(currentChar))
                        {
                            continue;
                        }
                        else if (currentChar == '/')
                        {
                            if (currentToken == "/")
                            {
                                state = StateMachine.CommentStarted;
                            }
                            else
                            {
                                currentToken = "/";
                            }
                        }
                        else if (Char.IsLetter(currentChar))
                        {
                            state = StateMachine.CommandStarted;
                            currentToken = currentChar.ToString();
                        }
                        else if (Char.IsDigit(currentChar))
                        {
                            state = StateMachine.LiteralStarted;
                            currentToken = currentChar.ToString();
                        }
                        else if (currentChar == ':')
                        {
                            state = StateMachine.LabelStarted;
                        }
                        else if (currentChar == '[')
                        {
                            state = StateMachine.AddressStarted;
                        }
                        else if (currentChar == ',')
                        {
                            newLine.Tokens.Add(new CommaToken());
                        }
                        else if (currentChar == '#')
                        {
                            state = StateMachine.DirectiveStarted;
                        }
                        break;

                    case StateMachine.CommentStarted:
                        {
                            continue;
                        }

                    case StateMachine.CommandStarted:

                        if (IsValidIdentifierChar(currentChar))
                        {
                            currentToken += currentChar;
                        }
                        else if (irrelevantChars.Contains(currentChar)
                            || currentChar == ',')
                        {
                            state = StateMachine.None;

                            if (IsRegister(currentToken))
                            {
                                newLine.Tokens.Add(new RegisterToken(currentToken));
                            }
                            else
                            {
                                newLine.Tokens.Add(new CommandToken(currentToken));
                            }

                            if (currentChar == ',')
                            {
                                newLine.Tokens.Add(new CommaToken());
                            }

                            currentToken = "";
                        }
                        else if (currentChar == ':')
                        {
                            state = StateMachine.None;
                            newLine.Tokens.Add(new LabelToken(currentToken));
                            currentToken = "";
                        }
                        break;

                    case StateMachine.LabelStarted:

                        if (IsValidIdentifierChar(currentChar))
                        {
                            currentToken += currentChar;
                        }
                        else if (irrelevantChars.Contains(currentChar))
                        {
                            state = StateMachine.None;
                            newLine.Tokens.Add(new LabelToken(currentToken));
                            currentToken = "";
                        }
                        break;

                    case StateMachine.DirectiveStarted:

                        if (IsValidIdentifierChar(currentChar))
                        {
                            currentToken += currentChar;
                        }
                        else if (irrelevantChars.Contains(currentChar) || currentChar == ',')
                        {
                            state = StateMachine.None;
                            newLine.Tokens.Add(new DirectiveToken(currentToken));
                            currentToken = "";

                            if (currentChar == ',')
                            {
                                newLine.Tokens.Add(new CommaToken());
                            }
                        }

                        break;

                    case StateMachine.LiteralStarted:

                        var isValidLiteral = CheckLiteral(ref currentToken, currentChar);

                        if (!isValidLiteral)
                        {
                            if (irrelevantChars.Contains(currentChar) || currentChar == ',')
                            {
                                state = StateMachine.None;
                                newLine.Tokens.Add(new LiteralToken(currentToken));
                                currentToken = "";

                                if (currentChar == ',')
                                {
                                    newLine.Tokens.Add(new CommaToken());
                                }
                            }
                            else
                            {
                                throw new InvalidCommandLineException(line);
                            }
                        }
                        break;

                    case StateMachine.AddressStarted:

                        var isValidLiteralAddress = CheckLiteral(ref currentToken, currentChar);

                        if (!isValidLiteralAddress)
                        {
                            if ((currentChar.ToString().ToUpper() == "H" && currentToken == "") ||
                               (currentChar.ToString().ToUpper() == "L" && currentToken.ToUpper() == "H"))
                            {
                                currentToken += currentChar;
                            }
                            else if (currentChar == ']')
                            {
                                if (currentToken.ToUpper() == "HL")
                                {
                                    newLine.Tokens.Add(new IndirectByRegisterToken(currentToken));
                                }
                                else
                                {
                                    newLine.Tokens.Add(new AddressToken(currentToken));
                                }

                                state = StateMachine.None;
                                currentToken = "";
                            }
                            else
                            {
                                throw new InvalidCommandLineException(line);
                            }
                        }
                        break;

                    default:
                        break;
                }

                if (isLastChar)
                {
                    switch (state)
                    {
                        case StateMachine.None:
                            break;

                        case StateMachine.CommandStarted:
                            if (IsRegister(currentToken))
                            {
                                newLine.Tokens.Add(new RegisterToken(currentToken));
                            }
                            else
                            {
                                newLine.Tokens.Add(new CommandToken(currentToken));
                            }
                            break;

                        case StateMachine.LiteralStarted:
                            newLine.Tokens.Add(new LiteralToken(currentToken));
                            break;

                        case StateMachine.LabelStarted:
                            newLine.Tokens.Add(new LabelToken(currentToken));
                            break;

                        case StateMachine.DirectiveStarted:
                            newLine.Tokens.Add(new DirectiveToken(currentToken));
                            break;

                        case StateMachine.CommentStarted:
                            // do nothing (commands at the end of line are ignored)
                            break;

                        default:
                            break;
                    }
                }
            }

            return newLine;
        }

        private static bool CheckLiteral(ref string currentToken, char currentChar)
        {
            // Check second char
            if (
                (Char.IsDigit(currentChar) &&
                 currentToken.Length == 1 && Char.IsDigit(currentToken.ElementAt(0))) ||

                (currentToken + currentChar).ToLower() == "0x" ||
                
                (currentToken + currentChar).ToLower() == "0b")
            {
                currentToken += currentChar;
                return true;
            }

            // Third char onwards
            else if (currentToken.ToLower().StartsWith("0x") &&
                IsHexaDigit(currentChar))
            {
                currentToken += currentChar;
                return true;
            }
            else if (currentToken.ToLower().StartsWith("0b") &&
                IsBinaryDigit(currentChar))
            {
                currentToken += currentChar;
                return true;
            }
            else if (
                !currentToken.ToLower().StartsWith("0x") &&
                !currentToken.ToLower().StartsWith("0b") &&
                Char.IsDigit(currentChar))
            {
                currentToken += currentChar;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Identifies and resolves Labels and Variables
        /// </summary>
        /// <param name="asmSource"></param>
        public static void ResolveLabelsAndDirectives(AsmSource asmSource)
        {
            var programAddress = 0;
            var varAddress = 0xc00;
            var defMemLastAddress = 0;

            foreach (var line in asmSource.Lines)
            {
                if (line.IsLabelDefinition())
                {
                    asmSource.Labels.Add(new KeyValuePair<string, int>(
                        line.Tokens[0].Text,
                        programAddress));
                }
                else if (line.IsVariableDefinition())
                {
                    asmSource.Variables.Add(new KeyValuePair<string, int>(
                        line.Tokens[1].Text,
                        varAddress));

                    varAddress++;
                }
                else if (line.IsOrgDirective())
                {
                    programAddress = ((LiteralToken)line.Tokens[1]).NumericValue;
                }
                else if (line.IsDefMemDirective())
                {
                    var address = 0;
                    var value = 0;
                    if (line.Tokens.Count == 3)
                    {
                        address = ((LiteralToken)line.Tokens[1]).NumericValue;
                        defMemLastAddress = address;

                        value = ((LiteralToken)line.Tokens[2]).NumericValue;
                    }
                    else if (line.Tokens.Count == 2)
                    {
                        defMemLastAddress++;
                        address = defMemLastAddress;

                        value = ((LiteralToken)line.Tokens[1]).NumericValue;
                    }


                    asmSource.DefMems.Add(new KeyValuePair<int, int>(
                        address,
                        value));
                }
                else
                {
                    line.Address = programAddress;
                    programAddress += 3;
                }
            }

            IList<Line> newLines = new List<Line>();
            foreach (var line in asmSource.Lines)
            {
                try
                {
                    // if is label definition or defbyte, drop line
                    if (line.IsLabelDefinition() ||
                       line.IsVariableDefinition() ||
                       line.IsOrgDirective() ||
                       line.IsDefMemDirective())
                    {
                        continue;
                    }

                    // if has label or variable in line, resolve it 
                    // (convert to corresponding memory address)
                    IList<Token> newTokens = new List<Token>();
                    foreach (var token in line.Tokens)
                    {
                        if (token is LabelToken)
                        {
                            var literal = asmSource.Labels[token.Text].ToString();
                            var literalToken = new LiteralToken(literal);

                            newTokens.Add(literalToken);
                        }
                        else if (token is DirectiveToken)
                        {
                            var address = asmSource.Variables[token.Text].ToString();
                            var addressToken = new AddressToken(address);

                            newTokens.Add(addressToken);
                        }
                        else
                        {
                            newTokens.Add(token);
                        }
                    }
                    line.Tokens.Clear();
                    line.Tokens = newTokens;

                    newLines.Add(line);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error in line: " + line.Text, ex);
                }
            }
            asmSource.Lines.Clear();
            asmSource.Lines = newLines;
        }

        public static void ConvertTokensToMachineCode(AsmSource asmSource)
        {
            var programAddressCounter = 0;
            foreach (var line in asmSource.Lines)
            {
                // Fill bytes with zeroes when there is a gap between 
                // instructions (caused by an #org directive)
                var start = programAddressCounter;
                for (var i = start; i < line.Address; i++)
                {
                    asmSource.Bytes.Add(0);
                    programAddressCounter++;
                }

                asmSource.Bytes.AddRange(ConvertLineTokensToMachineCode(line));
                programAddressCounter += 3;
            }

            var lastAddr = programAddressCounter;
            foreach (var defMem in asmSource.DefMems)
            {
                var address = defMem.Key;
                var value = defMem.Value;

                // Fill space between end of program (or last defmem addr)
                // and defmem addr
                for (int i = lastAddr; i < address; i++)
                {
                    asmSource.Bytes.Add(0);
                    lastAddr++;
                }

                asmSource.Bytes.Add(Convert.ToByte(value));
                lastAddr++;
            }
        }

        public static byte[] ConvertLineTokensToMachineCode(Line line)
        {
            if (line.Tokens.Count == 4)
            {
                if (line.Tokens[0] is CommandToken &&
                    line.Tokens[1] is RegisterToken &&
                    line.Tokens[2] is CommaToken &&
                    line.Tokens[3] is LiteralToken &&
                    line.Tokens[0].Text.ToUpper() == "LD")
                {
                    var opcode = 1;
                    var r1Addr = RegisterNameToAddress(
                        line.Tokens[1].Text,
                        EnumRegistersAllowed.All
                        );
                    var data = ((LiteralToken)line.Tokens[3]).NumericValue;

                    return ImediateInstruction(opcode, r1Addr, data);
                }

                else if (line.Tokens[0] is CommandToken &&
                    line.Tokens[1] is RegisterToken &&
                    line.Tokens[2] is CommaToken &&
                    line.Tokens[3] is RegisterToken &&
                    line.Tokens[0].Text.ToUpper() == "LD")
                {
                    var opcode = 2;
                    var r1Addr = RegisterNameToAddress(
                        line.Tokens[1].Text,
                        EnumRegistersAllowed.All
                        );
                    var r2Addr = RegisterNameToAddress(
                        line.Tokens[3].Text,
                        EnumRegistersAllowed.All
                        );

                    return ByRegisterInstruction(opcode, r1Addr, r2Addr);
                }

                else if (line.Tokens[0] is CommandToken &&
                    line.Tokens[1] is RegisterToken &&
                    line.Tokens[2] is CommaToken &&
                    line.Tokens[3] is AddressToken &&
                    line.Tokens[0].Text.ToUpper() == "LD")
                {
                    var opcode = 3;
                    var r1Addr = RegisterNameToAddress(
                        line.Tokens[1].Text,
                        EnumRegistersAllowed.All
                        );
                    var address = ((AddressToken)line.Tokens[3]).NumericValue;

                    return DirectInstruction(opcode, r1Addr, address);
                }

                else if (line.Tokens[0] is CommandToken &&
                    line.Tokens[1] is RegisterToken &&
                    line.Tokens[2] is CommaToken &&
                    line.Tokens[3] is IndirectByRegisterToken &&
                    line.Tokens[0].Text.ToUpper() == "LD" &&
                    line.Tokens[3].Text.ToUpper() == "HL")
                {
                    var opcode = 4;
                    var r1Addr = RegisterNameToAddress(
                        line.Tokens[1].Text,
                        EnumRegistersAllowed.All
                        );

                    return IndirectByRegisterInstruction(opcode, r1Addr);
                }

                else if (line.Tokens[0] is CommandToken &&
                    line.Tokens[1] is RegisterToken &&
                    line.Tokens[2] is CommaToken &&
                    line.Tokens[3] is RegisterToken &&
                    Constants.ALU_INSTRUCTIONS.Contains(line.Tokens[0].Text.ToUpper()) &&
                    !Constants.SINGLE_OPERAND_ALU_INSTRUCTIONS.Contains(line.Tokens[0].Text.ToUpper()))
                {
                    var opcode = OpcodeOfAluInstruction(line.Tokens[0].Text);
                    var r1Addr = RegisterNameToAddress(
                        line.Tokens[1].Text,
                        EnumRegistersAllowed.BankA
                        );
                    var r2Addr = RegisterNameToAddress(
                        line.Tokens[3].Text,
                        EnumRegistersAllowed.BankB
                        );

                    return ByRegisterInstruction(opcode, r1Addr, r2Addr);
                }

                else if (line.Tokens[0] is CommandToken &&
                    line.Tokens[1] is CommandToken &&
                    line.Tokens[2] is CommaToken &&
                    line.Tokens[3] is LiteralToken &&
                    (line.Tokens[0].Text.ToUpper() == "JP" ||
                     line.Tokens[0].Text.ToUpper() == "CALL") &&
                    line.Tokens[1].Text.ToUpper() == "Z")
                {
                    var opcode = 0;
                    switch (line.Tokens[0].Text.ToUpper())
                    {
                        case "JP":
                            opcode = 6;
                            break;
                        case "CALL":
                            opcode = 8;
                            break;
                        default:
                            break;
                    }

                    var r1Addr = 0;
                    var address = ((LiteralToken)line.Tokens[3]).NumericValue;

                    return DirectInstruction(opcode, r1Addr, address);
                }

                // TODO: 'C' here is not a register! It's working, but very ugly
                else if (line.Tokens[0] is CommandToken &&
                    line.Tokens[1] is RegisterToken &&
                    line.Tokens[2] is CommaToken &&
                    line.Tokens[3] is LiteralToken &&
                    (line.Tokens[0].Text.ToUpper() == "JP" ||
                     line.Tokens[0].Text.ToUpper() == "CALL") &&
                    line.Tokens[1].Text.ToUpper() == "C")
                {
                    var opcode = 0;
                    switch (line.Tokens[0].Text.ToUpper())
                    {
                        case "JP":
                            opcode = 12;
                            break;
                        case "CALL":
                            opcode = 13;
                            break;
                        default:
                            break;
                    }

                    var r1Addr = 0;
                    var address = ((LiteralToken)line.Tokens[3]).NumericValue;

                    return DirectInstruction(opcode, r1Addr, address);
                }

                else if (line.Tokens[0] is CommandToken &&
                    line.Tokens[1] is AddressToken &&
                    line.Tokens[2] is CommaToken &&
                    line.Tokens[3] is RegisterToken &&
                    line.Tokens[0].Text.ToUpper() == "ST")
                {
                    var opcode = 10;
                    var address = ((AddressToken)line.Tokens[1]).NumericValue;
                    var r1Addr = RegisterNameToAddress(
                        line.Tokens[3].Text,
                        EnumRegistersAllowed.All
                        );

                    return DirectInstruction(opcode, r1Addr, address);
                }

                else if (line.Tokens[0] is CommandToken &&
                    line.Tokens[1] is IndirectByRegisterToken &&
                    line.Tokens[2] is CommaToken &&
                    line.Tokens[3] is RegisterToken &&
                    line.Tokens[0].Text.ToUpper() == "ST" &&
                    line.Tokens[1].Text.ToUpper() == "HL")
                {
                    var opcode = 11;
                    var r1Addr = RegisterNameToAddress(
                        line.Tokens[3].Text,
                        EnumRegistersAllowed.All
                        );

                    return IndirectByRegisterInstruction(opcode, r1Addr);
                }

                else if (line.Tokens[0] is CommandToken &&
                    line.Tokens[1] is LiteralToken &&
                    line.Tokens[2] is CommaToken &&
                    line.Tokens[3] is RegisterToken &&
                    (line.Tokens[0].Text.ToUpper() == "IN" ||
                     line.Tokens[0].Text.ToUpper() == "OUT"))
                {
                    var opcode = 0;
                    switch (line.Tokens[0].Text.ToUpper())
                    {
                        case "IN":
                            opcode = 16;
                            break;
                        case "OUT":
                            opcode = 17;
                            break;
                        default:
                            break;
                    }

                    var ioAddr = ((LiteralToken)line.Tokens[1]).NumericValue;
                    var r1Addr = RegisterNameToAddress(
                        line.Tokens[3].Text,
                        EnumRegistersAllowed.BankA
                        );

                    return IOInstruction(opcode, ioAddr, r1Addr);
                }

            }
            else if (line.Tokens.Count == 2)
            {
                if (line.Tokens[0] is CommandToken &&
                    line.Tokens[1] is RegisterToken &&
                    Constants.SINGLE_OPERAND_ALU_INSTRUCTIONS.Contains(line.Tokens[0].Text.ToUpper()))
                {
                    var opcode = OpcodeOfAluInstruction(line.Tokens[0].Text);
                    var r1Addr = RegisterNameToAddress(
                        line.Tokens[1].Text,
                        EnumRegistersAllowed.BankA
                        );

                    return ByRegisterInstruction(opcode, r1Addr);
                }

                else if (line.Tokens[0] is CommandToken &&
                    (line.Tokens[0].Text.ToUpper() == "JP" ||
                     line.Tokens[0].Text.ToUpper() == "CALL") &&
                    line.Tokens[1] is LiteralToken)
                {
                    var opcode = 0;
                    switch (line.Tokens[0].Text.ToUpper())
                    {
                        case "JP":
                            opcode = 5;
                            break;
                        case "CALL":
                            opcode = 7;
                            break;
                        default:
                            break;
                    }

                    var r1Addr = 0; // not used
                    var address = ((LiteralToken)line.Tokens[1]).NumericValue;

                    return DirectInstruction(opcode, r1Addr, address);
                }
            }

            else if (line.Tokens.Count == 1)
            {
                if (line.Tokens[0] is CommandToken &&
                    line.Tokens[0].Text.ToUpper() == "RET")
                {
                    var opcode = 9;

                    return ByRegisterInstruction(opcode);
                }
            }

            else if (line.Tokens.Count == 6)
            {
                if (line.Tokens[0] is CommandToken &&
                    line.Tokens[1] is LiteralToken &&
                    line.Tokens[2] is CommaToken &&
                    line.Tokens[3] is RegisterToken &&
                    line.Tokens[4] is CommaToken &&
                    line.Tokens[5] is RegisterToken &&
                    (line.Tokens[0].Text.ToUpper() == "IN" ||
                     line.Tokens[0].Text.ToUpper() == "OUT"))
                {
                    var opcode = 0;
                    switch (line.Tokens[0].Text.ToUpper())
                    {
                        case "IN":
                            opcode = 16;
                            break;
                        case "OUT":
                            opcode = 17;
                            break;
                        default:
                            break;
                    }

                    var ioAddr = ((LiteralToken)line.Tokens[1]).NumericValue;
                    var r1Addr = RegisterNameToAddress(
                        line.Tokens[3].Text,
                        EnumRegistersAllowed.BankA
                        );
                    var r2Addr = RegisterNameToAddress(
                        line.Tokens[5].Text,
                        EnumRegistersAllowed.BankB
                        );

                    return IOInstruction(opcode, ioAddr, r1Addr, r2Addr);
                }

            }

            throw new InvalidCommandLineException("Invalid line: " + line.Text); ;
        }

        #region ConvertTokensToMachineCode auxiliary methods

        private static int RegisterNameToAddress(string registerName, EnumRegistersAllowed registersAllowed)
        {
            registerName = registerName.ToUpper();

            var errorMsg = "Register {0} not allowed. Allowed registers are {1}";

            if (registersAllowed == EnumRegistersAllowed.BankA && !Constants.REGISTERS_BANK_A.Contains(registerName))
            {
                var msg = string.Format(errorMsg,
                    registerName,
                    string.Join(", ", Constants.REGISTERS_BANK_A)
                    );

                throw new InvalidRegisterException(msg);
            }

            if (registersAllowed == EnumRegistersAllowed.BankB && !Constants.REGISTERS_BANK_B.Contains(registerName))
            {
                var msg = string.Format(errorMsg,
                    registerName,
                    string.Join(", ", Constants.REGISTERS_BANK_B)
                    );

                throw new InvalidRegisterException(msg);
            }

            if (Constants.REGISTERS.Contains(registerName))
            {
                return Convert.ToByte(Array.IndexOf(Constants.REGISTERS, registerName));
            }
            else
            {
                throw new InvalidCommandLineException("Invalid register name: " + registerName);
            }
        }
        
        private static byte OpcodeOfAluInstruction(string aluInstruction)
        {
            aluInstruction = aluInstruction.ToUpper();

            if (Constants.ALU_INSTRUCTIONS.Contains(aluInstruction))
            {
                return Convert.ToByte(Array.IndexOf(Constants.ALU_INSTRUCTIONS, aluInstruction) + 32);
            }
            else
            {
                throw new Exception("Invalid ALU instruction");
            }
        }

        private static byte[] ImediateInstruction(int opcode, int r1Addr, int data)
        {
            string opcodeBinary = Convert.ToString(opcode, 2);
            opcodeBinary = opcodeBinary.PadLeft(6, '0');

            string r1AddrBinary = Convert.ToString(r1Addr, 2);
            r1AddrBinary = r1AddrBinary.PadLeft(3, '0');

            string dataBinary = Convert.ToString(data, 2);
            dataBinary = dataBinary.PadLeft(8, '0');



            string instructionBinary = opcodeBinary + r1AddrBinary + "0000000" + dataBinary;



            byte[] bytes = InstructionBinaryToBytes(instructionBinary);

            return bytes;
        }

        private static byte[] ByRegisterInstruction(int opcode, int r1Addr = 0, int r2Addr = 0)
        {
            string opcodeBinary = Convert.ToString(opcode, 2);
            opcodeBinary = opcodeBinary.PadLeft(6, '0');

            string r1AddrBinary = Convert.ToString(r1Addr, 2);
            r1AddrBinary = r1AddrBinary.PadLeft(3, '0');

            string r2AddrBinary = Convert.ToString(r2Addr, 2);
            r2AddrBinary = r2AddrBinary.PadLeft(3, '0');



            string instructionBinary = opcodeBinary + r1AddrBinary + r2AddrBinary + "000000000000";



            byte[] bytes = InstructionBinaryToBytes(instructionBinary);

            return bytes;
        }

        private static byte[] DirectInstruction(int opcode, int r1Addr, int addr)
        {
            string opcodeBinary = Convert.ToString(opcode, 2);
            opcodeBinary = opcodeBinary.PadLeft(6, '0');

            string r1AddrBinary = Convert.ToString(r1Addr, 2);
            r1AddrBinary = r1AddrBinary.PadLeft(3, '0');

            string addrBinary = Convert.ToString(addr, 2);
            addrBinary = addrBinary.PadLeft(12, '0');



            string instructionBinary = opcodeBinary + r1AddrBinary + "000" + addrBinary;



            byte[] bytes = InstructionBinaryToBytes(instructionBinary);

            return bytes;
        }

        private static byte[] IndirectByRegisterInstruction(int opcode, int r1Addr)
        {
            string opcodeBinary = Convert.ToString(opcode, 2);
            opcodeBinary = opcodeBinary.PadLeft(6, '0');

            string r1AddrBinary = Convert.ToString(r1Addr, 2);
            r1AddrBinary = r1AddrBinary.PadLeft(3, '0');



            string instructionBinary = opcodeBinary + r1AddrBinary + "000000000000000";



            byte[] bytes = InstructionBinaryToBytes(instructionBinary);

            return bytes;
        }

        private static byte[] IOInstruction(int opcode, int IOAddr, int r1Addr, int r2Addr = 0)
        {
            string opcodeBinary = Convert.ToString(opcode, 2);
            opcodeBinary = opcodeBinary.PadLeft(6, '0');

            string IOAddrBinary = Convert.ToString(IOAddr, 2);
            IOAddrBinary = IOAddrBinary.PadLeft(3, '0');

            string r1AddrBinary = Convert.ToString(r1Addr, 2);
            r1AddrBinary = r1AddrBinary.PadLeft(3, '0');

            string r2AddrBinary = Convert.ToString(r2Addr, 2);
            r2AddrBinary = r2AddrBinary.PadLeft(3, '0');


            string instructionBinary = opcodeBinary + r1AddrBinary + r2AddrBinary + IOAddrBinary + "000000000";



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

        #endregion
    }
}
