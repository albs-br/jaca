using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCompiler.Exceptions
{
    public class VariableOutsideOfRangeException : Exception
    {
        public VariableOutsideOfRangeException(string variableName)
            : base(String.Format("Value of variable {0} outside of range.", variableName))
        { }
    }
}
