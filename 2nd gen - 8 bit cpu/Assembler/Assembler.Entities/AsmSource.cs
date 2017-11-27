using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Entities
{
    public class AsmSource
    {
        public AsmSource()
        {
            Lines = new List<Line>();
        }

        public string Text { get; set; }
        public IList<Line> Lines { get; set; }
    }
}
