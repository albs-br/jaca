using Assembler.Entities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Entities.Tokens
{
    public class NumericToken : Token
    {
        public int NumericValue
        {
            get
            {
                if (Text.ToLower().StartsWith("0x"))
                {
                    return Convert.ToInt32(Text, 16);
                }
                else if (Text.ToLower().StartsWith("0b"))
                {
                    var tempText = Text.Substring(2); // Remove the "0b" prefix
                    return Convert.ToInt32(tempText, 2);
                }
                else if (this is AscCharToken)
                {
                    var tempText = Text.Substring(1, Text.Length-2); // Remove the single quote (') at beginning and end
                    var charArray = tempText.ToCharArray();

                    if (charArray.Length != 1)
                    {
                        throw new InvalidCommandLineException("Invalid char: " + Text);
                    }

                    var ch = charArray[0];
                    
                    return (int)ch;
                }
                else
                {
                    return Convert.ToInt32(Text, 10);
                }
            }
        }
    }
}
