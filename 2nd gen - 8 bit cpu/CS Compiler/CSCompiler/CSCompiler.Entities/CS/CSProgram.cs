using CSCompiler.Entities.CS.Tokens;
using CSCompiler.Entities.MachineCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCompiler.Entities;
using CSCompiler.Exceptions;

namespace CSCompiler.Entities.CS
{
    public class CSProgram
    {
        // Constructor
        public CSProgram()
        {
            this.Commands = new List<Command>();
            this.Variables = new List<Variable>();
        }

        //public string SourceCodeText;

        public IList<Command> Commands { get; set; }
        public IList<Variable> Variables { get; set; }
        //public IList<Function> Functions { get; set; }
    }
}
