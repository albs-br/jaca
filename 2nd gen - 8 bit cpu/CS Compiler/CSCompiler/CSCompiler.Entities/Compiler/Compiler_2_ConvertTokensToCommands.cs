using CSCompiler.Entities.CS;
using CSCompiler.Entities.CS.Tokens;
using CSCompiler.Entities.Enums;
using CSCompiler.Entities.MachineCode;
using CSCompiler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCompiler.Entities.Compiler
{
    public static partial class Compiler
    {
        /// <summary>
        /// Step 2 of 3 - Convert tokens to commands (Abstract Syntax Tree)
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public static CSProgram ConvertTokensToCommands(IList<Token> tokens)
        {
            var csProgram = new CSProgram();
            var currentProgramAddr = Constants.BASE_ADDR_PROGRAM;
            var currentVariableAddr = Constants.BASE_ADDR_VARIABLES;

            var bracesOpened = 0;
            Command parentCommand = null;

            var currentCommandTokens = new List<Token>();
            var lastToken = tokens.Last();
            foreach (var token in tokens)
            {
                currentCommandTokens.Add(token);

                if (bracesOpened == 0 && token is CloseBracesToken)
                {
                    throw new UnmatchingBracesException();
                }
                else
                if (bracesOpened > 0 && token is CloseBracesToken)
                {
                    parentCommand = null;

                    bracesOpened--;
                    currentCommandTokens.Clear();
                }
                else if (currentCommandTokens.Count == 7 &&
                    currentCommandTokens[0] is KeywordToken &&
                    currentCommandTokens[1] is OpenParenthesisToken &&
                    currentCommandTokens[2] is IdentifierToken &&
                    currentCommandTokens[3] is ComparisonToken &&
                    currentCommandTokens[4] is IdentifierToken &&
                    currentCommandTokens[5] is CloseParenthesisToken &&
                    currentCommandTokens[6] is OpenBracesToken &&
                    currentCommandTokens[0].Text == "if"
                    )
                {
                    // Test whether is a If Instruction

                    var variableLeftOperandName = currentCommandTokens[2].Text;
                    var variableRightOperandName = currentCommandTokens[4].Text;

                    Variable variableLeftOperand = GetVariableByName(csProgram, variableLeftOperandName);
                    Variable variableRightOperand = GetVariableByName(csProgram, variableRightOperandName);

                    var command = new IfInstruction();
                    command.ParentCommand = parentCommand;
                    command.CsProgram = csProgram;
                    command.Tokens = new List<Token>(currentCommandTokens);
                    command.BaseInstructionAddress = currentProgramAddr;

                    command.VariableLeftOperand = variableLeftOperand;
                    command.VariableRightOperand = variableRightOperand;


                    // add bytes of program
                    //var bytesOfCommand = command.MachineCode();
                    //currentProgramAddr = AddBytesOfCommand(machineCodeProgram, currentProgramAddr, bytesOfCommand);

                    parentCommand = command;
                    bracesOpened++;

                    csProgram.Commands.Add(command);
                    currentCommandTokens.Clear();
                }
                else if (token is SemicolonToken || token == lastToken)
                {
                    // Test whether is a Var Definition Instruction
                    if (VarDefinitionInstruction.CheckFormat(currentCommandTokens)) // TODO: make all checks like this one
                    {
                        var variableName = currentCommandTokens[1].Text;
                        var variableValue = currentCommandTokens[3].Text;


                        if (csProgram.Variables.Where(x => x.Name == variableName).Count() > 0)
                        {
                            throw new VariableAlreadyDefinedException(variableName);
                        }


                        if (int.Parse(variableValue) > 255)
                        {
                            throw new VariableOutsideOfRangeException(variableName);
                        }


                        var command = new VarDefinitionInstruction();
                        command.ParentCommand = parentCommand;
                        command.CsProgram = csProgram;
                        command.Tokens = new List<Token>(currentCommandTokens);

                        // add bytes of program
                        //var bytesOfCommand = command.MachineCode();
                        //currentProgramAddr = AddBytesOfCommand(machineCodeProgram, currentProgramAddr, bytesOfCommand);




                        var variable = new Variable();
                        variable.Name = variableName;
                        variable.Address = currentVariableAddr;
                        variable.VarType = EnumVarType.Byte;
                        csProgram.Variables.Add(variable);
                        currentVariableAddr++; // TODO: check type of var and increment it according to size of the type

                        command.Variable = variable;


                        //TODO: all instructions must update currentProgramAddr (or not)?
                        // Update currentProgramAddr 
                        currentProgramAddr += command.MachineCode().Count;


                        csProgram.Commands.Add(command);

                        // NO intruction change memory var area in compile-time
                        //machineCodeProgram.Bytes[this.GetNextVariableAddress()] = Convert.ToByte(variableValue);
                    }
                    // Test whether is an Atribution Instruction
                    else if (currentCommandTokens.Count == 4
                        && currentCommandTokens[0] is IdentifierToken
                        && currentCommandTokens[1] is EqualToken
                        && currentCommandTokens[2] is OperandToken
                        && currentCommandTokens[3] is SemicolonToken)
                    {
                        var variableDestinyName = currentCommandTokens[0].Text;





                        var variableDestiny = GetVariableByName(csProgram, variableDestinyName);


                        if (currentCommandTokens[2] is LiteralToken)
                        {
                            var literalValue = currentCommandTokens[2].Text;

                            if (int.Parse(literalValue) > 255)
                            {
                                throw new VariableOutsideOfRangeException(variableDestinyName);
                            }

                            var command = new AtributionFromLiteralInstruction();
                            command.ParentCommand = parentCommand;
                            command.CsProgram = csProgram;
                            command.Tokens = new List<Token>(currentCommandTokens);
                            command.VariableResult = variableDestiny;


                            // add bytes of program
                            //var bytesOfCommand = command.MachineCode();
                            //currentProgramAddr = AddBytesOfCommand(machineCodeProgram, currentProgramAddr, bytesOfCommand);

                            //TODO: extract method
                            if (parentCommand == null)
                            {
                                csProgram.Commands.Add(command);
                            }
                            else
                            {
                                ((ComplexCommand)parentCommand).InnerCommands.Add(command);
                            }

                            // Atribution intruction DON'T change memory var area!
                            //machineCodeProgram.Bytes[this.GetNextVariableAddress()] = Convert.ToByte(literalValue);
                        }
                        else if (currentCommandTokens[2] is IdentifierToken)
                        {
                            var variableSourceName = currentCommandTokens[2].Text;

                            var variableSource = GetVariableByName(csProgram, variableSourceName);

                            var command = new AtributionFromVarInstruction();
                            command.ParentCommand = parentCommand;
                            command.CsProgram = csProgram;
                            command.Tokens = new List<Token>(currentCommandTokens);
                            command.VariableSource = variableSource;
                            command.VariableDestiny = variableDestiny;


                            // add bytes of program
                            //var bytesOfCommand = command.MachineCode();
                            //currentProgramAddr = AddBytesOfCommand(machineCodeProgram, currentProgramAddr, bytesOfCommand);

                            csProgram.Commands.Add(command);

                            // Atribution intruction DON'T change memory var area!
                            //machineCodeProgram.Bytes[this.GetNextVariableAddress()] = Convert.ToByte(literalValue);
                        }
                    }
                    // Test whether is an Increment Instruction
                    else if (currentCommandTokens.Count == 4
                        && currentCommandTokens[0] is IdentifierToken
                        && currentCommandTokens[1] is ArithmeticSignalToken
                        && currentCommandTokens[2] is ArithmeticSignalToken
                        && currentCommandTokens[3] is SemicolonToken)
                    {
                        var variableName = currentCommandTokens[0].Text;
                        var arithmeticSignal1 = currentCommandTokens[1].Text;
                        var arithmeticSignal2 = currentCommandTokens[2].Text;

                        var variable = GetVariableByName(csProgram, variableName);

                        var command = new IncrementInstruction();
                        command.ParentCommand = parentCommand;
                        command.CsProgram = csProgram;
                        command.Tokens = new List<Token>(currentCommandTokens);
                        command.VariableOperand = variable;
                        command.BaseInstructionAddress = currentProgramAddr;

                        if (arithmeticSignal1 == "+" && arithmeticSignal2 == "+")
                        {
                            command.IncrementOperation = EnumIncrementOperation.Increment;
                        }
                        else if (arithmeticSignal1 == "-" && arithmeticSignal2 == "-")
                        {
                            command.IncrementOperation = EnumIncrementOperation.Decrement;
                        }


                        // add bytes of program
                        //var bytesOfCommand = command.MachineCode();
                        //currentProgramAddr = AddBytesOfCommand(machineCodeProgram, currentProgramAddr, bytesOfCommand);

                        csProgram.Commands.Add(command);
                    }
                    // Test whether is an Arithmetic Instruction
                    else if (currentCommandTokens.Count == 6
                        && currentCommandTokens[0] is IdentifierToken
                        && currentCommandTokens[1] is EqualToken
                        && currentCommandTokens[2] is OperandToken
                        && currentCommandTokens[3] is ArithmeticSignalToken
                        && currentCommandTokens[4] is OperandToken
                        && currentCommandTokens[5] is SemicolonToken)
                    {
                        var variableDestinyName = currentCommandTokens[0].Text;

                        var variableDestiny = GetVariableByName(csProgram, variableDestinyName);


                        if (currentCommandTokens[2] is IdentifierToken && currentCommandTokens[4] is LiteralToken)
                        {
                            //TODO:
                            //var literalValue = currentCommandTokens[2].Text;

                            //if (int.Parse(literalValue) > 255)
                            //{
                            //    throw new VariableOutsideOfRangeException(variableDestinyName);
                            //}

                            //var command = new AtributionFromLiteralInstruction();
                            //command.csProgram = this;
                            //command.Tokens = new List<Token>(currentCommandTokens);
                            //command.VariableResult = variableDestiny;


                            //// add bytes of program
                            //var bytesOfCommand = command.MachineCode();
                            //currentProgramAddr = AddBytesOfProgram(machineCodeProgram, currentProgramAddr, bytesOfCommand);

                            //this.Commands.Add(command);

                            //// Atribution intruction DON'T change memory var area!
                            ////machineCodeProgram.Bytes[this.GetNextVariableAddress()] = Convert.ToByte(literalValue);
                        }
                        else if (currentCommandTokens[2] is IdentifierToken && currentCommandTokens[4] is IdentifierToken)
                        {
                            var variableLeftOperandName = currentCommandTokens[2].Text;
                            var variableRightOperandName = currentCommandTokens[4].Text;
                            var arithmeticOperation = currentCommandTokens[3].Text;

                            var variableLeftOperand = GetVariableByName(csProgram, variableLeftOperandName);

                            var variableRightOperand = GetVariableByName(csProgram, variableRightOperandName);

                            var command = new ArithmeticInstruction();
                            command.ParentCommand = parentCommand;
                            command.CsProgram = csProgram;
                            command.Tokens = new List<Token>(currentCommandTokens);
                            command.VariableLeftOperand = variableLeftOperand;
                            command.VariableRightOperand = variableRightOperand;
                            command.VariableDestiny = variableDestiny;
                            switch (arithmeticOperation)
                            {
                                case "+":
                                    command.ArithmeticOperation = EnumArithmeticOperation.Addition;
                                    break;

                                case "-":
                                    command.ArithmeticOperation = EnumArithmeticOperation.Subtraction;
                                    break;

                                default:
                                    break;
                            }

                            // add bytes of program
                            //var bytesOfCommand = command.MachineCode();
                            //currentProgramAddr = AddBytesOfCommand(machineCodeProgram, currentProgramAddr, bytesOfCommand);

                            csProgram.Commands.Add(command);
                        }
                    }
                    // Test whether is an Command Instruction
                    else if (currentCommandTokens.Count == 7
                        && currentCommandTokens[0] is CommandToken
                        && currentCommandTokens[1] is OpenParenthesisToken
                        && currentCommandTokens[2] is LiteralToken
                        && currentCommandTokens[3] is CommaToken
                        && currentCommandTokens[4] is IdentifierToken
                        && currentCommandTokens[5] is CloseParenthesisToken
                        && currentCommandTokens[6] is SemicolonToken
                        )
                    {
                        var variableName = currentCommandTokens[4].Text;

                        var variable = GetVariableByName(csProgram, variableName);

                        var command = new CommandInstruction();
                        command.ParentCommand = parentCommand;
                        command.CsProgram = csProgram;
                        command.Tokens = new List<Token>(currentCommandTokens);
                        command.VariableOperand = variable;


                        // add bytes of program
                        //var bytesOfCommand = command.MachineCode();
                        //currentProgramAddr = AddBytesOfCommand(machineCodeProgram, currentProgramAddr, bytesOfCommand);

                        csProgram.Commands.Add(command);
                    }
                    else
                    {
                        IList<string> items = currentCommandTokens.Select(x => x.Text).ToList();
                        string instruction = items.Aggregate((i, j) => i + " " + j);

                        throw new InvalidInstructionFormatException(instruction);
                    }
                    currentCommandTokens.Clear();
                }
            }

            return csProgram;
        }

        private static Variable GetVariableByName(CSProgram csProgram, string variableLeftOperandName)
        {
            var variableLeftOperand = csProgram.Variables.Where(x => x.Name == variableLeftOperandName).FirstOrDefault();
            if (variableLeftOperand == null)
            {
                throw new UndefinedVariableException(variableLeftOperandName);
            }

            return variableLeftOperand;
        }

        //private static int AddBytesOfCommand(MachineCodeProgram machineCodeProgram, int currentProgramAddr, IList<byte> bytesOfCommand)
        //{
        //    var j = 0;
        //    for (var i = currentProgramAddr; i < currentProgramAddr + bytesOfCommand.Count; i++)
        //    {
        //        machineCodeProgram.Bytes[i] = bytesOfCommand[j++];
        //    }
        //    currentProgramAddr = currentProgramAddr + bytesOfCommand.Count;
        //    return currentProgramAddr;
        //}

        //private static int GetNextVariableAddress()
        //{
        //    // TODO: this should sum the sizes of each variable
        //    return Constants.BASE_ADDR_VARIABLES + this.Variables.Count - 1;
        //}
    }
}
