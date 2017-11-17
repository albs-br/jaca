using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCompiler.Exceptions
{
    public class UnmatchingBracesException : Exception
    {
        public UnmatchingBracesException()
            : base(String.Format("Program has unmatching number of braces."))
        { }
    }
}
