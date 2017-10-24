using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCompiler.Exceptions
{
    public class UndefinedVariableException : Exception
    {
        public UndefinedVariableException(string variableName)
            : base(String.Format("Variable {0} has not been defined.", variableName))
        { }
    }
}
