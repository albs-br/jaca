using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCompiler.Entities.CS
{
    public class SimpleCommand : Command
    {
        public override IList<byte> MachineCode()
        {
            return null;
        }
    }
}
