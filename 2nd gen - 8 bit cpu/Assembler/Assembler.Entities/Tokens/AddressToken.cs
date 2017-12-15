using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Entities.Tokens
{
    public class AddressToken: NumericToken
    {
        public AddressToken(string text)
        {
            Text = text;
        }
    }
}
