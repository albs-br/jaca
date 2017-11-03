using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCompiler.Entities.CS
{
    public abstract class ComplexCommand : Command
    {
        protected IList<Command> InnerCommands { get; set; }
    }
}
