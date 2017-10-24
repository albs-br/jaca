using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCompiler.Exceptions
{
    public class InvalidInstructionFormatException : Exception
    {
        public InvalidInstructionFormatException(string instruction)
            : base(String.Format("Instruction '{0}' is in invalid format.", instruction))
        { }
    }
}
