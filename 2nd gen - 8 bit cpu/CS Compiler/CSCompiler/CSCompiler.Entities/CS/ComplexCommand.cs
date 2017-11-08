using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCompiler.Entities.CS
{
    public abstract class ComplexCommand : Command
    {
        public ComplexCommand()
        {
            InnerCommands = new List<Command>();
        }

        public IList<Command> InnerCommands { get; set; }
    }
}
