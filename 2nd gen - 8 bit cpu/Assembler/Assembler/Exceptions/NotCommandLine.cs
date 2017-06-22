using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Exceptions
{
    public class NotCommandLineException : Exception
    {
        public NotCommandLineException(string message)
            : base(message)
        { 
        }
    }
}
