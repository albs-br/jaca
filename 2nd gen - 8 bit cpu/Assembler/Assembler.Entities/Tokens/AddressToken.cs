using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Entities.Tokens
{
    public class AddressToken: Token
    {
        public AddressToken(string text)
        {
            Text = text;
        }

        public int NumericValue
        {
            get
            {
                int numericBase;
                if (Text.ToLower().StartsWith("0x"))
                {
                    numericBase = 16;
                }
                else
                {
                    numericBase = 10;
                }
                return Convert.ToInt32(Text, numericBase);
            }
        }
    }
}
