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
        }

        public int Address { get; set; }
        public string Text { get; set; }
        public IList<Token> Tokens { get; set; }
    }
}
