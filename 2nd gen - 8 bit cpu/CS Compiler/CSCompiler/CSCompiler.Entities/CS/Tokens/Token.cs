using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCompiler.Entities.CS.Tokens
{
    public abstract class Token
    {
        public string Text { get; set; }
    }

    public class TypeToken : Token
    {
        public TypeToken(string text)
        {
            Text = text;
        }
    }

    public class IdentifierToken : Token
    {
        public IdentifierToken(string text)
        {
            Text = text;
        }
    }

    public class EqualToken : Token
    {
        public EqualToken(string text)
        {
            Text = text;
        }
    }

    public class LiteralToken : Token
    {
        public LiteralToken(string text)
        {
            Text = text;
        }
    }

    public class SemicolonToken : Token
    {
        public SemicolonToken(string text)
        {
            Text = text;
        }
    }

    public class ArithmeticSignalToken : Token
    {
        public ArithmeticSignalToken(string text)
        {
            Text = text;
        }
    }

}
