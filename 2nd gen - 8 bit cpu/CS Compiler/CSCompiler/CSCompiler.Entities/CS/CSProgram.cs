using CSCompiler.Entities.CS.Tokens;
using CSCompiler.Entities.MachineCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCompiler.Entities;
using CSCompiler.Exceptions;

namespace CSCompiler.Entities.CS
{
    public class CSProgram
    {
        // Constructor
        public CSProgram()
        {
            this.Commands = new List<Command>();
            this.Variables = new List<Variable>();
        }

        public string SourceCodeText;

        public IList<Command> Commands { get; set; }
        public IList<Variable> Variables { get; set; }

        // State Machine
        private enum States
        {
            None,
            TypeOrIdentifierTokenStarted,
            LiteralTokenStarted,
            //TokenEnd,
        }

        public IList<Token> ConvertSourceToTokens()
        {
            States currentState = States.None;

            var tokens = new List<Token>();
            var currentToken = "";
            for (int i = 0; i < this.SourceCodeText.Length; i++)
            {
                char currentChar = SourceCodeText.ElementAt(i);

                switch (currentState)
                {
                    case States.None:
                        if (Constants.IsIrrelevantChar(currentChar))
                        {
                            continue;
                        }
                        else
                        {
                            if (Char.IsLetter(currentChar))
                            {
                                currentState = States.TypeOrIdentifierTokenStarted;
                                currentToken = currentChar.ToString();
                            }
                            else if (Char.IsNumber(currentChar))
                            {
                                currentState = States.LiteralTokenStarted;
                                currentToken = currentChar.ToString();
                            }
                            else if (currentChar == '=')
                            {
                                tokens.Add(new EqualToken(currentChar.ToString()));
                                currentToken = "";
                            }
                            else if (Constants.IsArithmeticSignal(currentChar))
                            {
                                tokens.Add(new ArithmeticSignalToken(currentChar.ToString()));
                                currentToken = "";
                            }
                            else if (currentChar == ';')
                            {
                                tokens.Add(new SemicolonToken(currentChar.ToString()));
                                currentToken = "";
                            }
                        }
                        break;

                    case States.TypeOrIdentifierTokenStarted:
                        if (Char.IsLetterOrDigit(currentChar))
                        {
                            // Identifier continues
                            currentToken += currentChar;
                        }
                        else
                        {
                            // Identifier ends
                            if (Constants.IsValidType(currentToken))
                            {
                                tokens.Add(new TypeToken(currentToken));
                            }
                            else
                            {
                                tokens.Add(new IdentifierToken(currentToken));

                                if (currentChar == '=')
                                {
                                    tokens.Add(new EqualToken(currentChar.ToString()));
                                    //currentToken = "";
                                }
                                else if (Constants.IsArithmeticSignal(currentChar))
                                {
                                    tokens.Add(new ArithmeticSignalToken(currentChar.ToString()));
                                    //currentToken = "";
                                }
                                if (currentChar == ';')
                                {
                                    tokens.Add(new SemicolonToken(currentChar.ToString()));
                                    //currentToken = "";
                                }
                            }

                            currentToken = currentChar.ToString();
                            currentState = States.None;
                        }
                        break;

                    case States.LiteralTokenStarted:
                        if (Char.IsNumber(currentChar))
                        {
                            currentToken += currentChar;
                        }
                        else
                        {
                            tokens.Add(new LiteralToken(currentToken));

                            if (currentChar == ';')
                            {
                                tokens.Add(new SemicolonToken(currentChar.ToString()));
                                //currentToken = "";
                            }
                            else if (Constants.IsArithmeticSignal(currentChar))
                            {
                                tokens.Add(new ArithmeticSignalToken(currentChar.ToString()));
                                //currentToken = "";
                            }

                            currentToken = "";
                            currentState = States.None;
                        }
                        break;
                }
            }

            return tokens;
        }

        public MachineCodeProgram ConvertTokensToMachineCode(IList<Token> tokens)
        {
            var machineCodeProgram = new MachineCodeProgram();
            var currentProgramAddr = Constants.BASE_ADDR_PROGRAM;
            var currentVariableAddr = Constants.BASE_ADDR_VARIABLES;

            var currentCommandTokens = new List<Token>();
            foreach (var token in tokens)
            {
                currentCommandTokens.Add(token);
                if (token is SemicolonToken)
                {
                    // Test whether is a Var Definition Instruction
                    if (currentCommandTokens.Count == 5
                        && currentCommandTokens[0] is TypeToken
                        && currentCommandTokens[1] is IdentifierToken
                        && currentCommandTokens[2] is EqualToken
                        && currentCommandTokens[3] is LiteralToken
                        && currentCommandTokens[4] is SemicolonToken)
                    {
                        var variableName = currentCommandTokens[1].Text;
                        var variableValue = currentCommandTokens[3].Text;


                        if (this.Variables.Where(x => x.Name == variableName).Count() > 0)
                        {
                            throw new VariableAlreadyDefinedException(variableName);
                        }


                        if (int.Parse(variableValue) > 255)
                        {
                            throw new VariableOutsideOfRangeException(variableName);
                        }


                        var command = new VarDefinitionInstruction();
                        command.csProgram = this;
                        command.Tokens = currentCommandTokens;

                        // add bytes of program
                        var bytesOfCommand = command.MachineCode();
                        currentProgramAddr = AddBytesOfProgram(machineCodeProgram, currentProgramAddr, bytesOfCommand);



                        var variable = new Variable();
                        variable.Name = variableName;
                        variable.Address = currentVariableAddr;
                        this.Variables.Add(variable);
                        currentVariableAddr++; // TODO: check type of var and increment it according to size of the type



                        this.Commands.Add(command);

                        machineCodeProgram.Bytes[this.GetNextVariableAddress()] = Convert.ToByte(variableValue);
                    }
                    // Test whether is a Atribution Instru0tion
                    else if (currentCommandTokens.Count == 4
                        && currentCommandTokens[0] is IdentifierToken
                        && currentCommandTokens[1] is EqualToken
                        && currentCommandTokens[2] is LiteralToken
                        && currentCommandTokens[3] is SemicolonToken)
                    {
                        var variableName = currentCommandTokens[0].Text;
                        var variableValue = currentCommandTokens[2].Text;





                        var variable = this.Variables.Where(x => x.Name == variableName).FirstOrDefault();
                        if (variable == null)
                        {
                            throw new UndefinedVariableException(variableName);
                        }


                        if (int.Parse(variableValue) > 255)
                        {
                            throw new VariableOutsideOfRangeException(variableName);
                        }

                        var command = new AtributionInstruction();
                        command.csProgram = this;
                        command.Tokens = currentCommandTokens;
                        command.VariableResult = variable;


                        // add bytes of program
                        var bytesOfCommand = command.MachineCode();
                        currentProgramAddr = AddBytesOfProgram(machineCodeProgram, currentProgramAddr, bytesOfCommand);

                        this.Commands.Add(command);

                        machineCodeProgram.Bytes[this.GetNextVariableAddress()] = Convert.ToByte(variableValue);
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

            return machineCodeProgram;
        }

        private static int AddBytesOfProgram(MachineCodeProgram machineCodeProgram, int currentProgramAddr, IList<byte> bytesOfCommand)
        {
            var j = 0;
            for (var i = currentProgramAddr; i < currentProgramAddr + bytesOfCommand.Count; i++)
            {
                machineCodeProgram.Bytes[i] = bytesOfCommand[j++];
            }
            currentProgramAddr = currentProgramAddr + bytesOfCommand.Count;
            return currentProgramAddr;
        }

        private int GetNextVariableAddress()
        {
            // TODO: this should sum the sizes of each variable
            return Constants.BASE_ADDR_VARIABLES + this.Variables.Count - 1;
        }

    }
}
