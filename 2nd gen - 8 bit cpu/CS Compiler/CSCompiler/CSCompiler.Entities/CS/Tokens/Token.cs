using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCompiler.Entities.CS.Tokens
{
    public abstract class Token
    {
        //public Token(string text)
        //{
        //    Text = text;
        //}

        public string Text { get; set; }
    }

    public class TypeToken : Token
    { 
    }

    public class IdentifierToken : Token
    {
    }

    public class EqualToken : Token
    {
    }

    public class LiteralToken : Token
    {
    }

    public class SemicolonToken : Token
    {
    }
}
