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
            TypeOrIdentifierTokenStarted,
            LiteralTokenStarted,
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
                                tokens.Add(new EqualToken { Text = currentChar.ToString() });
                                currentToken = "";
                            }
                            else if (currentChar == ';')
                            {
                                tokens.Add(new SemicolonToken { Text = currentChar.ToString() });
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
                                tokens.Add(new TypeToken { Text = currentToken });
                            }
                            else
                            {
                                tokens.Add(new IdentifierToken { Text = currentToken });

                                if (currentChar == '=')
                                {
                                    tokens.Add(new EqualToken { Text = currentChar.ToString() });
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
                            tokens.Add(new LiteralToken { Text = currentToken });

                            if (currentChar == ';')
                            {
                                tokens.Add(new SemicolonToken { Text = currentChar.ToString() });
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
