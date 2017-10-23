    using CSCompiler.Entities.CS.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCompiler.Entities.CS
{
    public abstract class Command
    {
        public Command()
        {
            this.Tokens = new List<Token>();
        }

        public CSProgram csProgram { get; set; }

        public IList<Token> Tokens { get; set; }

        public abstract IList<byte> MachineCode();
    }
}
