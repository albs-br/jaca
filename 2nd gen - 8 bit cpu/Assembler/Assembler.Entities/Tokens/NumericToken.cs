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
                else
                {
                    return Convert.ToInt32(Text, 10);
                }
            }
        }
    }
}
