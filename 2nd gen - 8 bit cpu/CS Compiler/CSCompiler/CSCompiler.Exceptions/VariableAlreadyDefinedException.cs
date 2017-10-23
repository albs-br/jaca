using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCompiler.Exceptions
{
    public class VariableAlreadyDefinedException : Exception
    {
        public VariableAlreadyDefinedException(string msg)
            : base(msg)
        { }
    }
}
