using CSCompiler.Entities.CS.Tokens;
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
        /// Step 1 of 3 - Convert source to tokens
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IList<Token> ConvertSourceToTokens(string source)
        {
            TokenStates currentState = TokenStates.None;

            var tokens = new List<Token>();
            var currentToken = "";
            for (int i = 0; i < source.Length; i++)
            {
                char currentChar = source.ElementAt(i);
                char previousChar = (i == 0) ? char.MaxValue : source.ElementAt(i - 1);

                switch (currentState)
                {
                    case TokenStates.None:
                        if (Constants.IsIrrelevantChar(currentChar))
                        {
                            continue;
                        }
                        else
                        {
                            if (Char.IsLetter(currentChar))
                            {
                                currentState = TokenStates.TypeOrIdentifierTokenStarted;
                                currentToken = currentChar.ToString();
                            }
                            else if (Char.IsNumber(currentChar))
                            {
                                currentState = TokenStates.LiteralTokenStarted;
                                currentToken = currentChar.ToString();
                            }
                            else if (currentChar == '=' && previousChar == '=')
                            {
                                tokens.Remove(tokens.Last());
                                tokens.Add(new ComparisonToken(currentChar.ToString() + previousChar.ToString()));
                                currentToken = "";
                            }
                            else
                            {
                                CheckSingleTokens(tokens, currentChar);

                                currentToken = "";
                            }
                        }
                        break;

                    case TokenStates.TypeOrIdentifierTokenStarted:
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

                                //currentToken = "";
                            }
                            else if (Constants.IsCommand(currentToken))
                            {
                                tokens.Add(new CommandToken(currentToken));

                                //currentToken = "";
                            }
                            else if (Constants.IsKeyword(currentToken))
                            {
                                tokens.Add(new KeywordToken(currentToken));

                                //currentToken = "";
                            }
                            else
                            {
                                tokens.Add(new IdentifierToken(currentToken));
                            }

                            CheckSingleTokens(tokens, currentChar);

                            currentToken = "";

                            currentState = TokenStates.None;
                        }
                        break;

                    case TokenStates.LiteralTokenStarted:
                        if (Char.IsNumber(currentChar))
                        {
                            // Literal continues
                            currentToken += currentChar;
                        }
                        else
                        {
                            // Literal ends
                            tokens.Add(new LiteralToken(currentToken));

                            CheckSingleTokens(tokens, currentChar);

                            currentToken = "";
                            currentState = TokenStates.None;
                        }
                        break;
                }
            }

            return tokens;
        }

        private static void CheckSingleTokens(List<Token> tokens, char currentChar)
        {
            if (Constants.IsArithmeticSignal(currentChar))
            {
                tokens.Add(new ArithmeticSignalToken(currentChar.ToString()));
            }
            else
            {
                switch (currentChar)
                {
                    case '=':
                        tokens.Add(new EqualToken());
                        break;

                    case ';':
                        tokens.Add(new SemicolonToken());
                        break;

                    case '(':
                        tokens.Add(new OpenParenthesisToken());
                        break;

                    case ')':
                        tokens.Add(new CloseParenthesisToken());
                        break;

                    case ',':
                        tokens.Add(new CommaToken());
                        break;

                    case '{':
                        tokens.Add(new OpenBracesToken());
                        break;

                    case '}':
                        tokens.Add(new CloseBracesToken());
                        break;


                    default:
                        break;
                }
            }
        }
    }
}
