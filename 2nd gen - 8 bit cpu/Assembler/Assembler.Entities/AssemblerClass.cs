using Assembler.Entities.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Entities
{
    public static class AssemblerClass
    {
        private enum StateMachine 
        { 
            None,
            CommandStarted,
            LiteralStarted,
            LabelStarted
        }

        private static char[] irrelevantChars = { ' ', '\t' };

        public static void ConvertToTokens(AsmSource asmSource)
        {
            var lines = asmSource.Text.Split(
                    new[] { Environment.NewLine },
                    StringSplitOptions.None
                );


            foreach (var line in lines)
            {
                var state = StateMachine.None;

                var newLine = new Line(line);

                for (var counter = 0; counter < line.Length; counter++)
                {
                    char currentChar = asmSource.Text[counter];
                    var currentToken = "";

                    switch (state)
                    {
                        case StateMachine.None:
                            if (irrelevantChars.Contains(currentChar))
                            {
                                continue;
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
                                currentToken = currentChar.ToString();
                            }
                            break;
                        
                        //case StateMachine.CommandStarted:
                            
                        //    if (Char.IsLetter(currentChar))
                        //    {
                        //        currentToken += currentChar;
                        //        continue;
                        //    }
                        //    else
                        //    {
                        //        currentToken = currentChar.ToString();
                        //        state = StateMachine.None;
                        //        newLine.Tokens.Add(new CommandToken(currentToken));
                        //    }
                        //    break;
                        
                        //case StateMachine.LiteralStarted:
                        //    currentToken += currentChar;
                            
                        //    if (Char.IsDigit(currentChar))
                        //    {
                        //        continue;
                        //    }
                        //    else
                        //    {
                        //        state = StateMachine.None;
                        //        newLine.Tokens.Add(new LiteralToken(currentToken));
                        //    }
                        //    break;
                        
                        default:
                            break;
                    }
                }

                asmSource.Lines.Add(newLine);
            }
        }
    }
}
