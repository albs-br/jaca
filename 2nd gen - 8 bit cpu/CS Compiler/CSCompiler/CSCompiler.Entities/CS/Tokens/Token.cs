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

        public string TokenToString()
        {
            var output = Text + " ";

            if (this is SemicolonToken || this is OpenBracesToken || this is CloseBracesToken)
            {
                output += Environment.NewLine;
            }

            return output;
        }
    }

    public abstract class OperandToken : Token
    {
    }

    public class TypeToken : Token
    {
        public TypeToken(string text)
        {
            Text = text;
        }
    }

    public class IdentifierToken : OperandToken
    {
        public IdentifierToken(string text)
        {
            Text = text;
        }
    }

    public class EqualToken : Token
    {
        public EqualToken()
        {
            Text = "=";
        }
    }

    public class LiteralToken : OperandToken
    {
        public LiteralToken(string text)
        {
            Text = text;
        }
    }

    public class SemicolonToken : Token
    {
        public SemicolonToken()
        {
            Text = ";";
        }
    }

    public class ArithmeticSignalToken : Token
    {
        public ArithmeticSignalToken(string text)
        {
            Text = text;
        }
    }

    public class OpenParenthesisToken : Token
    {
        public OpenParenthesisToken()
        {
            Text = "(";
        }
    }

    public class CloseParenthesisToken : Token
    {
        public CloseParenthesisToken()
        {
            Text = ")";
        }
    }

    public class CommaToken : Token
    {
        public CommaToken()
        {
            Text = ",";
        }
    }

    public class CommandToken : Token
    {
        public CommandToken(string text)
        {
            Text = text;
        }
    }

    /// <summary>
    /// Comparison Token (==, !=, >, <, >= , <=)
    /// </summary>
    public class ComparisonToken : Token
    {
        public ComparisonToken(string text)
        {
            Text = text;
        }
    }

    /// <summary>
    /// Keyword Token (if, for, etc)
    /// </summary>
    public class KeywordToken : Token
    {
        public KeywordToken(string text)
        {
            Text = text;
        }
    }

    public class OpenBracesToken : Token
    {
        public OpenBracesToken()
        {
            Text = "{";
        }
    }

    public class CloseBracesToken : Token
    {
        public CloseBracesToken()
        {
            Text = "}";
        }
    }
}
