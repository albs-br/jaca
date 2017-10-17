using CSCompiler.Entities.CS.Tokens;
using CSCompiler.Entities.MachineCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCompiler.Entities;

namespace CSCompiler.Entities.CS
{
    public class CSProgram
    {
        public string SourceCodeText;

        public IList<Command> Commands { get; set; }

        // State Machine
        private enum States
        {
            None,
            IdentifierTokenStarted,
            NumberTokenStarted,
            //TokenEnd,
        }

        public IList<Token> ConvertToTokens()
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
                        if (currentChar == ' ')
                        {
                            continue;
                        }
                        else
                        {
                            if (Char.IsLetter(currentChar))
                            {
                                currentToken += currentChar;
                                currentState = States.IdentifierTokenStarted;
                            }
                            else if (Char.IsNumber(currentChar))
                            {
                                currentToken += currentChar;
                                currentState = States.NumberTokenStarted;
                            }
                            else if (currentChar == '=')
                            {
                                currentToken = "";
                                tokens.Add(new EqualToken { Text = currentToken });
                            }
                            else if (currentChar == ';')
                            {
                                currentToken = "";
                                tokens.Add(new SemicolonToken { Text = currentToken });
                            }
                        }
                        break;

                    case States.IdentifierTokenStarted:
                        if (Char.IsLetterOrDigit(currentChar))
                        {
                            currentToken += currentChar;
                        }
                        else
                        {
                            //if (Constants.VALID_TYPES.Contains(currentToken))
                            if (currentToken == "byte")
                            {
                                tokens.Add(new TypeToken { Text = currentToken });
                            }
                            else
                            {
                                tokens.Add(new IdentifierToken { Text = currentToken });
                            }

                            currentToken = "";
                            currentState = States.None;
                        }
                        break;

                    case States.NumberTokenStarted:
                        if (Char.IsNumber(currentChar))
                        {
                            currentToken += currentChar;
                        }
                        else
                        {
                            tokens.Add(new LiteralToken { Text = currentToken });

                            if (currentToken == ";")
                            {
                                tokens.Add(new SemicolonToken { Text = currentToken });
                            }
                            
                            currentToken = "";
                            currentState = States.None;
                        }
                        break;
                }
            }

            return tokens;
        }
    }

    public abstract class Command
    {
    }

    public class SimpleCommand : Command
    {
    }

    public class ComplexCommand : Command
    {
        IList<Command> InnerCommands { get; set; }
    }
}
