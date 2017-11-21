using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Entities.Exceptions
{
    public class LabelNotResolvedException : Exception
    {
        public LabelNotResolvedException(string label)
            : base("Label " + label + " not resolved.")
        {
        }
    }
}
