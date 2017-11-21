using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Entities.Exceptions
{
    public class VariableNotResolvedException : Exception
    {
        public VariableNotResolvedException(string variable)
            : base("Variable " + variable + " not resolved.")
        {
        }
    }
}
