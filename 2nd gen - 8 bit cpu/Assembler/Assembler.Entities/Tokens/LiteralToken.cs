using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Entities.Tokens
{
    public class LiteralToken : Token
    {
        public LiteralToken(string text)
        {
            Text = text;
        }
    }
}
