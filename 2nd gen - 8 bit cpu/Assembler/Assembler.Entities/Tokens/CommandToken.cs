using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Entities.Tokens
{
    public class CommandToken : Token
    {
        public CommandToken(string text)
        {
            Text = text;
        }
    }
}
