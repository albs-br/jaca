using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Entities.Exceptions
{
    public class InvalidCommandLineException : Exception
    {
        public InvalidCommandLineException(string message)
            : base(message)
        {
        }
    }
}
