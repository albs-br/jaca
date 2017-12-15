using Assembler.Entities.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Entities
{
    public class Line
    {
        public Line(string text)
        {
            Text = text;
            Tokens = new List<Token>();
        }

        public int Address { get; set; }
        public string Text { get; set; }
        public IList<Token> Tokens { get; set; }

        public bool IsLabelDefinition()
        {
            return (Tokens.Count == 1 && Tokens[0] is LabelToken);
        }

        public bool IsVariableDefinition()
        {
            return (Tokens.Count == 2 &&
                    Tokens[0] is DirectiveToken &&
                    Tokens[1] is CommandToken &&
                    Tokens[0].Text == "defbyte");
        }

        public bool IsOrgDirective()
        {
            return (Tokens.Count == 2 &&
                    Tokens[0] is DirectiveToken &&
                    Tokens[1] is LiteralToken &&
                    Tokens[0].Text.ToLower() == "org");
        }
    }
}
